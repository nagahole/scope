#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GenericWeaponAudioService : WeaponAudioService
{
    [System.Serializable]
    private struct AudioclipSettingsPair {
        [Header("Audio")]
        public AudioClip audioclip;
        [Range(0,1)] public float volume;
        public float volumeVariance;
        public bool consistentVolume;

        [Space]
        [Header("Positioning")] [Tooltip("Only use this when consistent volume is off")]
        public Transform transform;
        public Vector3 relativeOffset;
        public Vector3 absoluteOffset;
    }

    [Header("Customisations")]
    [SerializeField] AudioclipSettingsPair[] audioclipSettingsPairs;

    AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public override void PlayAudioclip(int numberInList) {
        try {

            AudioclipSettingsPair info = audioclipSettingsPairs[numberInList];
            if (info.consistentVolume) {
                audioSource.PlayOneShot(info.audioclip, info.volume * Settings.activeSettings.sfxVolume);
            } else {
                AudioSource.PlayClipAtPoint(info.audioclip, info.transform.TransformPoint(info.relativeOffset) + info.absoluteOffset, info.volume * Settings.activeSettings.sfxVolume);
            }

        } catch (System.Exception e) {
            Debug.LogWarning($"Weapon Audio Service Error in {gameObject} : {e.Message}");
        }
    }

}
