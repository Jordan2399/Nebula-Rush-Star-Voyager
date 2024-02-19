using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeControls : MonoBehaviour
{
    [SerializeField] private int movingSpeed = 2;
    [SerializeField] private Transform cubeTransform;
    [SerializeField] private Rigidbody rigidbody;

    public InputActionReference move;

    private Vector2 moveDirection;
    
    private void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(moveDirection.x * movingSpeed, moveDirection.y * movingSpeed);
    }

    public void Fire()
    {
        //TODO Fire bullets
        Debug.Log("Fire");
    }
}
