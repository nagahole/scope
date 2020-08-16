using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Simple interface to handle weapon input
/// </summary>
public interface IWeaponInputHandler
{
    /// <summary>
    /// Called once the weapon input is detected, whether programmatically or from a key. Should be a field-property pair with a private setter
    /// </summary>
    UnityEvent onFireInputReceived { get; }

    /// <summary>
    /// Should be using Unity's input system
    /// </summary>
    /// <returns></returns>
    bool FireInputReceived();
    bool ReloadInputReceived();
}

/// <summary>
/// This should only handle RAW input, so any special features such as chargeups should be handled in
/// the ammo service provider class
/// </summary>
[RequireComponent(typeof(IUserCommunicator))]
[RequireComponent(typeof(IAmmoService))]
public abstract class WeaponInputHandler : MonoBehaviour, IWeaponInputHandler {
    [Tooltip("Remember, this will only shoot if player's selected weapon is this")]
    [SerializeField] protected UnityEvent _onFireInputReceived;

    public UnityEvent onFireInputReceived {
        get {
            return _onFireInputReceived;
        }
        protected set {
            _onFireInputReceived = value;
        }
    }

    private IAmmoService ammoService;
    private IUserCommunicator userCommunicator;

    private void Awake() {
        ammoService = GetComponent<IAmmoService>();
        userCommunicator = GetComponent<IUserCommunicator>();
    }

    protected virtual void Update() {
        if (FireInputReceived() && userCommunicator.HeldWeapon() == gameObject) {
            if (ammoService.RequestToShoot()) {
                onFireInputReceived.Invoke();
            }
        }

        if (ReloadInputReceived()) {
            ammoService.RequestToReload();
        }
    }

    public virtual bool ReloadInputReceived() {
        return Input.GetButtonDown("Reload");
    }

    public abstract bool FireInputReceived();
}

