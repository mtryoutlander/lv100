using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerMovementControler : MonoBehaviour
{
    public float speed, climeSpeed, crawlSpeed;
    public float fallSpeed=5 ;
    
    public bool isPaused { get; set; }
    private Animator animate;

    private GameObject wall;
    private Vector2 velocity;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private GameObject topOfWall;
    enum actionState { clime, crawl, walk, idle, topOfClime, stop};
    actionState state = actionState.walk;
    private IEnumerator coroutine;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animate = GetComponent<Animator>();
        PAUSE_EVENT.Pause += Pause;
        PAUSE_EVENT.Resume += Resume;
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += StopMoving;
        playerInputActions.Player.Look.performed += OnLook;
        playerInputActions.Player.Clime.performed += OnClimb;
        playerInputActions.Player.Clime.canceled += StopClimb;
        playerInputActions.Player.Crawing.performed += OnCrouch;
        playerInputActions.Player.Crawing.canceled += StopCrouch;    
        playerInputActions.Player.Enable();
        coroutine = AnimatieWait();
    }

   

   private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "ClimeAble")
            wall = other.gameObject;
        if (other.tag == "TopOfWall")
        {
            topOfWall = other.gameObject;
            animate.SetTrigger("TopOfWall");
            state = actionState.topOfClime;
            StartCoroutine(coroutine);
        }
            Debug.Log(wall);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        wall = null;
        state = actionState.walk;
        animate.SetBool("Climeing", false);
        Debug.Log("stop climbing");

        rb.gravityScale = fallSpeed;
    }

    private void OnDestroy()
    {
        PAUSE_EVENT.Pause -= Pause;
        PAUSE_EVENT.Pause -= Resume;
    }
    void FixedUpdate()
    {
        if (isPaused)
            return;
        switch (state)
        {
            case actionState.clime:
                Vector2 fliped = new Vector2(moveInput.y, moveInput.x);
                rb.MovePosition(rb.position+fliped  * climeSpeed* Time.deltaTime);
                animate.SetFloat("ClimeSpeed", moveInput.magnitude);
                break;
            
            case actionState.crawl:
                rb.MovePosition(rb.position + moveInput * crawlSpeed * Time.deltaTime);
                animate.SetFloat("CrawlSpeed", moveInput.magnitude);
                break;
            case actionState.walk:
            case actionState.idle:
                rb.MovePosition(rb.position + moveInput * speed * Time.deltaTime);
                animate.SetFloat("WalkingSpeed", moveInput.magnitude);
                break;
            case actionState.topOfClime:
                //move the tranform to topOfWall position smoothly
                transform.position = Vector2.Lerp(transform.position, topOfWall.transform.position, 0.1f);
                //transform.position = (topOfWall.transform.position);
                Debug.Log("moved to "+ topOfWall.transform.position);
                break;
        }
            
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
        StartCoroutine(AnimatieWait());

    }

    private void OnClimb(InputAction.CallbackContext obj)
    {
        if(wall != null)
        {
            rb.gravityScale = 0;
            state = actionState.clime;
            animate.SetBool("Climeing", true);

        }
    }
    private void StopClimb(InputAction.CallbackContext obj)
    {
        state = actionState.walk;
        animate.SetFloat("ClimeSpeed", 0f);
        animate.SetBool("Climeing", false);
        rb.gravityScale = fallSpeed;
    }
    private void OnLook(InputAction.CallbackContext obj)
    { 
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

    void OnMove(InputAction.CallbackContext obj)
    {
        moveInput = obj.ReadValue<Vector2>();
    }
    private void StopMoving(InputAction.CallbackContext obj)
    {
        moveInput = Vector2.zero; ;
        animate.SetFloat("WalkingSpeed", 0f);
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
    //make a corutien method that changes the state to idle after animate time
    IEnumerator AnimatieWait()
    {
        yield return new WaitForSeconds(animate.GetCurrentAnimatorStateInfo(0).length +1);
        state = actionState.idle;
    }
}
