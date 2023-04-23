using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class LookAround : MonoBehaviour
{
    public GameObject head;
    public float roationSpeed = 1f;

    private enum lookAngle {up,center,down,floor };
    private lookAngle angle = lookAngle.center;
    private bool leftOrRight =false; // false = left, true = right

    private void Awake()
    {
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Look.performed += LookAt;
        playerInputActions.Player.Enable();
    }

    public void LookAt(InputAction.CallbackContext context)
    {
        Vector3 mousePos =  Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        mousePos -= head.transform.position;
        if(mousePos.x > 0)
            leftOrRight= true;
        else
            leftOrRight= false;
        if(mousePos.y > 2)
            angle = lookAngle.up;
        else
            angle = lookAngle.center;
        if(mousePos.y <-1)
            angle = lookAngle.down;
        if(mousePos.y <-2)
            angle = lookAngle.floor;
       // float angle = Vector2.Angle(flashlight.transform.up, mousePos);
       // flashlight.transform.Rotate(flashlight.transform.forward.normalized * roationSpeed *Time.deltaTime, angle);
    }
    private void Update()
    {
        TurnHead();
        
    }

    private void TurnHead()
    {
        if(leftOrRight)
            switch (angle)
            {
                case lookAngle.up:
                    head.transform.rotation = Quaternion.Slerp(head.transform.rotation, Quaternion.Euler(0, 0, -45), roationSpeed * Time.deltaTime);
                    break;
                case lookAngle.center:
                    head.transform.rotation = Quaternion.Slerp(head.transform.rotation, Quaternion.Euler(0, 0, -90), roationSpeed * Time.deltaTime);
                    break;
                case lookAngle.down:
                    head.transform.rotation = Quaternion.Slerp(head.transform.rotation, Quaternion.Euler(0, 0, -120f), roationSpeed * Time.deltaTime);
                    break;
                case lookAngle.floor:
                    head.transform.rotation = Quaternion.Slerp(head.transform.rotation, Quaternion.Euler(0, 0, -150f), roationSpeed * Time.deltaTime);
                    break;
            }
        else
            switch (angle)
            {
                case lookAngle.up:
                    head.transform.rotation = Quaternion.Slerp(head.transform.rotation, Quaternion.Euler(0, 0, 45), roationSpeed * Time.deltaTime);
                    break;
                case lookAngle.center:
                    head.transform.rotation = Quaternion.Slerp(head.transform.rotation, Quaternion.Euler(0, 0, 90), roationSpeed * Time.deltaTime);
                    break;
                case lookAngle.down:
                    head.transform.rotation = Quaternion.Slerp(head.transform.rotation, Quaternion.Euler(0, 0, 120f), roationSpeed * Time.deltaTime);
                    break;
                case lookAngle.floor:
                    head.transform.rotation = Quaternion.Slerp(head.transform.rotation, Quaternion.Euler(0, 0, 150f), roationSpeed * Time.deltaTime);
                    break;
            }
    }
}
