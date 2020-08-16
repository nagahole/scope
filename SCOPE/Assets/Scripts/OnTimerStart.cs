using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScenarioManagement;

public abstract class OnTimerStart : MonoBehaviour
{
    protected virtual void Start() {
        SOSingleton<ScenarioManager>.sharedInstance.onScenarioStarted.RemoveListener(OnScenarioStarted);
        SOSingleton<ScenarioManager>.sharedInstance.onScenarioStarted.AddListener(OnScenarioStarted);
    }

    protected abstract void OnScenarioStarted();
}
