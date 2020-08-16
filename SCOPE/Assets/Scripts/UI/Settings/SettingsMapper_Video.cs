using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingsMapper_Video : SettingsMapper
{
    [SerializeField] ResolutionDropdown resolutionDropdown; 
    [SerializeField] Toggle fullscreenToggle, vsyncToggle;
    [SerializeField] SliderInputPair fovSlider;

    Camera mainCamera;

    protected override void Start() {
        base.Start();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected override void GetCaches() {
        mainCamera = Camera.main;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
        mainCamera = Camera.main;
    }

    protected override void SetupInputEvents() {
        resolutionDropdown.dropdown.onValueChanged.AddListener((e) => { ApplySettings(); });
        fullscreenToggle.onValueChanged.AddListener((e) => { ApplySettings(); });
        vsyncToggle.onValueChanged.AddListener((e) => { ApplySettings(); });
        resolutionDropdown.dropdown.onValueChanged.AddListener((e) => { ApplySettings(); });
        fovSlider.onValueChanged.AddListener((e) => { ApplySettings(); });
    }

    public override void MapFromSettings() {
        resolutionDropdown.SetResolution(Settings.activeSettings.resolution);
        fullscreenToggle.isOn = Settings.activeSettings.fullscreen;
        vsyncToggle.isOn = Settings.activeSettings.vsync;
        fovSlider.SetValue(Settings.activeSettings.fov);
        ApplySettings();
    }

    public override void MapToSettings() {
        Settings.activeSettings.resolution = resolutionDropdown.GetSelectedResolutionInVector2();
        Settings.activeSettings.vsync = vsyncToggle.isOn;
        Settings.activeSettings.fullscreen = fullscreenToggle.isOn;
        Settings.activeSettings.fov = fovSlider.value;
        ApplySettings();
    }

    //Always get from the components, because settings is not updated until going back to main menu
    public override void ApplySettings() {
        Vector2 resolution = resolutionDropdown.GetSelectedResolutionInVector2();
        Screen.SetResolution((int)resolution.x, (int) resolution.y, fullscreenToggle.isOn);
        QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0; 
        if(mainCamera != null) {
            mainCamera.fieldOfView = fovSlider.value;
        }
    }
}
