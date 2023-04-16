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
    

    private Vector2 velocity;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private void PlayerInput_onActionTriggered(InputAction.CallbackContext contex)
    {
        Debug.Log("For the love of god");
        Debug.Log(contex);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        PAUSE_EVENT.Pause += Pause;
        PAUSE_EVENT.Resume += Resume;
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnStop;
        playerInputActions.Player.Look.performed += OnLook;
        playerInputActions.Player.Enable();
    }

    private void OnLook(InputAction.CallbackContext obj)
    { 
        //check if the obj.readvalue vector2 is left or right of the player
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(obj.ReadValue<Vector2>());
        Vector2 dif = mousePos - transform.position;
        Debug.Log("mouse pos x "+mousePos.x +" , player position x " + transform.position.x + ",  the dif " + dif.x);
        if (dif.x > 0)
            FaceRight();
        else
            FaceLeft();

    }
    private void FaceRight()
    {
        if (transform.rotation.y != 0)
        {
            transform.SetLocalPositionAndRotation(transform.position, new Quaternion(0, 0, 0, 0));
        }
    }
    private void FaceLeft() 
    {
        if (transform.rotation.y != 180)
        {
            transform.SetLocalPositionAndRotation(transform.position, new Quaternion(0,180,0,0));
        }

    }

    private void OnStop(InputAction.CallbackContext obj)
    {
        moveInput = Vector2.zero;
    }

    private void OnDestroy()
    {
        PAUSE_EVENT.Pause -= Pause;
        PAUSE_EVENT.Pause -= Resume;
    }

    void OnMove(InputAction.CallbackContext contex)
    {
        moveInput = contex.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if (!isPaused)
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
