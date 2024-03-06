using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpaceshipControls : MonoBehaviour
{
    [SerializeField] private Transform cubeTransform;

    [SerializeField] private int movingSpeed = 10;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private GameObject bulletPrefabL1;
    [SerializeField] private GameObject bulletPrefabL2;
    [SerializeField] private GameObject bulletPrefabL3;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int PlayerLavel = 1;

    [SerializeField] private float invincibilityTime = 2.0f;
    private bool isInvincible;

    public HealthBar healthBar;


    private List<SpriteRenderer> spriteRenderers; // Store all sprite renderers
    // [SerializeField] private GameObject shieldGameObject; // Assign in the inspector


    public InputActionReference move;
    public InputActionReference fire;

    private Vector2 moveDirection;
    private Camera mainCamera;
    private Vector2 screenBounds;
    private Vector2 objectSize;

    private Animator animator;
    private Vector3 lastPosition;


    private ScoreManager scoreManager;


    private void Start()
    {
        mainCamera = Camera.main;
        objectSize = GetObjectBoundsSize();
        // currentLives = maxLives;
        lastPosition = transform.position;


        // Find and cache the PlayerScore script on the player object
        /** playerScore = FindObjectOfType<PlayerScore>();
        if (playerScore == null)
        {
            UnityEngine.Debug.LogError("PlayerScore script not found on the player object!");
        }
        **/
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>(true));

        // UpdateHealthFill();
    }

    private Vector2 GetObjectBoundsSize()
    {
        var collider = GetComponent<Collider2D>();
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
        // Debug.Log("moving direction" + moveDirection);
        // Move the cubea
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
        // if (moveDirection == Vector2.zero)
        // {
        //     Debug.Log("Idle");
        // }
        // else
        // {
        //     if (moveDirection.x > 0)
        //     {
        //         Debug.Log("Moving Right");
        //     }
        //     else if (moveDirection.x < 0)
        //     {
        //         Debug.Log("Moving Left");
        //     }
        //     if (moveDirection.y > 0)
        //     {
        //         Debug.Log("Moving Up");
        //     }
        //     else if (moveDirection.y < 0)
        //     {
        //         Debug.Log("Moving Down");
        //     }
        // }

        // float distanceTraveled = Vector3.Distance(transform.position, lastPosition);
        // ScoreManager.Instance.AddDistanceScore(distanceTraveled);
        // lastPosition = transform.position;
    }

    public void Fire()
    {
        var bullet = new GameObject();

        if (PlayerLavel == 1)
        {
            bullet = Instantiate(bulletPrefabL1, firePoint.position, firePoint.rotation);
        }
        else if (PlayerLavel == 2)
        {
            bullet = Instantiate(bulletPrefabL2, firePoint.position, firePoint.rotation);
        }
        else if (PlayerLavel == 3)
        {
            bullet = Instantiate(bulletPrefabL3, firePoint.position, firePoint.rotation);
            
        }


        if (bullet.TryGetComponent<Rigidbody2D>(out var rigidBody))
        {
            rigidBody.velocity = transform.up * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Rigidbody2D component not found on the bullet prefab.");
        }
    }


    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerBullet") && !isInvincible)
        {
            Debug.Log("Player Collided with something other that playerBullet");
            healthBar.LoseLife();

            // Instantiate explosion effect
            // Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            if (healthBar.currentLives <= 0)
            {
                // Handle player death here
                gameObject.SetActive(false); // Or Destroy(gameObject);
                SceneManager.LoadScene("GameOverScene");
            }
            else
            {
                // Handle player hit but not dead, such as respawning
                RespawnPlayer();
            }
        }
    }


    // private void RespawnPlayer()
    // {
    //     // Your respawn logic here (e.g. reset position)
    //     transform.position = Vector3.zero;
    //     StartCoroutine(InvincibilityRoutine());
    // }

    private void RespawnPlayer()
    {
        // Calculate the left boundary position based on the camera's view
        var leftBoundary = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        var verticalCenter = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2f, 0)).y;

        // Get the player's sprite width to prevent spawning half off-screen
        var playerSpriteWidth = objectSize.x;
        if (spriteRenderers.Count > 0)
        {
            playerSpriteWidth = spriteRenderers[0].bounds.size.x / 2;
        }

        // Set the player's position to the left boundary (plus half sprite width) and vertically centered
        var respawnPosition = new Vector3(leftBoundary + playerSpriteWidth, verticalCenter, 0);
        rigidbody.position = respawnPosition; // Using Rigidbody2D's position for physics consistency

        // Begin the invincibility routine
        StartCoroutine(InvincibilityRoutine());
    }


    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        // spaceshipSpriteRenderer.enabled = false; // Start with the spaceship invisible

        // How often the sprite should flicker during invincibility
        var flickerInterval = 0.1f; //TODO: move to SeralizeField property if value must be changed later
        var elapsed = 0f;

        while (elapsed < invincibilityTime)
        {
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled; // Toggle visibility
            }

            yield return new WaitForSeconds(flickerInterval);
            elapsed += flickerInterval;
        }

        // Ensure all renderers are visible after invincibility ends
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.enabled = true;
        }

        isInvincible = false;
        // Disable the shield visual
        // shieldGameObject.SetActive(false);
    }
}