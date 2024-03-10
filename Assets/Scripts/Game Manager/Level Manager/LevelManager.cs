using System;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public GameObject bossPrefab;
	public GameObject bossHealth;// Assign this in the Inspector
	public float levelDistanceThreshold; // Distance before the boss appears
    private bool bossSpawned = false;
    [SerializeField] private Animator screenFlickerAnimator;
    // private int GameLevel = 0;
    private bool isFlickering = false;
    
    private Camera mainCamera;
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mainCamera = Camera.main; // Cache the main camera
    }

    void Update()
    {
        if (!bossSpawned && PlayerHasReachedDistanceThreshold())
        {
            // SpawnBoss();
            StartCoroutine(BossEntrySequence());

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
    
    
    
    
    
    
    // private IEnumerator BossEntrySequence()
    // {
    //     // Pause gameplay-related updates, stop the player and other activities
    //     
    //     // Start the screen flickering effect
    //     // Assume you have an Animator for a full-screen UI image that has a flicker effect.
    //     Animator screenAnimator = screenFlickerAnimator /* Get your Animator reference */;
    //     screenAnimator.SetTrigger("StartFlicker");
    //     Time.timeScale = 0;
    //     
    //     // Wait a bit while the screen is flickering
    //     yield return new WaitForSecondsRealtime(2f); // Adjust time as needed for flicker effect duration
    //
    //     // Instantiate the boss off-screen here and animate its entry
    //     var enemyBossRotation = Quaternion.Euler(0, 0, 90); // Adjust the Euler angles as needed for your prefab
    //
    //     GameObject boss = Instantiate(bossPrefab, GetOffScreenPosition(), enemyBossRotation);
    //     StartCoroutine(AnimateBossEntry(boss));
    //     bossHealth.SetActive(true);
    //
    //     // Wait for the boss entry animation to complete
    //     yield return new WaitForSecondsRealtime(3f); // Adjust time as needed for boss entry duration
    //
    //     // Stop the screen flickering effect
    //     screenAnimator.SetTrigger("StopFlicker");
    //
    //     // Resume gameplay
    //     Time.timeScale = 1;
    //
    //     // Continue with any other setup needed after boss has entered
    // }
    
    
    private IEnumerator BossEntrySequence()
    {
        // Start the screen flickering effect
        Animator screenAnimator = screenFlickerAnimator/* Get your Animator reference */;
        // screenAnimator.SetTrigger("StartFlicker");
        // Instantiate the boss off-screen here and animate its entry
        var enemyBossRotation = Quaternion.Euler(0, 0, 90); // Adjust the Euler angles as needed for your prefab
        // Slowly move the boss onto the screen
        GameObject boss = Instantiate(bossPrefab, GetOffScreenPosition(), enemyBossRotation);
        Coroutine bossEntry = StartCoroutine(AnimateBossEntry(boss));

        // Pause the game, but keep animations and coroutines that use real-time updating
        Time.timeScale = 0;
    
        // Wait for the boss to finish entering
        yield return bossEntry; // This will still complete because we're using real-time in the coroutine
    
        // Stop the screen flickering effect
        // screenAnimator.SetTrigger("StopFlicker");
    
        // Resume the game
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
        
        
        var enemyBossRenderer = bossPrefab.GetComponent<Renderer>();

        
        
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
        
        
        
        
        Vector3 startPosition = GetOffScreenPosition();
        Vector3 endPosition = spawnPosition; // Replace with the actual boss position on screen
        float duration = 3f; // Duration of the animation
        
        
        // Start flickering effect
        // Start flickering effect
        screenFlickerAnimator.SetBool("IsFlickering", true);
        
        
        
        // Animate the boss entry
        float startTime = Time.unscaledTime; // Using unscaled time if the game is paused
        while (Time.unscaledTime - startTime < duration)
        {
            float progress = (Time.unscaledTime - startTime) / duration;
            boss.transform.position = Vector3.Lerp(startPosition, endPosition, progress);

            // You could also add some scaling or other effects here if you want

            yield return null;
        }
        
        boss.transform.position = endPosition;
        // Wait a little bit more if you want to ensure the flickering continues for a while after the boss has stopped moving
        yield return new WaitForSecondsRealtime(0.5f); // Adjust as necessary
        
        // Stop flickering effect
        screenFlickerAnimator.SetBool("IsFlickering", false);

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
		bossHealth.SetActive(true);


	}

    public void BossDefeated()
    {
        Debug.Log("Boss is killed");
		bossHealth.SetActive(false);
		// Start next level or show victory screen
	}
}