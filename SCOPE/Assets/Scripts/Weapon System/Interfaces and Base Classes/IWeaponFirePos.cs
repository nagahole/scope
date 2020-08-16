using UnityEngine;

public interface IWeaponFirePos {
    Vector3 GetWeaponFirePos();
}

public abstract class WeaponFirePos : MonoBehaviour, IWeaponFirePos {
    public abstract Vector3 GetWeaponFirePos();
}