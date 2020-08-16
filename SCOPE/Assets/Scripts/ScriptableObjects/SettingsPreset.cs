using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsPreset {
    [Header("Game")]
    public float mouseSensitivity;
    public Vector2 mouseSensitivityAxis;
    public bool rawInput;

    [Header("Video")]
    public Vector2 resolution;
    public bool fullscreen;
    public bool vsync;

    public float fov;

    [Header("Audio")]
    public float masterVolume;

    public float sfxVolume;

    public float musicVolume;

    [Space]
    public AudioClip hitSound;

    public bool playHitsound;

}


