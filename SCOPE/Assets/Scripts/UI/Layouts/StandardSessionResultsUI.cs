using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using ScenarioManagement;
using UnityEngine.SceneManagement;
using UnityEditor;

public interface ISessionResultsUI {
    void Show();
    void Hide();
    void Setup();
    void Exit(); //TODO : Make this into just SetSessionInfo and handle logic within this class?
    void SetScore(float score);
    void SetHighscore(float score);
    void IsNewHighscore(bool isNewHighscore);
    void SetKills(int kills);
    void SetAccuracy(int hits, int misses);
    void SetTitle(string name);
    void SetScoreText(string text);
    void SetHighscoreText(string text);
    void SetScoreDescriber(string text);
    void SetHighscoreDescriber(string text);
}

public class StandardSessionResultsUI : MonoBehaviour, ISessionResultsUI {
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI titleText, hitsText, missesText, scoreText, accuracyText, highscoreText, killsText, scoreDescriber, highscoreDescriber;
    [SerializeField] private GameObject highscoreIndicator;
    [SerializeField] private GameObject container;

    public void Setup() {
        animator.SetTrigger("Setup");
    }

    public void Exit() {
        animator.SetTrigger("Exit");
    }

    public void SetTitle(string title) {
        titleText.text = title;
    }

    public void SetScore(float score) {
        scoreText.text = score.ToString("0.#");
    }

    public void SetAccuracy(int hits, int misses) {
        hitsText.text = hits.ToString();
        missesText.text = misses.ToString();
        accuracyText.text = string.Join("%", (((float) hits / (hits + misses)) * 100f).ToString("#.0") ,string.Empty);
    }

    public void SetHighscore(float score) {
        highscoreText.text = score.ToString("0.#");
    }

    public void IsNewHighscore(bool isNewHighscore) {
        highscoreIndicator.gameObject.SetActive(isNewHighscore);
    }

    public void SetKills(int kills) {
        killsText.text = kills.ToString();
    }

    public void MENU_BUTTON() {
        SceneManager.LoadScene(SOSingleton<KeySceneReferences>.sharedInstance.menuPath);
    }

    public void PLAYAGAIN_BUTTON() {
        SOSingleton<ScenarioManager>.sharedInstance.ReplayScenario();
    }

    public void Show() {
        container.gameObject.SetActive(true);
    }

    public void Hide() {
        container.gameObject.SetActive(false);
    }

    /// <summary>
    /// Incase of any special stuff
    /// </summary>
    /// <param name="text"></param>
    public void SetScoreText(string text) {
        scoreText.text = text;
    }

    public void SetHighscoreText(string text) {
        highscoreText.text = text;
    }

    public void SetScoreDescriber(string text) {
        scoreDescriber.text = text;
    }

    public void SetHighscoreDescriber(string text) {
        highscoreDescriber.text = text;
    }
}
