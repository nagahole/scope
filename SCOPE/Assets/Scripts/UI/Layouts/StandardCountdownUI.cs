using UnityEngine;
using TMPro;
using MEC;
using System.Collections.Generic;
using UnityEngine.UI;

public interface ICountdownUI {
    void Show();
    void Hide();
    void Setup();
    void Exit();
    void CountdownTicked(long milliseconds);
    void SetStartingCountdownTime(int seconds);
}

public class StandardCountdownUI : MonoBehaviour, ICountdownUI 
{
    [SerializeField] private Animator animator;
    [SerializeField] private Canvas selfCanvas;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Transform countdownParent;
	[Space]
    [SerializeField] private TextMeshProUGUI shootToStartText;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float beginToFadeAt = 1f;
    [Space]
    [SerializeField] private RawImage flash;
    [SerializeField] private Color32 flashColor;
    [SerializeField] private float flashDuration;

    private bool countdownHasTicked = false;
    private bool hasExited = false;

    public void Setup() {
        countdownHasTicked = false;
        hasExited = false;
        flash.gameObject.SetActive(false);
        shootToStartText.gameObject.SetActive(true);
        animator.SetTrigger("Setup");
    }

    public void Show() {
        selfCanvas.enabled = true;
    }

	public void Hide() {
        selfCanvas.enabled = false;
    }

    public void Exit() {
        Debug.Log("Exit");
        hasExited = true;
        animator.SetTrigger("Exit");
    }

    public void SetStartingCountdownTime(int seconds) {
        countdownText.text = seconds.ToString();
    }

    public void CountdownTicked(long milliseconds) {
        if (!countdownHasTicked) {
            Timing.RunCoroutine(_FadeShootToStart());
            countdownHasTicked = true;
        }

        if (milliseconds < beginToFadeAt * 1000 && !hasExited) {
            Exit();
        }

        if (milliseconds <= 0) {
            countdownText.text = "GO";
            Timing.RunCoroutine(_Flash());
            return;
        }

        countdownText.text = Mathf.Ceil(milliseconds / 1000f).ToString("0");
    }

    private IEnumerator<float> _FadeShootToStart() {
        Color32 color = shootToStartText.color;
        byte initialAlpha = color.a;
        float timeAlive = 0;
        while (timeAlive < fadeDuration) {
            color.a = (byte)((1 - (timeAlive / fadeDuration)) * initialAlpha);
            shootToStartText.color = color;
            timeAlive += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        color.a = initialAlpha;
        shootToStartText.color = color;
        shootToStartText.gameObject.SetActive(false);
    }

    //TODO : REFACTOR THIS

    private IEnumerator<float> _Flash() {
        flash.gameObject.SetActive(true);

        flash.color = flashColor;
        Color32 color = flashColor;

        byte initialAlpha = color.a;

        float timeAlive = 0;
        while(timeAlive < flashDuration) {
            color.a = (byte)((1 - (timeAlive / flashDuration)) * initialAlpha);
            flash.color = color;
            timeAlive += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }

        flash.gameObject.SetActive(false);
    }
}