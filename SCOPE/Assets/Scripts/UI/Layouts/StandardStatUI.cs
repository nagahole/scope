using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public interface IStatUI {
    void UpdateAccuracy(float accuracy);
    void UpdateScore(float newScore);
    void UpdateScoreText(string text);
    void UpdateTime(long milliseconds);
    void SetUntimed();
    void Show();
    void Hide();
    void Setup();
    void SetTimeDP(int dp);
    void SetTimerMode();
    void SetStopwatchMode();
}

public class StandardStatUI : MonoBehaviour, IStatUI {
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Animator animator;

    private string dpFormat = "";

    private bool stopwatchMode;

    //might refactor this into scenariomanager UpdateCountdown
    public void Setup() {
        Show();
        animator.SetTrigger("Setup");
    }

    public void Hide() {
        uiCanvas.enabled = false;
    }

    public void Show() {
        uiCanvas.enabled = true;
    }

    #region stats
    public void SetTimeDP(int dp) {
        dpFormat = new string('0', dp);
    }

    public void SetUntimed() {
        timeText.text = "-/-";
    }

    public void UpdateAccuracy(float accuracy) {
        accuracyText.text = (accuracy * 100).ToString("#0.0") + "%";
    }

    public void UpdateScore(float newScore) {
        scoreText.text = newScore.ToString();
    }

    public void UpdateScoreText(string text) {
        scoreText.text = text;
    }

    public void UpdateTime(long milliseconds) {
        milliseconds = (long) Mathf.Max(0, milliseconds);
        int minutes = (int)(milliseconds / 60000);
        float seconds = (milliseconds - minutes * 60000) / 1000f;
        if (stopwatchMode) {
            if(minutes > 0) {
                timeText.text = string.Join(":", minutes, seconds.ToString("00." + dpFormat));
            } else {
                timeText.text = seconds.ToString("0." + dpFormat);
            }
        } else {
            //TODO: DEFINITELY REWORK THIS 
            if (milliseconds <= 59000) {
                timeText.text = Mathf.Ceil(seconds).ToString("0." + dpFormat);
            } else if (milliseconds <= 60000) {
                timeText.text = "1:00";
            } else {
                timeText.text = string.Join(":", minutes, Mathf.Ceil(seconds).ToString("00."));
            }
        }
                
        
    }

    public void SetTimerMode() {
        stopwatchMode = false;
    }

    public void SetStopwatchMode() {
        stopwatchMode = true;
    }
    #endregion
}
