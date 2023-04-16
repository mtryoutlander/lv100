using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    //get input from new input system
    private PlayerInput input;
    private Movement movement;
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        
    }
}
