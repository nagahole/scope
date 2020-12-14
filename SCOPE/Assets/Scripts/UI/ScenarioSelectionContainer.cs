using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NagaUnityUtilities;
using MEC;

public class ScenarioSelectionContainer : MonoBehaviour
{
    [SerializeField] private ScenarioInfoContainer scenarioContainer;
    [SerializeField] private ScenarioElement elementPrefab;
    [SerializeField] private GridLayoutGroup gridLayout;

    private RectTransform targetTransform;

    private void Awake() {
        targetTransform = gridLayout.GetComponent<RectTransform>();
    }

    private void Start() {
        CreateScenarios();
    }

    private void CreateScenarios() {
        for(int i = 0; i < scenarioContainer.scenarios.Length; i++) {
            CreateElement(scenarioContainer.scenarios[i]);
        }
        AdjustHeight();
        //Timing.RunCoroutine(_FixSize(), Segment.SlowUpdate);
    }

    private IEnumerator<float> _FixSize() {
        yield return Timing.WaitForOneFrame;
        while (true) {
            if (targetTransform == null)
                yield break;
            targetTransform.sizeDelta += new Vector2(1, 0);
            yield return Timing.WaitForOneFrame;
            if (targetTransform == null)
                yield break;
            targetTransform.sizeDelta += new Vector2(-1, 0);
        }
    }

    private void CreateElement(ScenarioInfo info) {
        ScenarioElement element = Instantiate(this.elementPrefab);
        element.SetScenarioInfo(info);
        element.transform.SetParent(targetTransform);
        element.transform.localScale = new Vector3(1, 1, 1);
    }

    private void AdjustHeight() {
        targetTransform.sizeDelta = new Vector2(targetTransform.sizeDelta.x, gridLayout.preferredHeight);
    }
}
