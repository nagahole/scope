using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new PersistentAudioSource", menuName ="ScriptableObjects/Singletons/PersistentAudioSource")]
public class PersistentAudioSource : ScriptableObject, ICanBeSingleton, IInitializable
{
    [SerializeField] AudioSource audioSourcePrefab;

    public AudioSource audioSource { get; private set; }

    public void Initialize() {
        audioSource = Instantiate(audioSourcePrefab);
        DontDestroyOnLoad(audioSource);
    }
}
