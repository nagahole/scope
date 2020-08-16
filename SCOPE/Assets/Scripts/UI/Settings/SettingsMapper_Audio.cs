using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMapper_Audio : SettingsMapper
{
    [SerializeField] SliderInputPair masterVolume, sfx, music;

    protected override void SetupInputEvents() {
        masterVolume.onValueChanged.AddListener((e) => { ApplySettings(); });
        sfx.onValueChanged.AddListener((e) => { ApplySettings(); });
        music.onValueChanged.AddListener((e) => { ApplySettings(); });
    }

    public override void ApplySettings() {
        AudioListener.volume = masterVolume.value;
        Settings.activeSettings.sfxVolume = sfx.value; //These are bit different because SFX and MUSIC are directly taken from settings
        Settings.activeSettings.musicVolume = music.value;
    }

    public override void MapToSettings() {
        Settings.activeSettings.masterVolume = masterVolume.value;
        Settings.activeSettings.sfxVolume = sfx.value;
        Settings.activeSettings.musicVolume = music.value;
    }

    public override void MapFromSettings() {
        masterVolume.SetValue(Settings.activeSettings.masterVolume);
        sfx.SetValue(Settings.activeSettings.sfxVolume);
        music.SetValue(Settings.activeSettings.musicVolume);
    }
}
