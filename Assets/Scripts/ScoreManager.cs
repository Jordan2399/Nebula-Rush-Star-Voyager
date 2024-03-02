using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int Score { get; private set; }

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

    private void Update()
    {
        Debug.Log("my current score is" + Score);
    }

    public void AddScore(int points)
    {
        Score += points;
        // Update the UI here if needed
    }

    public void AddDistanceScore(float distance)
    {
        // Assuming 1 point per unit of distance for example
        Score += Mathf.FloorToInt(distance);
        // Update the UI here if needed
    }

    public void ResetScore()
    {
        Score = 0;
        // Update the UI here if needed
    }
}