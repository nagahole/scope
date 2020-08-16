#pragma warning disable 0649

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class StandardClip : Clip {
    [SerializeField] int clipSize;
    [ReadOnly][SerializeField] private int _ammo;
    
    private int ammo {
        get { return _ammo; }
        set {
            _ammo = value;
            onAmmoChange.Invoke(value);
        }
    }

    public override int TryFillAmmo(int reserve) {
        int unfilledSlots = clipSize - ammo;
        int refill = Mathf.Min(unfilledSlots, reserve);

        ammo += refill;

        return refill;
    }

    public override bool TryAddAmmo(int n) {
        if(ammo + n > clipSize) {
            return false;
        }
        ammo += n;
        return true;
    }

    public override bool TryUseAmmo(int n) {
        if (ammo >= n) {
            ammo -= n;
            return true;
        }
        return false;
    }

    public override bool IsFull() {
        return ammo == clipSize;
    }

    public override int Remaining() {
        return ammo;
    }

    public override int Size() {
        return clipSize;
    }
}