#pragma warning disable 0649

using UnityEngine;

[RequireComponent(typeof(IUserCommunicator))]
public class BasicWeaponRecoil : WeaponRecoil {
    [Header("Customisations")]
    [SerializeField] private float intensity;
    [SerializeField] private float intensityVariance;
    [SerializeField] private float angle;
    [SerializeField] private float angleVariance;

    private IUserCommunicator userCommunicator;

    private void Awake() {
        userCommunicator = GetComponent<IUserCommunicator>();
    }

    public override void AddRecoil() {
        userCommunicator.AddRecoil(intensity + (Random.value - 0.5f) * intensityVariance, angle + (Random.value - 0.5f) * angleVariance);
    }
}
