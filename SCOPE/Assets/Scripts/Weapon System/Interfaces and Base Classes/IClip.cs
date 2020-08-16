using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// A generic ammo container used as a communication interface between ReloadService and AmmoService. 
/// Note that IClip is used instead of IAmmoContainer for easier use - so this can still be treated
/// as just a generic ammo container.
/// </summary>
public interface IClip {
    /// <summary>
    /// Called when ammo count is changed. Should be a field-property pair with a private setter
    /// </summary>
    OnAmmoChangedEvent onAmmoChange { get; }

    /// <summary>
    /// Tries to fill clip with as much ammo as possible while not overfilling it.
    /// Returns how much ammo is used to refill
    /// </summary>
    /// <param name="ammoInReserve"></param>
    /// <returns>Bool on whether clip has been successfully filled</returns>
    int TryFillAmmo(int ammoInReserve);

    /// <summary>
    /// Tries to add ammo. If adding the ammo would cause the clip to hold more than it is designed to, it will refuse and return false, else return true
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    bool TryAddAmmo(int n);

    /// <summary>
    /// If it can be used, it will be used and return true, else return false
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    bool TryUseAmmo(int n);

    bool IsFull();

    int Remaining();
    int Size();
}

[System.Serializable]
public class OnAmmoChangedEvent : UnityEvent<int> { }

public abstract class Clip : MonoBehaviour, IClip {



    [SerializeField] private OnAmmoChangedEvent _onAmmoChange;

    public OnAmmoChangedEvent onAmmoChange {
        get { return _onAmmoChange; }
        private set { _onAmmoChange = value; }
    }

    public abstract bool IsFull();

    public abstract bool TryAddAmmo(int n);

    public abstract int TryFillAmmo(int ammoInReserve);

    public abstract bool TryUseAmmo(int n);

    public abstract int Size();
    public abstract int Remaining();
}