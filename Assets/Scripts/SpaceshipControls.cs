using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipControls : MonoBehaviour
{
    [SerializeField] private int movingSpeed = 2;
    [SerializeField] private Transform cubeTransform;
    [SerializeField] private Rigidbody rigidbody;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 5f;

    public InputActionReference move;
    public InputActionReference fire;

    private Vector2 moveDirection;
    private Camera mainCamera;
    private Vector2 screenBounds;
    private Vector2 objectSize;

    private Animator animator;


    private void Start()
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
        var collider = GetComponent<Collider>();
        if (collider != null)
        {
            var bounds = collider.bounds;
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
        if (fire.action.triggered)
        {
            Fire();
        }
    }

    private void FixedUpdate()
    {
        Debug.Log("moving direction" + moveDirection);
        // Move the cube
        rigidbody.velocity = new Vector2(moveDirection.x * movingSpeed, moveDirection.y * movingSpeed);

        // Calculate screen bounds dynamically considering aspect ratio
        var screenRatio = (float)Screen.width / Screen.height;
        var orthoSize = mainCamera.orthographicSize;
        screenBounds = new Vector2(orthoSize * screenRatio, orthoSize);

        // Clamp the cube's position to keep it within the screen bounds
        var clampedPosition = transform.position;
        clampedPosition.x =
            Mathf.Clamp(clampedPosition.x, -screenBounds.x + objectSize.x, screenBounds.x - objectSize.x);
        clampedPosition.y =
            Mathf.Clamp(clampedPosition.y, -screenBounds.y + objectSize.y, screenBounds.y - objectSize.y);
        transform.position = clampedPosition;
        
        
        
        // Pass the moveDirection directly to the Animator to drive the Blend Tree
        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);

        // Logging movement for debugging
        if (moveDirection == Vector2.zero)
        {
            Debug.Log("Idle");
        }
        else
        {
            if (moveDirection.x > 0)
            {
                Debug.Log("Moving Right");
            }
            else if (moveDirection.x < 0)
            {
                Debug.Log("Moving Left");
            }
            if (moveDirection.y > 0)
            {
                Debug.Log("Moving Up");
            }
            else if (moveDirection.y < 0)
            {
                Debug.Log("Moving Down");
            }
        }

    }

   public void Fire()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        if (bullet.TryGetComponent<Rigidbody2D>(out var rigidBody))
        {
            rigidBody.velocity = transform.up * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Rigidbody2D component not found on the bullet prefab.");
        }
    }
}