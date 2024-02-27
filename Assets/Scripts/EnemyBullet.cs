using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 2f; // Speed of the bullet

    // Update is called once per frame
    private void Update()
    {
        // Move the bullet forward
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
        // Destroy the bullet if it goes out of the screen

        if (!gameObject.TryGetComponent<Renderer>(out Renderer renderer) || !renderer.isVisible)
 
        {
            Destroy(gameObject);
        }
    }

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the bullet collided with an object tagged as "Player"
        if (collision.CompareTag("Player"))
        {
            // Destroy the bullet upon collision with an enemy
            Destroy(gameObject);
        }
    }
}