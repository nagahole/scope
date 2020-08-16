using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScenarioManagement;

public class LockedUntilTimerStart : OnTimerStart {
    private Vector3 startingPos;
    private Quaternion rotation;

    private void Awake() {
        startingPos = transform.position;
        rotation = transform.localRotation;
    }

    private void LateUpdate() {
        transform.position = startingPos;
        transform.localRotation = rotation;
    }

    protected override void OnScenarioStarted() {
        this.enabled = false;
    }
}
