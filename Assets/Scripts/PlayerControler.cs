using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    //get input from new input system
    private PlayerInput input;
    private PlayerMovemetnControler movement;
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovemetnControler>();
    }

    private void Update()
    {
        
    }
}
