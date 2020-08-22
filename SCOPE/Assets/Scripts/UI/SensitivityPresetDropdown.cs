using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SensitivityPresetDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown;

    [SerializeField] private SensitivityPreset[] presets;

    private int currentPresetIndex = -1;

    public TMP_Dropdown dropdown => _dropdown;

    private void Awake() {
        dropdown.ClearOptions();

        List<string> options = new List<string>();
        for(int i = 0; i < presets.Length; i++) {
            options.Add(presets[i].name);
        }
        dropdown.AddOptions(options);
    }

    public void SetPreset(SensitivityPreset preset) {
        int inputPresetIndex = -1;
        for (int i = 0; i < presets.Length; i++) {
            if (preset == presets[i]) {
                inputPresetIndex = i;
            }
        }

        if (inputPresetIndex == -1) {//none found
            currentPresetIndex = 0; //defaults to first item in dropdown
        } else {
            currentPresetIndex = inputPresetIndex;
        }

        dropdown.value = currentPresetIndex;
    }

    public SensitivityPreset GetSelectedPreset() {
        return presets[dropdown.value];
    }
}
