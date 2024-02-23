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

    void Start()
    {
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        objectSize = GetObjectBoundsSize();
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
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(moveDirection.x * movingSpeed, moveDirection.y * movingSpeed);

        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectSize.x, screenBounds.x - objectSize.x);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectSize.y, screenBounds.y - objectSize.y);
        transform.position = viewPos;
    }

    public void Fire()
    {
        //TODO Fire bullets
        Debug.Log("Fire");
    }
}