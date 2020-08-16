using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface to handle all the logic on the gun's ammunition, such as cooldowns, reloading, and clipsize. All lower dependencies should only communicate with
/// this interface, and not with each other
/// </summary>
public interface IAmmoService {
    /// <summary>
    /// Requests permission to shoot the gun. If it can, it will automatically handle the logic once the weapon is shot with a RegisterShot() function
    /// 
    /// Note that this method will not actually shoot the weapon, and is instead a verifier to whether the weapon can shoot or not
    /// </summary>
    /// <returns>Returns bool on whether the gun is allowed to shoot</returns>
    bool RequestToShoot();

    void RequestToReload();
}

public abstract class AmmoService : MonoBehaviour, IAmmoService {
    public abstract bool RequestToShoot();
    public abstract void RequestToReload();

    protected abstract void RegisterShot();
}
