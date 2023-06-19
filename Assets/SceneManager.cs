using System;
using System.Collections;
using System.Collections.Generic;
using LightShaft.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    [SerializeField] TMP_Text countdownText;
    [SerializeField]
    private YoutubePlayer player;

    public GameObject startButton;
    //public Image imagePreview; // Reference to the UI image component
    //public RawImage cameraPreview;
    public string youtubeURL;
    public int youtubeTime;
    public string musicClipName; // Name of the music clip to play
    public string videoSavePath; // File path to save the recorded video


    private float countdownTime; // Time remaining for the countdown
    private bool isCounting; // Flag to track if countdown is active
    private bool isFadingOut; // Flag to track if text is fading out
    private float fadeOutTimer;
    private float countdownDuration = 3f;
    private float fadeOutDuration = 1f;

    private bool isRecording; // Flag to track if video recording is active

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        ResetScene();
    }

    public void ResetScene()
    {
        player.StopAllCoroutines();
        player.Stop();
        countdownTime = countdownDuration;
        countdownText.gameObject.SetActive(false);
        startButton.SetActive(true);
        isCounting = false;
        isFadingOut = false;
        isRecording = false;       
    }

    private IEnumerator WaitSystemDoSomething()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCounting)
        {
            countdownTime -= Time.deltaTime;

            if (countdownTime <= 0f)
            {
                // Countdown has finished, perform action here
                PerformAction();
            }
            else
            {
                // Update UI text with countdown value
                countdownText.text = Mathf.CeilToInt(countdownTime).ToString();
            }
        }

        if (isFadingOut)
        {
            fadeOutTimer += Time.deltaTime;

            if (fadeOutTimer >= fadeOutDuration)
            {
                // Fade out animation complete
                countdownText.text = ""; // Hide the text
                isFadingOut = false;
                countdownText.gameObject.SetActive(false);
            }
            else
            {
                // Update text opacity during fade out
                float alpha = 1f - (fadeOutTimer / fadeOutDuration);
                countdownText.color = new Color(countdownText.color.r, countdownText.color.g, countdownText.color.b, alpha);
            }
        }

        if (isRecording)
        {
            // Continue recording video
            // Add your video recording logic here
        }
    }

    public void StartCountdown()
    {
        startButton.SetActive(false);
        ResetTimer();
        isCounting = true;
        //DisplayImageFromCamera();
    }

    private void PerformAction()
    {
        isCounting = false;
        // Perform the desired action here after the countdown finishes
        countdownText.text = "Start!";
        FadeOutText();
        //DisplayImageFromCamera();
        PlayMusic();
        StartVideoRecording();
    }

    private void FadeOutText()
    {
        isFadingOut = true;
        fadeOutTimer = 0f;
    }

    private void ResetTimer()
    {
        countdownText.gameObject.SetActive(true);
        countdownTime = countdownDuration;
        countdownText.text = "";
        countdownText.color = Color.white; // Reset text color to full opacity

    }

    private void PlayMusic()
    {
        PlayFromUrlFieldStartingAt(youtubeTime);
    }

    private void StartVideoRecording()
    {
        // Start recording the video
        // Add your video recording logic here
        isRecording = true;
    }


    public void PlayFromUrlFieldStartingAt(int timePlaying)
    {
        Reset();
        //Simple call to start playing starting from second. in this case 10 seconds
        player.Play(youtubeURL, timePlaying);
    }

    private void Reset()
    {
        player.loadYoutubeUrlsOnly = false;
    }
}
