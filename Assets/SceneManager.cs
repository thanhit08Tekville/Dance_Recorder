using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    [SerializeField] TMP_Text countdownText;
    public GameObject startButton;

    private float countdownTime; // Time remaining for the countdown
    private bool isCounting; // Flag to track if countdown is active
    private bool isFadingOut; // Flag to track if text is fading out
    private float fadeOutTimer;
    private float countdownDuration = 3f;
    private float fadeOutDuration = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    public void StartCountdown()
    {
        startButton.SetActive(false);
        ResetTimer();
        isCounting = true;
    }

    private void PerformAction()
    {
        isCounting = false;
        // Perform the desired action here after the countdown finishes
        countdownText.text = "Start!";
        FadeOutText();
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
}
