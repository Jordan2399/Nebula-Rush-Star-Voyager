using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject bossPrefab; // Assign this in the Inspector
    public float levelDistanceThreshold; // Distance before the boss appears
    private bool bossSpawned = false;
    
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main; // Cache the main camera
    }

    void Update()
    {
        if (!bossSpawned && PlayerHasReachedDistanceThreshold())
        {
            SpawnBoss();
            bossSpawned = true;
        }
    }

    private bool PlayerHasReachedDistanceThreshold()
    {
        // Access the total distance from the ScoreManager
        float totalDistance = ScoreManager.Instance.GetTotalDistance(); // This method needs to be implemented in your ScoreManager

        // Check if the total distance is greater than or equal to the threshold
        Debug.Log("Distance travelled: "+totalDistance);
        return totalDistance >= levelDistanceThreshold;
    }

    private void SpawnBoss()
    {
        
        var enemyBossRenderer = bossPrefab.GetComponent<Renderer>();
        
        // Stop the background scroll
        BGScroll bgScroll = FindObjectOfType<BGScroll>();
        if (bgScroll != null)
        {
            bgScroll.enabled = false;
        }
        
        // Instantiate the boss
        var enemyBossRotation = Quaternion.Euler(0, 0, 90); // Adjust the Euler angles as needed for your prefab

        var screenHalfHeight = mainCamera.orthographicSize;
        var screenHalfWidth = screenHalfHeight * mainCamera.aspect;
        // Calculate the maximum spawn area considering the enemy size
        var bounds = enemyBossRenderer.bounds;
        var maxSpawnX = screenHalfWidth - bounds.extents.x;
        var maxSpawnY = screenHalfHeight - bounds.extents.y;
        
        // Set spawn position on the extreme right y-axis border
        var spawnX = maxSpawnX; 

        
        // Calculate spawn position with an offset from the center of the screen
        var spawnPosition = new Vector3(mainCamera.transform.position.x + spawnX, 0, 0);
        Instantiate(bossPrefab, spawnPosition, enemyBossRotation);
    }

    public void BossDefeated()
    {
        // Start next level or show victory screen
    }
}