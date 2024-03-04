using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
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

	// You can also create a method to retrieve the current score if needed
	public int GetScore()
	{
		return score;
	}
}
