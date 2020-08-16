using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScenarioManagement;

public class ScenarioElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title, description;
    [SerializeField] private RectTransform optionSpawnRect;
    private ScenarioInfo scenarioInfo;

    public void SetScenarioInfo(ScenarioInfo scenarioInfo) {
        this.scenarioInfo = scenarioInfo;
        title.text = scenarioInfo.scenarioName;
        description.text = scenarioInfo.description;
    }

    public void CallScenarioSelectionUI() {
        var e = ScenarioSelectionOptions.instance;
        if(e.scenarioInfo != scenarioInfo || !e.shown) {
            e.Show();
            e.Goto(optionSpawnRect.position + Vector3.up * 5);
            e.TweenIn();
            e.SetScenarioInfo(scenarioInfo);
        }
    }

    private bool IsLeftSide() {
        return true;
    }
}
