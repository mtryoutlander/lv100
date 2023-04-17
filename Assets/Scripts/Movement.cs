using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public float maxSpeed = 5f;
    public bool isPaused { get; set; }
    private Animator animate;

    private GameObject wall;
    private Vector2 velocity;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool climing = false;

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

    private void OnCrouch(InputAction.CallbackContext obj)
    {
        animate.SetBool("crawling", true);
    }

    private void StopCrouch(InputAction.CallbackContext obj)
    {
        animate.SetBool("crawling", false);
    }

   private void OnTriggerEnter2D(Collider2D other)
    {
        wall = other.gameObject;
        Debug.Log(wall);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        wall = null;
        climing = false;
        animate.SetBool("climeing", false);
        Debug.Log("stop climbing");

        rb.gravityScale = 10;
    }

    private void OnClimb(InputAction.CallbackContext obj)
    {
        if(wall != null)
        {
            Debug.Log("climbing");
            rb.gravityScale = 1;
            climing= true;
            animate.SetBool("climeing", true);
            
        }
    }
    private void StopClimb(InputAction.CallbackContext obj)
    {
        climing = false;
        animate.SetBool("climeing", false);
        Debug.Log("stop climbing");
        rb.gravityScale = 10;
    }
    private void OnLook(InputAction.CallbackContext obj)
    { 
        //check if the obj.readvalue vector2 is left or right of the player
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(obj.ReadValue<Vector2>());
        Vector2 dif = mousePos - transform.position;
        //Debug.Log("mouse pos x "+mousePos.x +" , player position x " + transform.position.x + ",  the dif " + dif.x);
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
                new Quaternion(transform.rotation.x, 1,transform.rotation.z,0));
        }

    }

    private void StopMoving(InputAction.CallbackContext obj)
    {
        moveInput = Vector2.zero;
        animate.SetBool("walking", false);
    }

    private void OnDestroy()
    {
        PAUSE_EVENT.Pause -= Pause;
        PAUSE_EVENT.Pause -= Resume;
    }

    void OnMove(InputAction.CallbackContext contex)
    {
        moveInput = contex.ReadValue<Vector2>();
        animate.SetBool("walking", true);
    }

    void FixedUpdate()
    {
        if (isPaused)
            return;
        if (climing)
            rb.MovePosition(new Vector2(rb.position.x, rb.position.y + moveInput.y * speed * Time.fixedDeltaTime));
        else
            rb.MovePosition(new Vector2( rb.position.x + moveInput.x *speed *Time.fixedDeltaTime, rb.position.y));
        
    }

    public void Pause()
    {
        Debug.Log("Pause");
        isPaused = true;
        velocity = rb.velocity;
        rb.velocity = Vector2.zero;

    }

    public void Resume()
    {
        Debug.Log("resume");
        isPaused = false;
        rb.velocity = velocity;
    }

    
}
