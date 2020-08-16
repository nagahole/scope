#pragma warning disable 0649

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MEC;

public class ClipReload : ReloadService {
    [Header("Customisations")]
    [SerializeField] private float reloadDuration;

    //onReloaded not to be confused with UnityEvent onReloadFinished. onReloaded is called internally 
    protected override IEnumerator<float> _Reload(IClip clip, Func<int> ammoInReserve, System.Action<int> callback) {
        yield return Timing.WaitForSeconds(reloadDuration);
        int ammoUsed = clip.TryFillAmmo(ammoInReserve());
        callback(ammoUsed);
    }
}