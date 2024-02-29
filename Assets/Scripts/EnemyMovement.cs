using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private float speed = 3f;

    // Set the target player for the enemy to follow
    public void SetTarget(Transform newTarget)
    {
        Debug.Log("from setTarget");

        target = newTarget;
    }

    private void Update()
    {
        // Check if a target is not set, if not set break
        if (target is null) return;
        // Calculate the direction towards the target player
        Vector2 direction = (target.position - transform.position).normalized;

        // Move towards the target player
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Ensure the enemy is facing the direction of movement
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object collided with has not the tag "PlayerBullet"
        if (!collision.CompareTag("PlayerBullet")) return;
        
        // Optionally, you might want to add additional logic here
        // to handle what happens when the enemy is destroyed.
        // For example, you could play an explosion effect, increase the player's score, etc.

        // Destroy the enemy object this script is attached to
        Destroy(gameObject);

        // Also, you might want to destroy the bullet to prevent it from continuing through space
        Destroy(collision.gameObject);
    }
}