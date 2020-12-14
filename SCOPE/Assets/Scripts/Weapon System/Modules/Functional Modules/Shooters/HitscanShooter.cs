#pragma warning disable 0649

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NagaUnityUtilities;
using MEC;

[System.Serializable]
public struct ShootInfo {
    public Vector3 start;
    public Vector3 end;
    public RaycastHit hitInfo;
    public bool hitHealthComponent;

    public ShootInfo(Vector3 start, Vector3 end, RaycastHit hitInfo, bool hitHealthComponent) {
        this.start = start;
        this.end = end;
        this.hitInfo = hitInfo;
        this.hitHealthComponent = hitHealthComponent;
    }
}

[System.Serializable]
public class ShootInfoEvent : UnityEvent<ShootInfo[]> { }

/// <summary>
/// Bullet will travel from face to straight in front but will render a trail from the barrel
/// </summary>
[RequireComponent(typeof(IUserCommunicator))]
public class HitscanShooter : Shooter {
    [Header("Customisations")]
    [SerializeField] protected int bullets;
    [SerializeField] protected float damage;
    [SerializeField] protected float range;
    [SerializeField] protected float inaccuracyInDegrees;
    [SerializeField] protected LayerMask layermask;
    [Space]
    [SerializeField] private ShootInfoEvent _onHitscanShoot;

    private IUserCommunicator userCommunicator;

    protected void Awake() {
        userCommunicator = GetComponent<IUserCommunicator>();
    }

    public ShootInfoEvent onHitscanShoot {
        get { return _onHitscanShoot; }
        private set { _onHitscanShoot = value; }
    }

    protected override void HandleShot() {
       for(int i = 0; i < bullets; i++) {
            FireOnce();
        }
    }

    protected virtual void FireOnce() {
        Vector3 start = userCommunicator.LookStartPos();
        Vector3 end;

        bool hitHealthComponent = false;

        Vector3 direction = Quaternion.Euler(InaccuracyOffset()) * userCommunicator.LookDirection();

        RaycastHit hitInfo;

        if (Physics.Raycast(start, direction, out hitInfo, range, layermask)) { //Hits something
            end = hitInfo.point;
            IHealth health = hitInfo.collider.GetComponent<IHealth>();
            Debug.Log(hitInfo.collider);
            if (health != null) {
                hitHealthComponent = true;
                health.Damage(damage);
            }
        } else {
            end = NagaUtils.GetPointAlongDirection(start, direction, range);
        }

        Debug.DrawLine(start, end, Color.white, 0.4f);

        onHitscanShoot.Invoke((new ShootInfo[] { new ShootInfo(start, end, hitInfo, hitHealthComponent) }));
    }

    //Handles inaccuracy as well
    protected virtual Vector3 InaccuracyOffset() {
        return inaccuracyInDegrees * new Vector3(
            Random.value - 0.5f,
            Random.value - 0.5f,
            Random.value - 0.5f
        ).normalized;
    }
}