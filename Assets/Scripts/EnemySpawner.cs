using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject Enemy; // Assign this in the Inspector
	public float baseSpawnRate = 4f; // The base spawn rate (every 4 seconds by default)
	public float minSpawnRate = 0.5f; // The minimum spawn rate
	public float spawnDistance = 10f; // Distance from the center of the screen to the spawn location
	public float progressMultiplier = 0.9f; // Multiplier to increase spawn rate based on progress

	private Camera mainCamera;
	private bool isEnemySpawned = false;
	private float nextSpawnTime;
	private float userProgress;

	private void Start()
	{
		mainCamera = Camera.main; // Cache the main camera
		nextSpawnTime = Time.time + baseSpawnRate; // Initialize the next spawn time
	}

	private void Update()
	{
		// Simulate user progress (replace this with your actual progression logic)
		userProgress += Time.deltaTime * 0.1f; // Adjust the multiplier as needed

		// Update spawn rate based on user progress
		var currentSpawnRate = Mathf.Max(minSpawnRate, baseSpawnRate * Mathf.Pow(progressMultiplier, userProgress));

		// Check if it's time to spawn a new enemy
		if (!isEnemySpawned && Time.time >= nextSpawnTime) //move this to function PrepareSpawnEnemy
		{
			SpawnEnemy();
			isEnemySpawned = true;
			nextSpawnTime = Time.time + currentSpawnRate; // Set the time for the next spawn using the adjusted spawn rate
		}
	}

	private void SpawnEnemy()
	{
		// Determine the vertical bounds of the camera view
		var screenHalfHeight = mainCamera.orthographicSize;
		var verticalExtent = screenHalfHeight * 2f;

		// Choose a random position within the camera view
		var spawnX = UnityEngine.Random.Range(-screenHalfHeight, screenHalfHeight);
		var spawnY = UnityEngine.Random.Range(-screenHalfHeight, screenHalfHeight);

		// Calculate spawn position with an offset from the center of the screen
		var spawnPosition = new Vector3(mainCamera.transform.position.x + spawnDistance + spawnX, spawnY, 0);

		var enemyRotation = Quaternion.Euler(0, 0, -90); // Adjust the Euler angles as needed for your prefab
		Instantiate(Enemy, spawnPosition, enemyRotation);
	}

	// Call this method when the enemy is destroyed or defeated
	public void EnemyDestroyed()
	{
		isEnemySpawned = false;
	}
}
