using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NagaUnityUtilities;
using MEC;
using UnityEngine.Events;
using ScenarioManagement;

public class IngameMenuCommands : MonoBehaviour {
    [SerializeField] private GameObject _container;

    public GameObject container => _container;

    [SerializeField] private UnityEvent onShow, onHide;

    public void ReturnToMenu() {
        Hide();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(SOSingleton<KeySceneReferences>.sharedInstance.menuPath);
    }

    public void Replay() {
        SOSingleton<ScenarioManager>.sharedInstance.ReplayScenario();
        Hide();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Settings() {

    }

    public void Resume() {
        Hide();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Show() {
        SOSingleton<ScenarioManager>.sharedInstance.Pause();
        container.SetActive(true);
        onShow.Invoke();
    }

    private void Hide() {
        SOSingleton<ScenarioManager>.sharedInstance.Resume();
        container.SetActive(false);
        onHide.Invoke();
    }

    public void ToggleMenu() {
        if (container.activeSelf) {
            Hide();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }
        Show();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
