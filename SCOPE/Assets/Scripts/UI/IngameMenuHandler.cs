using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using UnityEngine.SceneManagement;
using ScenarioManagement;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="new IngameMenuHandler", menuName ="ScriptableObjects/Singletons/IngameMenuHandler")]
public class IngameMenuHandler : ScriptableObject, IInitializable, ICanBeSingleton
{
    [SerializeField] KeyCode hotkey = KeyCode.Escape, restartHotkey = KeyCode.F2;
    [SerializeField] IngameMenuCommands menuPrefab;

    private IngameMenuCommands menu;

    public void Initialize() {
        Timing.RunCoroutine(_Update());
        menu = Instantiate(menuPrefab);
        DontDestroyOnLoad(menu);
    }

    private IEnumerator<float> _Update() {
        while (true) {
            Update();
            yield return Timing.WaitForOneFrame;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(hotkey) && SceneManager.GetActiveScene().path != SOSingleton<KeySceneReferences>.sharedInstance.menuPath
            && SOSingleton<ScenarioManager>.sharedInstance.scenarioState != ScenarioState.Results) { //need to change this incase this gets more complicated TODO
            menu.ToggleMenu();
        }

        if(Input.GetKeyDown(restartHotkey) && SOSingleton<ScenarioManager>.sharedInstance.IsPlaying()) {
            menu.Replay();
        }
    }
}
