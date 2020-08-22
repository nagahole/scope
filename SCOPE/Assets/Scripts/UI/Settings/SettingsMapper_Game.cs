using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SettingsMapper_Game : SettingsMapper
{
    [SerializeField] private SensitivityPresetDropdown sensitivityPresetDropdown;
    [SerializeField] private SliderInputPair mouseSensitivityX, mouseSensitivityY;
    [SerializeField] private Toggle lockXandYToggle, rawInputToggle;

    protected override void SetupInputEvents() {
        sensitivityPresetDropdown.dropdown.onValueChanged.AddListener(SensitivityPresetDropdownChanged);
        mouseSensitivityX.onValueChanged.AddListener((f) => {
            if (lockXandYToggle.isOn)
                mouseSensitivityY.SilentSetValue(f);
        });
        mouseSensitivityY.onValueChanged.AddListener((f) => {
            if(lockXandYToggle.isOn)
                mouseSensitivityX.SilentSetValue(f);
        });
    }

    public override void MapFromSettings() {
        sensitivityPresetDropdown.SetPreset(Settings.activeSettings.sensitivityPreset);
        ApplySensitivityPreset(sensitivityPresetDropdown.GetSelectedPreset());

        mouseSensitivityX.SetValue(Settings.activeSettings.mouseSensitivity.x);
        mouseSensitivityY.SetValue(Settings.activeSettings.mouseSensitivity.y);

        rawInputToggle.isOn = Settings.activeSettings.rawInput;
        lockXandYToggle.isOn = Settings.activeSettings.lockXandY;
    }

    public override void MapToSettings() {
        Settings.activeSettings.sensitivityPreset = sensitivityPresetDropdown.GetSelectedPreset();
        Settings.activeSettings.mouseSensitivity.x = mouseSensitivityX.value;
        Settings.activeSettings.mouseSensitivity.y = mouseSensitivityY.value;
        Settings.activeSettings.rawInput = rawInputToggle.isOn;
        Settings.activeSettings.lockXandY = lockXandYToggle.isOn;
    }

    private void SensitivityPresetDropdownChanged(int newVal) {
        ApplySensitivityPreset(sensitivityPresetDropdown.GetSelectedPreset());
    }

    private void ApplySensitivityPreset(SensitivityPreset preset) {
        mouseSensitivityX.SetRange(preset.range);
        mouseSensitivityY.SetRange(preset.range);
        mouseSensitivityX.SetValue(mouseSensitivityX.value);
        mouseSensitivityY.SetValue(mouseSensitivityY.value);
    }
}
