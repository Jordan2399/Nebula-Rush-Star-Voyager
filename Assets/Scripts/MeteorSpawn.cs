using UnityEngine;

public class MeteorSpawn : MonoBehaviour
{
	public GameObject meteorPrefab; // Assign this in the Inspector
	public float spawnRate = 2f; // The rate at which enemies will spawn (every 2 seconds by default)

	private Camera mainCamera;
	private float nextSpawnTime;

	// Start is called before the first frame update
	void Start()
    {
		mainCamera = Camera.main; // Cache the main camera
		nextSpawnTime = Time.time + spawnRate; // Initialize the next spawn time
	}

    // Update is called once per frame
    void Update()
    {
		// Check if it's time to spawn a new enemy
		if (Time.time >= nextSpawnTime)
		{
			SpawnMeteor();
			nextSpawnTime = Time.time + spawnRate; // Set the time for the next spawn
		}

	}

	private void SpawnMeteor()
	{
		// Determine the vertical bounds of the camera view
		var screenHalfHeight = mainCamera.orthographicSize;

		// Determine the horizontal bounds of the camera view
		var screenHalfWidth = screenHalfHeight * mainCamera.aspect;

		// Get the enemy renderer component
		var meteorRenderer = meteorPrefab.GetComponent<Renderer>();

		if (meteorRenderer is null)
		{
			Debug.LogError("Meteor prefab must have a Renderer component for accurate width calculation.");
			return;
		}

		// Calculate the maximum spawn area considering the meteor size
		var bounds = meteorRenderer.bounds;
		var maxSpawnX = screenHalfWidth - bounds.extents.x;
		var maxSpawnY = screenHalfHeight - bounds.extents.y;

		// Set spawn position on the extreme right y-axis border
		var spawnX = maxSpawnX;
		var spawnY = Random.Range(-maxSpawnY, maxSpawnY);

		// Calculate spawn position with an offset from the center of the screen
		var spawnPosition = new Vector3(mainCamera.transform.position.x + spawnX, spawnY, 0);

		var meteorRotation = Quaternion.Euler(0, 0, -90); // Adjust the Euler angles as needed for your prefab

		// Instantiate meteor
		GameObject meteor = Instantiate(meteorPrefab, spawnPosition, meteorRotation);

		// Set velocity for the meteor to move towards the left
		var meteorRigidbody = meteor.GetComponent<Rigidbody2D>(); // Assuming you have a Rigidbody2D on the meteorPrefab
		if (meteorRigidbody != null)
		{
			// Adjust the velocity as needed
			var meteorVelocity = new Vector2(-5f, 0f); // Example velocity: move left at a speed of 5 units per second
			meteorRigidbody.velocity = meteorVelocity;
		}
		else
		{
			Debug.LogError("Meteor prefab must have a Rigidbody2D component for velocity control.");
		}
	}

}
