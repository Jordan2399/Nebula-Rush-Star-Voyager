using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject menuCanvas;
    public GameObject lunchCanvas;
    public Animator launchAnimationObject;

    private void Awake()
    {
        // Singleton pattern to ensure only one GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object between scene reloads
        }
        else
        {
            Destroy(gameObject); // Ensure there are no duplicates
        }
    }

    public void StartLevelTransition(string levelName)
    {
        // Disable the MenuCanvas
        menuCanvas.SetActive(false);

        // Ensure the LunchCanvas is active
        lunchCanvas.SetActive(true);

        // Start the transition coroutine
        StartCoroutine(LoadLevelAfterAnimation(levelName));
    }

    private IEnumerator LoadLevelAfterAnimation(string levelName)
    {
        Debug.Log("loading animation");
        // Trigger the start of the lunch animation
        launchAnimationObject.SetTrigger("StartLunchAnimation");

        // Wait for the Animator to start playing
        yield return null;

        // Now wait until the current state finishes playing
        while (launchAnimationObject.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        Debug.Log("waiting animation finished");
        // Optionally add a slight delay here if needed to allow for transition effects
        // yield return new WaitForSeconds(0.5f);
        Debug.Log("waiting o.5 finished");

        // Load the next level
        SceneManager.LoadScene(levelName);
    }
}