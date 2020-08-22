using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SettingsSoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] [Range(0,1)]private float hitsoundMultiplier;
    [SerializeField] private AudioClip hitsound;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHitsound(float volume) {
        if (Settings.activeSettings.playHitsound) {
            audioSource.PlayOneShot(hitsound, Settings.activeSettings.sfxVolume * volume * hitsoundMultiplier);
        }
    }  
}
