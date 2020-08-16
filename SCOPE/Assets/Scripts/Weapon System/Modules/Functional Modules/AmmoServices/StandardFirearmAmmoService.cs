#pragma warning disable 0649

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MEC;
using NagaUnityUtilities;

[RequireComponent(typeof(IUserCommunicator))]
[RequireComponent(typeof(IReloadService))]
[RequireComponent(typeof(IClip))]
public class StandardFirearmAmmoService : AmmoService {
    [Header("Customisations")]
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private int ammoUsedPerShot;
    [SerializeField] private int _ammoInReserve;
    [SerializeField] private bool canInterruptReload;

    private IReloadService reloader;
    private IClip clip;
    private IUserCommunicator userCommunicator;

    private ClipBasedAmmoInfo ammoInfo = new ClipBasedAmmoInfo();

    private bool isOnCooldown;

    private int ammoInReserve {
        get { return _ammoInReserve; }
        set { _ammoInReserve = value; OnAmmoChange(); }
    }

    private void Awake() {
        reloader = GetComponent<IReloadService>();
        clip = GetComponent<IClip>();
        userCommunicator = GetComponent<IUserCommunicator>();
        //clip.onAmmoChange.AddListener((x) => OnAmmoChange());
    }



    //Perhaps refactor this into two methods by using UnityEvents in IShooter? (But then there will be a dependency for this to have a shooter)

    /*By refactoring into two methods, it means that:
     * 
     *  This method will ONLY return a bool on whether shooting is available or not
     * 
     *  RegisterShot() is moved away from this method, and instead be added directly to OnShoot();
     *  
     *  However, at the same time, I want to keep the events for purely additional effects, and away from core functionality of the classes
     *  
     *  Right now, this implementation is probably the way to go. This will only go wrong when this returns true but the shooter
     *  class does not shoot
     *  
     *  ^^^ OUTDATED
     *  RequestToShoot() is now instead called from the IWeaponInputHandler class.
     *  Shooter class now only has 2 members: an onShoot event and a Shoot() method
     *  
     *  This way, a weapon can shoot multiple types of projectiles or hitscans at once, making them
     *  all modular
     */

    public override bool RequestToShoot() { //Naming a bit unintuitive : might change later
        if (!HasActiveCooldown()) {
            reloader.CancelReload();
            if (clip.TryUseAmmo(ammoUsedPerShot)) {
                RegisterShot();
                return true;
            } else { //Runs out of ammo
                RequestToReload();
                return false;
            }
        }
        return false;
    }

    protected override void RegisterShot() {
        ApplyCooldown();
        OnAmmoChange();
    }

    protected virtual bool HasActiveCooldown() {
        return (isOnCooldown || (reloader.IsReloading() && !canInterruptReload));
    }

    private void ApplyCooldown() {
        Timing.RunCoroutine(_ApplyCooldown());
    }

    private IEnumerator<float> _ApplyCooldown() {
        isOnCooldown = true;
        yield return Timing.WaitForSeconds(timeBetweenShots);
        isOnCooldown = false;
    }

    private void OnAmmoChange() {
        ammoInfo.SetAmmoInfo(clip.Remaining(), clip.Size(), ammoInReserve);
        userCommunicator.SetAmmoInfo(ammoInfo);
    }

    public override void RequestToReload() {
        if (!HasActiveCooldown() && !clip.IsFull()) {
            Reload();
        }
    }

    private void Reload() {
        reloader.Reload(clip, () => ammoInReserve, (ammoUsed) => {
            ammoInReserve -= ammoUsed;
        });
    }
}