#pragma warning disable 0649

using System.Collections.Generic;
using UnityEngine;
using MEC;
using System;

public class PumpActionReload : ReloadService {
    [Header("Customisations")]
    [SerializeField] private int ammoReloadedPerPump;
    [SerializeField] private float reloadDurationPerPump;

    protected override IEnumerator<float> _Reload(IClip clip, Func<int> getReserveAmmo, Action<int> callback) {
        while (true) {
            yield return Timing.WaitForSeconds(reloadDurationPerPump);
            if(getReserveAmmo() <= 0) {
                yield break;
            }

            int ammoUsed = clip.TryFillAmmo(Mathf.Min(getReserveAmmo(), ammoReloadedPerPump));
            callback(ammoUsed);

            if (clip.IsFull()) {
                yield break;
            }
        }
    }
}
