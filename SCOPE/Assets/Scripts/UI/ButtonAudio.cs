using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class ButtonAudio : MonoBehaviour
{
    [SerializeField] AudioClip highlightSound, selectSound;
    [SerializeField] [Range(0,1)] float highlightVolume, selectVolume;

    public void PlayHighlightSound() {
        SOSingleton<PersistentAudioSource>.sharedInstance.audioSource.PlayOneShot(highlightSound, highlightVolume * Settings.activeSettings.sfxVolume);
    }


    public void PlaySelectSound() {
        SOSingleton<PersistentAudioSource>.sharedInstance.audioSource.PlayOneShot(selectSound, selectVolume * Settings.activeSettings.sfxVolume);
    }
}
