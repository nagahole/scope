#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MovementStatsDisplayer : MonoBehaviour
{
    [SerializeField] private PlayerMovement movementScript;

    [SerializeField] private TextMeshProUGUI horizontalSpeedText, verticalSpeedText, bunnyhopAccuracyText;

    private void Update() {
        horizontalSpeedText.text = $"Horizontal Speed: {movementScript.horizontalSpeed}";
        verticalSpeedText.text = $"Vertical Speed: {movementScript.verticalSpeed}";
        bunnyhopAccuracyText.text = $"Bhop Accuracy: {Mathf.Round(movementScript.bunnyhopAccuracy * 100 * 10) / 10f}";
    }
}
