#pragma warning disable 0649

using UnityEngine;

public class TransformBasedFirePos : WeaponFirePos {
    [Header("Customisations")]
    [SerializeField] Transform firepos;

    public override Vector3 GetWeaponFirePos() {
        return firepos.position;
    }
}