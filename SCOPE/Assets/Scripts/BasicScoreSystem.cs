#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This does not handle UI. UI is handled in the ScenarioManager with the ScenarioSession events
//This class only handles calculating the change in score
public class BasicScoreSystem : ScoreSystem {
    [Header("CUSTOMISATIONS")]
    [SerializeField][ReadOnly] private float points;

    public override void NotifyPoints(float basePoints) {
        float changeInPoints = basePoints;
        points += changeInPoints;
        onScoreChanged?.Invoke(changeInPoints);
    }

    public override void ResetPoints() {
        points = 0;
    }
}
