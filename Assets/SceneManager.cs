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
    public GameObject pauseButton;
    public GameObject stopButton;
    //public Image imagePreview; // Reference to the UI image component
    //public RawImage cameraPreview;
    public List<string> youtubeURL = new List<string>();
    public List<int> youtubeTime = new List<int>();
    public string musicClipName; // Name of the music clip to play
    public string videoSavePath; // File path to save the recorded video


    private float countdownTime; // Time remaining for the countdown
    private bool isCounting; // Flag to track if countdown is active
    private bool isFadingOut; // Flag to track if text is fading out
    private float fadeOutTimer;
    private float countdownDuration = 3f;
    private float fadeOutDuration = 1f;

    private bool isRecording; // Flag to track if video recording is active

    /// <summary>
    /// Song youtube URL index
    /// </summary>
    public static int songIndex = 0;


    public RawImage rawImage;
    private BoxCollider2D rawImageCollider;

    private void OnRawImageClick()
    {
        // Handle the raw image click event here
        Debug.Log("Raw image clicked!");
    }

    // Start is called before the first frame update
    void Start()
    {
        // Add a click event listener to the raw image
        rawImageCollider = rawImage.GetComponent<BoxCollider2D>();
    }

    private void LateUpdate()
    {
        var rectTransform = rawImage.GetComponent<RectTransform>();
        if (rectTransform.rect.width == 0 || rectTransform.rect.height == 0)
        {
            return;
        }

        var (width, height) = GetBoundingBoxSize(rectTransform);
        rawImageCollider.size = new Vector2(width, height);
    }

    private (float, float) GetBoundingBoxSize(RectTransform rectTransform)
    {
        var rect = rectTransform.rect;
        var center = rect.center;
        var topLeftRel = new Vector2(rect.xMin - center.x, rect.yMin - center.y);
        var topRightRel = new Vector2(rect.xMax - center.x, rect.yMin - center.y);
        var rotatedTopLeftRel = rectTransform.rotation * topLeftRel;
        var rotatedTopRightRel = rectTransform.rotation * topRightRel;
        var wMax = Mathf.Max(Mathf.Abs(rotatedTopLeftRel.x), Mathf.Abs(rotatedTopRightRel.x));
        var hMax = Mathf.Max(Mathf.Abs(rotatedTopLeftRel.y), Mathf.Abs(rotatedTopRightRel.y));
        return (2 * wMax, 2 * hMax);
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        ResetScene();
    }

    public void switchSong(int iSongIndex)
    {
        songIndex = iSongIndex;
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

        if (isRecording && !isFadingOut)
        {
            // Continue recording video
            // Add your video recording logic here

            if (Input.GetMouseButtonDown(0))
            {
                //Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (rawImageCollider.OverlapPoint(Input.mousePosition))
                {
                    // Handle the raw image click event here
                    Debug.Log("Raw image clicked!");
                    PauseMusic();
                }
            }
        }

        
    }

    public void StartCountdown()
    {
        startButton.SetActive(false);
        ResetTimer();
        isCounting = true;
        //DisplayImageFromCamera();
    }

    public void ResumMusic()
    {
        pauseButton.SetActive(false);
        stopButton.SetActive(false);
        player.PlayPause();
    }

    public void PauseMusic()
    {
        if (isCounting)
        {
            ResetScene();
        } else
        {
            pauseButton.SetActive(true);
            stopButton.SetActive(true);
            player.Pause();
        }

    }

    public void StopMusic()
    {
        pauseButton.SetActive(false);
        stopButton.SetActive(false);
        StopVideoRecording();
        ResetScene();
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
        PlayFromUrlFieldStartingAt(youtubeTime[songIndex]);
    }

    private void StartVideoRecording()
    {
        // Start recording the video
        // Add your video recording logic here
        isRecording = true;
    }

    private void PauseVideoRecording()
    {
        // Start recording the video
        // Add your video recording logic here
        isRecording = false;
    }

    private void StopVideoRecording()
    {
        // Start recording the video
        // Add your video recording logic here
        isRecording = false;
    }


    public void PlayFromUrlFieldStartingAt(int timePlaying)
    {
        Reset();
        //Simple call to start playing starting from second. in this case 10 seconds
        if (timePlaying == 0)
        {
            player.Play(youtubeURL[songIndex]);
        }
        else
        {
            player.Play(youtubeURL[songIndex], timePlaying);
        }
        
    }

    private void Reset()
    {
        player.loadYoutubeUrlsOnly = false;
    }
}
