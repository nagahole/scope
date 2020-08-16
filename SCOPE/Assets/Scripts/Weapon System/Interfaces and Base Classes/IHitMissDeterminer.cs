using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IHitMissDeterminer 
{
    UnityEvent onHit { get; }
    UnityEvent onMiss { get; }
}

public abstract class HitMissDeterminer : MonoBehaviour, IHitMissDeterminer {
    [SerializeField] private UnityEvent _onHit;
    [SerializeField] private UnityEvent _onMiss;
    public UnityEvent onHit { get { return _onHit; } protected set { _onHit = value; } }
    public UnityEvent onMiss { get { return _onMiss; } protected set { _onMiss = value; } }
}
