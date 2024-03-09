using System;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject bossPrefab; // Assign this in the Inspector
    public float levelDistanceThreshold; // Distance before the boss appears
    private bool bossSpawned = false;
    [SerializeField] private Animator bossEntryAnimator;


    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main; // Cache the main camera
    }

    void Update()
    {
        if (!bossSpawned && PlayerHasReachedDistanceThreshold())
        {
            // StartCoroutine(BossEntrySequence());

            // SpawnBoss();
            bossSpawned = true;
        }
    }

    private bool PlayerHasReachedDistanceThreshold()
    {
        // Access the total distance from the ScoreManager
        float totalDistance =
            ScoreManager.Instance.GetTotalDistance(); // This method needs to be implemented in your ScoreManager

        // Check if the total distance is greater than or equal to the threshold
        Debug.Log("Distance travelled: " + totalDistance);
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


    private IEnumerator BossEntrySequence()
    {
        // Pause gameplay-related updates, stop the player and other activities
        Time.timeScale = 0;

        // Start the screen flickering effect
        // Assume you have an Animator for a full-screen UI image that has a flicker effect.
        Animator screenAnimator = bossEntryAnimator /* Get your Animator reference */;
        screenAnimator.SetTrigger("StartFlicker");

        // Wait a bit while the screen is flickering
        yield return new WaitForSecondsRealtime(2f); // Adjust time as needed for flicker effect duration

        // Instantiate the boss off-screen here and animate its entry
        // GameObject boss = Instantiate(bossPrefab, GetOffScreenPosition(), Quaternion.identity);
        // StartCoroutine(AnimateBossEntry(boss));

        // Wait for the boss entry animation to complete
        yield return new WaitForSecondsRealtime(3f); // Adjust time as needed for boss entry duration

        // Stop the screen flickering effect
        screenAnimator.SetTrigger("StopFlicker");

        // Resume gameplay
        Time.timeScale = 1;

        // Continue with any other setup needed after boss has entered
    }

    // Here's how you can get an off-screen position to the right of the camera
    private Vector3 GetOffScreenPosition()
    {
        float screenHalfHeight = Camera.main.orthographicSize;
        float screenHalfWidth = screenHalfHeight * Camera.main.aspect;
        return new Vector3(Camera.main.transform.position.x + screenHalfWidth, 0, 0);
    }

    // Animate boss entering from the side of the screen to its starting position
    private IEnumerator AnimateBossEntry(GameObject boss)
    {
        Vector3 startPosition = GetOffScreenPosition();
        Vector3 endPosition = new Vector3(0, 0, 0); // Replace with the actual boss position on screen
        float duration = 3f; // Duration of the animation

        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            float normalizedTime = t / duration;
            boss.transform.position = Vector3.Lerp(startPosition, endPosition, normalizedTime);
            yield return null;
        }

        boss.transform.position = endPosition;
    }
}