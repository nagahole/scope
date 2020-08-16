using UnityEngine;

public interface IWeaponRecoil
{
    void AddRecoil();
}

public abstract class WeaponRecoil : MonoBehaviour, IWeaponRecoil {
    public abstract void AddRecoil();
}