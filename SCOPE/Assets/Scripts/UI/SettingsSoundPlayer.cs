using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SettingsSoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] [Range(0,1)]private float hitsoundMultiplier;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHitsound(float volume) {
        if (Settings.activeSettings.playHitsound) {
            audioSource.PlayOneShot(Settings.activeSettings.hitSound, Settings.activeSettings.sfxVolume * volume * hitsoundMultiplier);
        }
    }  
}
