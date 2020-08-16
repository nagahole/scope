using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown;

    private Resolution[] resolutions;

    private int currentResolutionIndex = -1;

    public TMP_Dropdown dropdown => _dropdown;
    

    // Start is called before the first frame update
    void Awake()
    {
        resolutions = Screen.resolutions;
        dropdown.ClearOptions();

        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++) {
            options.Add(resolutions[i].width.ToString() + " * " + resolutions[i].height.ToString());
        }

        dropdown.AddOptions(options);
    }

    public void SetResolution(Vector2 resolution) {
        int inputResolutionIndex = -1;

        for (int i = 0; i < resolutions.Length; i++) {
            if (resolution == new Vector2(resolutions[i].width, resolutions[i].height)) {
                inputResolutionIndex = i;
            }

            if (Screen.currentResolution.height == resolutions[i].height &&
                Screen.currentResolution.width == resolutions[i].width) {
                currentResolutionIndex = i;
            }
        }

        //This runs if the resolution in settings is contained in the possible resolutions
        if (inputResolutionIndex != -1) {
            currentResolutionIndex = inputResolutionIndex;
        } else {// elsedefault value if settings resolution cannot be applied
            Debug.LogWarning($"Settings Resolution cannot be found. Defaulting to current screen resolution ({resolution})");
        } 

        dropdown.value = currentResolutionIndex;
    }

    public Resolution GetSelectedResolution() {
        return resolutions[dropdown.value];
    }

    public Vector2 GetSelectedResolutionInVector2() {
        return new Vector2(resolutions[dropdown.value].width, resolutions[dropdown.value].height);
    }
}
