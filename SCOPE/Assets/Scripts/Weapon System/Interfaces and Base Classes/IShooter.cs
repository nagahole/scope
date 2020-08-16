using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MEC;

/// <summary>
/// Shooter interface that shoots based on input and whether is allowed from an ammoserviceprovider.
/// 
/// Requires an IAmmoServiceProvider and an IWeaponInputHandler to function
/// 
/// Shoot input should be added as an event listener to an IWeaponInputHandler
/// </summary>
/// 
public interface IShooter {
    /// <summary>
    /// Called on shooting a weapon. Should be a field-property pair with a private setter
    /// </summary>
    UnityEvent onShoot { get; }

    /// <summary>
    /// This should only contain the logic for the actual dealing damage part / creating projectile part of a gun
    /// </summary>
    void Shoot();
}

[RequireComponent(typeof(IAmmoService))]
public abstract class Shooter : MonoBehaviour, IShooter {
    [SerializeField] protected float delay;

    [SerializeField] private UnityEvent _onShoot;
    public UnityEvent onShoot {
        get {
            return _onShoot;
        }
        private set {
            _onShoot = value;
        }
    }

    public void Shoot() {
        if (delay == 0) {
            RegisterShot();
            return;
        }
        Timing.RunCoroutine(_DelayedShot());
    }

    private IEnumerator<float> _DelayedShot() {
        yield return Timing.WaitForSeconds(delay);
        RegisterShot();
    }

    private void RegisterShot() {
        HandleShot();
        onShoot.Invoke();
    }

    

    protected abstract void HandleShot();
}

