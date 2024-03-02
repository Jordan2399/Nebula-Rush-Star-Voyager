using UnityEngine;

public class EnemySpaceshipBullet : MonoBehaviour
{
    public GameObject bulletPrefab; // Assign the enemy bullet prefab in the Inspector

    // public GameObject player; // Assign the player GameObject in the Inspector
    public float firingRate = 1f; // How often the enemy fires
    public float bulletSpeed = 5f; // Speed of the bullet
    public float bulletSpawnOffset = 1f; // Distance in front of the enemy where bullets spawn

    private float nextFireTime;
    private GameObject player;

    private void Start()
    {
        // Find the player in the scene and assign it
        player = GameObject.FindGameObjectWithTag("Player");
        // Initialize the nextFireTime
        nextFireTime = Time.time + firingRate;
    }

    private void Update()
    {
        // Check if it's time to fire
        if (Time.time >= nextFireTime && player && player.activeInHierarchy)
        {
            FireAtPlayer();
            nextFireTime = Time.time + firingRate; // Set the time for the next shot
        }
    }

    private void FireAtPlayer()
    {
        if (player && bulletPrefab && player.activeInHierarchy)
        {
            // Update the player position every time we fire
            var playerPosition = player.transform.position;

            // Calculate the direction from the enemy to the player
            var directionToPlayer = (playerPosition - transform.position).normalized;

            // Offset the spawn position in front of the enemy
            var spawnPosition = transform.position + directionToPlayer * bulletSpawnOffset;

            // Instantiate the bullet
            var bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            var rigidbody = bullet.GetComponent<Rigidbody2D>();

            if (rigidbody)
            {
                // Set the bullet velocity towards the player
                rigidbody.velocity = directionToPlayer * bulletSpeed;
                
                // Rotate the bullet to face the player
                // float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
                // float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg-90f;
                // rigidbody.rotation = angle;

                // Debug log to show where the bullet is being instantiated
                // Debug.Log($"Bullet fired towards player at position: {playerPosition}");
                
                
                
                // Rotate the bullet to face towards the direction it's moving
                var angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
                rigidbody.rotation = angle - 90f; // Adjust the angle if necessary
                
                
            }
            else
            {
                Debug.LogError("Rigidbody not found on the bullet prefab!");
            }
        }
        else
        {
            if (!player)
            {
                Debug.LogError("Player object not assigned or not found!");
            }

            if (!bulletPrefab)
            {
                Debug.LogError("Bullet prefab not assigned!");
            }
        }
    }
    
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object collided with has the tag "PlayerBullet"
        if (collision.CompareTag("PlayerBullet"))
        {
            // Optionally, you might want to add additional logic here
            // to handle what happens when the enemy is destroyed.
            // For example, you could play an explosion effect, increase the player's score, etc.

            // Destroy the enemy object this script is attached to
            Destroy(gameObject);

            // Also, you might want to destroy the bullet to prevent it from continuing through space
            Destroy(collision.gameObject);
        }
    }
}