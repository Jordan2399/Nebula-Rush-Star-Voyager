using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager Instance { get; private set; }
	public int Score { get; private set; }


	private int score = 0;

	public TextMeshProUGUI scoreText; // Assign your TextMeshPro UI element in the Inspector

	private void Start()
	{
		// Find and cache the TextMeshPro UI element
		if (scoreText == null)
		{
			UnityEngine.Debug.LogError("TextMeshPro UI element not assigned!");
		}

		UpdateScoreText();
	}


	private void Awake()
	{
		// Singleton pattern to ensure only one ScoreManager exists
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

	// Add points to the player's score
	public void AddScore(int points)
	{
		score += points;
		UpdateScoreText();
	}

	// Update the TextMeshPro UI with the current score
	private void UpdateScoreText()
	{
		if (scoreText != null)
		{
			scoreText.text = $"Score:{score}";
		}
	}

	public void ResetScore()
	{
		Score = 0;
		UpdateScoreText();
	}



	public void AddDistanceScore(float distance)
	{
		// Assuming 1 point per unit of distance for example
		Score += Mathf.FloorToInt(distance);
		UpdateScoreText();
	}


}
