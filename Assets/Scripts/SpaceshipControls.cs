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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(moveDirection.x * movingSpeed, moveDirection.y * movingSpeed);
    }

    public void MoveUp()
    {
        Debug.Log("Up");
        var position = cubeTransform.position;
        float y = position.y;
        y += movingSpeed/50f;
        var newPosition = new Vector3(position.x, y, position.z);
        cubeTransform.position = newPosition;
    }

    public void MoveDown()
    {
        var position = cubeTransform.position;
        float y = position.y;
        y -= movingSpeed/50f;
        var newPosition = new Vector3(position.x, y, position.z);
        cubeTransform.position = newPosition;
    }

    public void MoveLeft()
    {
        var position = cubeTransform.position;
        float x = position.x;
        x -= movingSpeed/50f;
        var newPosition = new Vector3(x, position.y, position.z);
        cubeTransform.position = newPosition;
    }

    public void MoveRight()
    {
        var position = cubeTransform.position;
        float x = position.x;
        x += movingSpeed/50f;
        var newPosition = new Vector3(x, position.y, position.z);
        cubeTransform.position = newPosition;
    }
}
