using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Speed of the bullet

    // Update is called once per frame
    private void Update()
    {
        // Move the bullet forward
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
        // Destroy the bullet if it goes out of the screen
        if (!GetComponent<Renderer>().isVisible) //TODO: TryGetComponent!!!
        {
            Destroy(gameObject);
        }
    }

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the bullet collided with an object tagged as "Enemy"
        if (collision.CompareTag("Enemy"))
        {
            // Destroy the bullet upon collision with an enemy
            Destroy(gameObject);
        }
    }
}