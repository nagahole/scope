using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SettingsMapper_Game : SettingsMapper
{
    [SerializeField] private SliderInputPair mouseSensitivity, mouseSensitivityX, mouseSensitivityY;
    [SerializeField] private Toggle rawInputToggle;

    public override void MapFromSettings() {
        mouseSensitivity.SetValue(Settings.activeSettings.mouseSensitivity);
        mouseSensitivityX.SetValue(Settings.activeSettings.mouseSensitivityAxis.x);
        mouseSensitivityY.SetValue(Settings.activeSettings.mouseSensitivityAxis.y);
        rawInputToggle.isOn = Settings.activeSettings.rawInput;
    }

    public override void MapToSettings() {
        Settings.activeSettings.mouseSensitivity = mouseSensitivity.value;
        Settings.activeSettings.mouseSensitivityAxis.x = mouseSensitivityX.value;
        Settings.activeSettings.mouseSensitivityAxis.y = mouseSensitivityY.value;
        Settings.activeSettings.rawInput = rawInputToggle.isOn;
    }
}
