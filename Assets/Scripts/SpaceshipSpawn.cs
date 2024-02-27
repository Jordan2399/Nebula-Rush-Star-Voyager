using System.Collections;
using UnityEngine;

public class SpaceshipSpawn : MonoBehaviour
{
	public GameObject enemyPrefab; // Assign this in the Inspector
	public float spawnRate = 2f; // The rate at which enemies will spawn (every 2 seconds by default)

	private Camera mainCamera;
	private float nextSpawnTime;

	void Start()
	{
		mainCamera = Camera.main; // Cache the main camera
		nextSpawnTime = Time.time + spawnRate; // Initialize the next spawn time
	}

	void Update()
	{
		// Check if it's time to spawn a new enemy
		if (Time.time >= nextSpawnTime)
		{
			SpawnEnemy();
			nextSpawnTime = Time.time + spawnRate; // Set the time for the next spawn
		}
	}

	void SpawnEnemy()
	{
		// Determine the vertical bounds of the camera view
		float screenHalfHeight = mainCamera.orthographicSize;
		float verticalExtent = screenHalfHeight * 2f;

		// Determine the horizontal bounds of the camera view
		float screenHalfWidth = screenHalfHeight * mainCamera.aspect;
		float horizontalExtent = screenHalfWidth * 2f;

		// Get the enemy renderer component
		Renderer enemyRenderer = enemyPrefab.GetComponent<Renderer>();

		if (enemyRenderer == null)
		{
			UnityEngine.Debug.LogError("Enemy prefab must have a Renderer component for accurate width calculation.");
			return;
		}

		// Calculate the maximum spawn area considering the enemy size
		float maxSpawnX = screenHalfWidth - enemyRenderer.bounds.extents.x;
		var maxSpawnY = screenHalfHeight - enemyRenderer.bounds.extents.y;

		// Set spawn position on the extreme right y-axis border
		float spawnX = maxSpawnX;
		var spawnY = UnityEngine.Random.Range(-maxSpawnY, maxSpawnY);

		// Calculate spawn position with an offset from the center of the screen
		Vector3 spawnPosition = new Vector3(mainCamera.transform.position.x + spawnX, spawnY, 0);

		Quaternion enemyRotation = Quaternion.Euler(0, 0, -90); // Adjust the Euler angles as needed for your prefab
		Instantiate(enemyPrefab, spawnPosition, enemyRotation);
	}
	
	

}
