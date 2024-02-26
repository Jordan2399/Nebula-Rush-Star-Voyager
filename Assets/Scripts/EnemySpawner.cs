using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject enemyPrefab; // Assign this in the Inspector
	public float spawnRate = 2f; // The rate at which enemies will spawn (every 2 seconds by default)
	public float enemySpeed = 3f; // Speed of the enemy

	private Camera mainCamera;
	private float nextSpawnTime;
	private Transform playerTransform;


	void Start()
	{
		mainCamera = Camera.main; // Cache the main camera
		playerTransform = FindObjectOfType<SpaceshipControls>().transform; // Assuming you have a PlayerControls script

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
		var screenHalfHeight = mainCamera.orthographicSize;
		var verticalExtent = screenHalfHeight * 2f;

		// Determine the horizontal bounds of the camera view
		var screenHalfWidth = screenHalfHeight * mainCamera.aspect;
		var horizontalExtent = screenHalfWidth * 2f;

		// Get the enemy renderer component
		Renderer enemyRenderer = enemyPrefab.GetComponent<Renderer>();

		if (enemyRenderer == null)
		{
			UnityEngine.Debug.LogError("Enemy prefab must have a Renderer component for accurate width calculation.");
			return;
		}

		// Calculate the maximum spawn area considering the enemy size
		var maxSpawnX = screenHalfWidth - enemyRenderer.bounds.extents.x;
		var maxSpawnY = screenHalfHeight - enemyRenderer.bounds.extents.y;

		// Choose a random position within the spawn area
		var spawnX = UnityEngine.Random.Range(0, maxSpawnX);
		var spawnY = UnityEngine.Random.Range(-maxSpawnY, maxSpawnY);

		// Calculate spawn position with an offset from the center of the screen
		Vector3 spawnPosition = new Vector3(mainCamera.transform.position.x + spawnX, spawnY, 0);

		Quaternion enemyRotation = Quaternion.Euler(0, 0, -90); // Adjust the Euler angles as needed for your prefab
		GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, enemyRotation);

		// Get the EnemyMovement component from the instantiated enemy
		EnemyMovement enemyMovement = newEnemy.GetComponent<EnemyMovement>();
		if (enemyMovement != null)
		{
			// Set the target player for the enemy to follow
			enemyMovement.SetTarget(playerTransform);
		}
		else
		{
			UnityEngine.Debug.LogError("Enemy prefab must have an EnemyMovement component for movement.");
		}
	}





}