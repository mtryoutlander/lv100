using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerMovemetnControler : MonoBehaviour
{
    public float speed, climeSpeed, crawlSpeed;
    public float fallSpeed=5 ;
    
    public bool isPaused { get; set; }
    private Animator animate;

    private GameObject wall;
    private Vector2 velocity;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    enum actionState { clime, crawl, walk, idle};
    actionState state = actionState.walk; 

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
    }

   

   private void OnTriggerEnter2D(Collider2D other)
    {
        wall = other.gameObject;
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
                rb.MovePosition(rb.position + moveInput * speed * Time.deltaTime);
                animate.SetFloat("WalkingSpeed", moveInput.magnitude);
                break;
            case actionState.idle:
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
        state = actionState.walk;
        animate.SetFloat("CrawlSpeed", 0f);

        animate.SetBool("Crawling", false);
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

    
}
