using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public TextMeshProUGUI scoreText; // Assign your TextMeshPro UI element in the Inspector

    private int score = 0;
    private float distanceAccumulator = 0f; // This will accumulate the fractional distance until it exceeds 1
    private bool isBossActive = false; // Added to control score and distance accumulation
    
    private float totalDistance = 0f;

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
        score = 0; // Initialize score to zero
        UpdateScoreText(); // Update UI
    }

    // Add points to the player's score based on distance traveled
    public void AddDistanceScore(float distance, float scorePerUnit)
    {
        if (isBossActive) return; // Do not accumulate distance or score if a boss is active

        // Convert distance to score using the rate factor
        float scoreToAdd = distance * scorePerUnit;

        // Accumulate the score to add
        distanceAccumulator += scoreToAdd;

        if (distanceAccumulator >= 1f) // Adjust this threshold as needed
        {
            totalDistance += distance; // Accumulate total distance

            // Increment score by the integer part of the accumulated score
            score += Mathf.FloorToInt(distanceAccumulator);

            // Keep the fractional part of the score in the accumulator
            distanceAccumulator -= Mathf.Floor(distanceAccumulator);

            // Update the score display
            UpdateScoreText();
        }
    }

    public void AddPoint(int point)
    {
        score += point;
        UpdateScoreText();
    }

    // Update the TextMeshPro UI with the current score
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    public void ResetScore()
    {
        score = 0;
        distanceAccumulator = 0f;
        UpdateScoreText();
    }

    // Call this method when a boss is spawned
    public void BossActive(bool isActive)
    {
        isBossActive = isActive;
    }
    
    // Method to get the total distance traveled
    public float GetTotalDistance()
    {
        return totalDistance;
    }
}
