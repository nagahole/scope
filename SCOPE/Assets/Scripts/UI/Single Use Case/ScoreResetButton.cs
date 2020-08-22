using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScenarioManagement;

public class ScoreResetButton : MonoBehaviour
{
    [SerializeField] private ConfirmBox confirmBox;

    public void OnResetButtonClicked() {
        confirmBox.SetTitle("Reset Highscores");
        confirmBox.SetBody("Are you sure you want to reset all highscores? This cannot be undone");
        confirmBox.SetConfirmAction(OnConfirm);
        confirmBox.Enter();
    }

    private void OnConfirm() {
        SOSingleton<ScenarioManager>.sharedInstance.highscoreTracker.Purge();
    }
}
