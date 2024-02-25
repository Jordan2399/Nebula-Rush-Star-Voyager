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
    private Camera mainCamera;
    private Vector2 screenBounds;
    private Vector2 objectSize;

    private Animator animator;


    void Start()
    {
        mainCamera = Camera.main;
        objectSize = GetObjectBoundsSize();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private Vector2 GetObjectBoundsSize()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            Bounds bounds = collider.bounds;
            return new Vector2(bounds.extents.x, bounds.extents.y);
        }
        else
        {
            Debug.LogWarning("No collider found on the object. Cannot determine bounds size.");
            return Vector2.zero;
        }
    }

    private void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
        // Check if the character is not moving
  
    }

    private void FixedUpdate()
    {
        Debug.Log("moving direction" + moveDirection);
        // Move the cube
        rigidbody.velocity = new Vector2(moveDirection.x * movingSpeed, moveDirection.y * movingSpeed);

        // Calculate screen bounds dynamically considering aspect ratio
        float screenRatio = (float)Screen.width / Screen.height;
        float orthoSize = mainCamera.orthographicSize;
        screenBounds = new Vector2(orthoSize * screenRatio, orthoSize);

        // Clamp the cube's position to keep it within the screen bounds
        Vector3 clampedPosition = transform.position;
        clampedPosition.x =
            Mathf.Clamp(clampedPosition.x, -screenBounds.x + objectSize.x, screenBounds.x - objectSize.x);
        clampedPosition.y =
            Mathf.Clamp(clampedPosition.y, -screenBounds.y + objectSize.y, screenBounds.y - objectSize.y);
        transform.position = clampedPosition;
        
        
        
        
        if (moveDirection == Vector2.zero)
        {
            Debug.Log("Idle");
            animator.SetBool("isMovingLeft", false);
            animator.SetBool("isMovingRight", false);
        }
        else // Character is moving
        {
            if (moveDirection.y < 0)
            {
                Debug.Log("Moving Right");
                animator.SetBool("isMovingRight", true);
                animator.SetBool("isMovingLeft", false);
            }
            else if (moveDirection.y > 0)
            {
                Debug.Log("Moving Left");
                animator.SetBool("isMovingLeft", true);
                animator.SetBool("isMovingRight", false);
            }

            // If your game doesn't require specific animations for moving up or down,
            // you don't need to set animator booleans for vertical movement.
            // This example assumes vertical movement does not affect the isMovingLeft or isMovingRight flags.
        }
    }

    public void Fire()
    {
        //TODO Fire bullets
        Debug.Log("Fire");
    }
}