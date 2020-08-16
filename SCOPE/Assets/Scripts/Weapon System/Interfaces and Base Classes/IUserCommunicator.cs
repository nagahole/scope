using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IUserCommunicator
{
    Vector3 LookStartPos();
    Vector3 LookDirection();
    Quaternion LookRotation();

    void AddVelocity(Vector3 vel);

    void AddRecoil(float intensity, float angle);

    GameObject HeldWeapon();

    void SetAmmoInfo(AmmoInfo info);
}

public abstract class UserCommunicator : MonoBehaviour, IUserCommunicator {
    public abstract void AddVelocity(Vector3 vel);
    public abstract Vector3 LookDirection();
    public abstract Vector3 LookStartPos();
    public abstract Quaternion LookRotation();
    public abstract void AddRecoil(float intensity, float angle);
    public abstract GameObject HeldWeapon();
    public abstract void SetAmmoInfo(AmmoInfo info);
}
