using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class LookAround : MonoBehaviour
{
    public GameObject flashlight;
    public float roationSpeed = 1f;

    private void Awake()
    {
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Look.performed += LookAt;
        playerInputActions.Player.Enable();
    }

    public void LookAt(InputAction.CallbackContext context)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        float angle = Vector2.Angle(flashlight.transform.up, mousePos);
        flashlight.transform.Rotate(flashlight.transform.forward.normalized * roationSpeed *Time.deltaTime, angle);


    }

}
