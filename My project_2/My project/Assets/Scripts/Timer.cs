using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] bool countdownMode = false;
    [SerializeField] float timeToDeath = 601.0f; // (10 minutes, 5mins = 300.0f) 
    [SerializeField] string levelToLoad;
    private float elapsedTime;

    private int minsLeft;
    private int secsLeft;
    private int elapsedTime_mins;
    private int elapsedTime_secs;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (countdownMode) 
        {
            timeToDeath = getCountDownTime();
            minsLeft = Mathf.FloorToInt(timeToDeath / 60);
            secsLeft = Mathf.FloorToInt(timeToDeath % 60);
            doWeLoadGameOver(); // method includes a check
        } 
        else    // For posterity. 
        {
            elapsedTime += Time.deltaTime;
            elapsedTime_mins = Mathf.FloorToInt(elapsedTime / 60);
            elapsedTime_secs = Mathf.FloorToInt(elapsedTime % 60);
        }
    }

    // Get the timer as "xx:yy"
    public string getTimerText() {
        if (countdownMode) {
            return string.Format("{0:00}:{1:00}", minsLeft, secsLeft);
        } 
        else {
            return string.Format("{0:00}:{1:00}", elapsedTime_mins, elapsedTime_secs);
        }
    }

    // When timer ends, load a game over. If you don't want to load, you can replace the
    // SceneManager.LoadScene(...) with a boolean that tells objects that it's game over time
    private void doWeLoadGameOver() {
        if (timeToDeath <= 0.2f) {

            // No provided level to load
            if (levelToLoad == null) {
                Debug.Log("No level to load!");
                return;
            }

            // Load scene on game over
            SceneManager.LoadScene(levelToLoad);
        }
    }

    // Calculates countdown timer. Mainly to avoid negative numbers on the timer
    private float getCountDownTime() {
        if (timeToDeath <= 0.2f) {
            // I used <= 0.2f just in case. Y'know, Unity shenanigans.
            return 0.0f;
        }
        else {
            return timeToDeath - Time.deltaTime;
        }
    }
}
