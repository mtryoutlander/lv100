using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerMovementControler : MonoBehaviour
{
    public float speed, climeSpeed, crawlSpeed;
    public float fallSpeed=5, damageEnemy = 10;
    public OxgenCounter oxgenCounter;
    [HideInInspector]public bool isPaused { get; set; }
    [HideInInspector]public bool topLedge, bottomLedge, wall, ground;

    private float timeFell = 0;
    [SerializeField]private float timePlayerCanFall = 3;
    [SerializeField]private float timeDamageMultiplyer = 10;
    private Animator animate;
    private Vector2 velocity, moveVerticalInput, moveHorizontalInput;   // make vertical and horizontal input = the input 
    private Rigidbody2D rb;
    
    enum actionState { clime, crawl, walk, idle, topOfClime, run, falling, stop};
    actionState state = actionState.idle;
    private IEnumerator coroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animate = GetComponent<Animator>();
        PAUSE_EVENT.Pause += Pause;
        PAUSE_EVENT.Resume += Resume;
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.MoveVertical.performed += OnMoveVertical;
        playerInputActions.Player.MoveVertical.canceled += StopMoving;
        playerInputActions.Player.MoveHorizontal.performed += OnMoveHorizontal;
        playerInputActions.Player.MoveHorizontal.canceled += StopMoving;
        playerInputActions.Player.Look.performed += OnLook;
        playerInputActions.Player.Clime.performed += OnClimb;
        playerInputActions.Player.Clime.canceled += StopClime;
        playerInputActions.Player.Crawing.performed += OnCrouch;
        playerInputActions.Player.Crawing.canceled += StopCrouch;
        playerInputActions.Player.Sprint.performed += OnSprint;
        playerInputActions.Player.Sprint.canceled += StopSprint;
        playerInputActions.Player.Enable();
        coroutine = AnimatieWait();
    }

  

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            TakeDamage(damageEnemy);
        Debug.Log("collisionEnter Time Fell" + timeFell);
        if (timeFell > timePlayerCanFall)
        {
            state = actionState.idle;
            TakeDamage(timeFell*timeDamageMultiplyer);
        }
        timeFell= 0;
    }

    private void OnDestroy()
    {
        PAUSE_EVENT.Pause -= Pause;
        PAUSE_EVENT.Pause -= Resume;
    }
    void FixedUpdate()
    {
        if (!ground && state != actionState.clime)
        {
            state = actionState.falling;
            Debug.Log("Started falling");
        }else
            timeFell =0;

        if (isPaused)  // if game is paused
            return;
        if (bottomLedge && new Vector3(moveHorizontalInput.normalized.x, moveHorizontalInput.normalized.y) == transform.right 
            || (bottomLedge && moveVerticalInput.normalized.y == -1) )
            return;
        //diffrent kinds of movement
        switch (state)
        {
            case actionState.clime:
                oxgenCounter.oxgenLossRate = 1;
                rb.MovePosition(rb.position+ moveVerticalInput * climeSpeed* Time.deltaTime);
                animate.SetFloat("ClimeSpeed", moveVerticalInput.magnitude);
                animate.SetFloat("Speed", moveVerticalInput.magnitude);
                break;
            
            case actionState.crawl:
                oxgenCounter.oxgenLossRate = 1;
                rb.MovePosition(rb.position + moveHorizontalInput * crawlSpeed * Time.deltaTime);
                animate.SetFloat("CrawlSpeed", moveHorizontalInput.magnitude);
                animate.SetFloat("Speed", moveHorizontalInput.magnitude);
                break;
            case actionState.walk:
            case actionState.idle:
                oxgenCounter.oxgenLossRate = 1;
                rb.MovePosition(rb.position + moveHorizontalInput * speed * Time.deltaTime);
                animate.SetFloat("WalkingSpeed", moveHorizontalInput.magnitude);
                animate.SetFloat("Speed", moveHorizontalInput.magnitude);
                break;
            case actionState.run:
                oxgenCounter.oxgenLossRate = 3;
                rb.MovePosition(rb.position + moveHorizontalInput * (speed*2) * Time.deltaTime);
                animate.SetFloat("WalkingSpeed", moveHorizontalInput.magnitude*2);
                animate.SetFloat("Speed", moveHorizontalInput.magnitude);
                break;
            case actionState.falling:
                
                break;
        }
            
    }

    private void Update()
    {
        
        if (state == actionState.falling)
        {
            timeFell += Time.deltaTime;
        }
        if (state == actionState.clime)
        {
            //if player is at top of wall clime up 
            if (topLedge)
            {
                Vector3 dir = (transform.right * 2.5f ) + (transform.up * 5f);
                transform.position = transform.position + dir;
                state = actionState.walk;
                animate.SetFloat("ClimeSpeed", 0f);
                animate.SetBool("Climeing", false);
                animate.SetTrigger("ClimeUp");
                rb.gravityScale = fallSpeed;
                StartCoroutine(AnimatieWait());
            }

        }
    }
    private void OnMoveHorizontal(InputAction.CallbackContext obj)
    {
        moveHorizontalInput = obj.ReadValue<Vector2>();
    }

    private void OnMoveVertical(InputAction.CallbackContext obj)
    {
        moveVerticalInput = obj.ReadValue<Vector2>();
    }

    private void AddOxgen(int amount)
    {
        oxgenCounter.oxgen += amount;
    }

    
    private void TakeDamage(float amount)
    {
        oxgenCounter.oxgen -= amount;
    }
    

    private void OnCrouch(InputAction.CallbackContext obj)
    {
        state= actionState.crawl;
        animate.SetBool("Crawling", true);
    }

    private void StopCrouch(InputAction.CallbackContext obj)
    {
        state = actionState.stop;
        animate.SetFloat("CrawlSpeed", 0f);
        animate.SetBool("Crawling", false);

        rb.gravityScale = fallSpeed;

    }

    private void OnClimb(InputAction.CallbackContext obj)
    {
        if (bottomLedge)  ///clime down
        {
            Vector3 dir = (transform.right *3f) + (-transform.up * 4.5f);
            transform.position = transform.position + dir;
            rb.gravityScale = 0;
            if (dir.x > 0)
                FaceLeft();
            else
                FaceRight();
            animate.SetTrigger("ClimeDown");
            animate.SetBool("Climeing", true);
            state = actionState.clime;
        }
        else if (wall) //clime up
        {
            rb.gravityScale = 0;
            state = actionState.clime;
            animate.SetBool("Climeing", true);
        }
        
    }
    private void StopClime(InputAction.CallbackContext obj)
    {
        Debug.Log("STOP Climeing");
        state = actionState.walk;
        animate.SetFloat("ClimeSpeed", 0f);
        animate.SetBool("Climeing", false);
        rb.gravityScale = fallSpeed;
    }
    private void OnLook(InputAction.CallbackContext obj)
    { 
        if(state == actionState.clime || state == actionState.topOfClime)
            return;
        //check if the obj.readvalue vector2 is left or right of the player
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(obj.ReadValue<Vector2>());
        Vector2 dif = mousePos - transform.position;
        if (dif.x > 0)
            FaceRight();
        else
            FaceLeft();

    }
    private void FaceRight()
    {
        if (transform.rotation.y != 0)
        {
            transform.SetLocalPositionAndRotation(transform.position,
                new Quaternion(transform.rotation.x, 0, transform.rotation.z, 0));
        }
    }
    private void FaceLeft() 
    {
        if (transform.rotation.y != 180)
        {
            transform.SetLocalPositionAndRotation(transform.position,
                new Quaternion(transform.rotation.x, 1, transform.rotation.z,0));
        }

    }

    private void StopSprint(InputAction.CallbackContext obj)
    {
        state = actionState.walk;
    }

    private void OnSprint(InputAction.CallbackContext obj)
    {
        state = actionState.run;
    }


    private void StopMoving(InputAction.CallbackContext obj)
    {
        moveVerticalInput = Vector2.zero; 
        moveHorizontalInput = Vector2.zero;
        animate.SetFloat("WalkingSpeed", moveVerticalInput.magnitude);
        animate.SetFloat("Speed", moveVerticalInput.magnitude);
    }

    public void Pause()
    {
        isPaused = true;
        velocity = rb.velocity;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;

    }

    public void Resume()
    {
        isPaused = false;
        rb.velocity = velocity;
        rb.gravityScale = fallSpeed;
    }
    //call this function when you want to wait for the animation to finish
    //startcoroutine(AnimatieWait());
    IEnumerator AnimatieWait()
    {
        yield return new WaitForSeconds(animate.GetCurrentAnimatorStateInfo(0).length);
        state = actionState.idle;
    }
}
