using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DamageTakenEvent : UnityEvent<float> { }

public interface IHealth
{
    UnityEvent onDie { get; }
    UnityEvent onSilenceDie { get; }
    DamageTakenEvent onDamageTaken { get; }
    void Damage(float damage);
    void Heal(float healing);
    void MaxHealth();
    void SilentKill();
}

public abstract class Health : MonoBehaviour, IHealth {
    [SerializeField] private UnityEvent _onDie;
    [SerializeField] private UnityEvent _onSilenceDie;
    [SerializeField] private DamageTakenEvent _onDamageTaken;

    public UnityEvent onDie {
        get { return _onDie; }
        private set { _onDie = value; }
    }

    public UnityEvent onSilenceDie {
        get { return _onSilenceDie; }
        private set { _onSilenceDie = value; }
    }

    public DamageTakenEvent onDamageTaken {
        get { return _onDamageTaken; }
        private set { _onDamageTaken = value; }
    }

    public abstract void MaxHealth();
    public abstract void Damage(float damage);
    public abstract void Heal(float healing);
    public abstract void SilentKill();
}