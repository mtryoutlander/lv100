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
    public float fallSpeed=5 ;
    [HideInInspector]public bool isPaused { get; set; }
    [HideInInspector]public bool topLedge, bottomLedge, wall;


    private Animator animate;
    private Vector2 velocity, moveInput;
    private Rigidbody2D rb;
    
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
        playerInputActions.Player.Clime.canceled += StopClime;
        playerInputActions.Player.Crawing.performed += OnCrouch;
        playerInputActions.Player.Crawing.canceled += StopCrouch;    
        playerInputActions.Player.Enable();
        coroutine = AnimatieWait();
    }
    private void OnDestroy()
    {
        PAUSE_EVENT.Pause -= Pause;
        PAUSE_EVENT.Pause -= Resume;
    }
    void FixedUpdate()
    {
        if (isPaused)  // if game is paused
            return;
        // if player trying walk off ledge
        if (bottomLedge && new Vector3(moveInput.normalized.x, moveInput.normalized.y) == transform.right.normalized)  
            return;
        //diffrent kinds of movement
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
            case actionState.idle:                rb.MovePosition(rb.position + moveInput * speed * Time.deltaTime);
                animate.SetFloat("WalkingSpeed", moveInput.magnitude);
                break;
        }
            
    }


    private void Update()
    {

        if(state == actionState.clime)
        {
            //if player is at top of wall clime up 
            if (topLedge)
            {
                Vector3 dir = transform.right + (transform.up * 2.5f);
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
            Vector3 dir = (transform.right *1.3f) + (-transform.up * 2.8f);
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
    //call this function when you want to wait for the animation to finish
    //startcoroutine(AnimatieWait());
    IEnumerator AnimatieWait()
    {
        yield return new WaitForSeconds(animate.GetCurrentAnimatorStateInfo(0).length);
        state = actionState.idle;
    }
}
