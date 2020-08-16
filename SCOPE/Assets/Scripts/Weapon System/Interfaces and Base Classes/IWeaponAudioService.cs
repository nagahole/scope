using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponAudioService
{
    void PlayAudioclip(int numberInList);
}

public abstract class WeaponAudioService : MonoBehaviour, IWeaponAudioService {
    public abstract void PlayAudioclip(int numberInList);
}