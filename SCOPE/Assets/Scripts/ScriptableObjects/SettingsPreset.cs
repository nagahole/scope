using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsPreset {
    
    public float effectiveMouseSensitivityX => mouseSensitivity.x * sensitivityPreset.sensitivityMultiplier;
    public float effectiveMouseSensitivityY => mouseSensitivity.y * sensitivityPreset.sensitivityMultiplier;
    [Header("Game")]
    public SensitivityPreset sensitivityPreset;
    public Vector2 mouseSensitivity;
    public bool lockXandY;
    public bool rawInput;

    [Header("Video")]
    public Vector2 resolution;
    public bool fullscreen;
    public bool vsync;

    public float fov;
    public int fpsCap;

    [Header("Audio")]
    public float masterVolume;

    public float sfxVolume;

    public float musicVolume;

    [Space]

    public bool playHitsound;

}


