using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public float maxSpeed = 5f;
    public bool isPaused { get; set; }

    public PauseGame pauseGame;
    private Vector2 velocity;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PAUSE_EVENT.Pause += Pause;
        PAUSE_EVENT.Resume += Resume;
    }

    private void OnDestroy()
    {
        PAUSE_EVENT.Pause -= Pause;
        PAUSE_EVENT.Pause -= Resume;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        if (!isPaused)
            rb.MovePosition(rb.position + moveInput *speed *Time.fixedDeltaTime);
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
