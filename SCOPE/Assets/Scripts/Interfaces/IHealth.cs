using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DamageTakenEvent : UnityEvent<float> { }

[System.Serializable]
public class DeathEvent : UnityEvent<DeathInformation> { }

[System.Serializable]
public struct DeathInformation {
    public Vector3 deathPosition;

    public DeathInformation(Vector3 deathPosition) {
        this.deathPosition = deathPosition;
    }
}

public interface IHealth
{
    DeathEvent onDie { get; }
    DeathEvent onSilenceDie { get; }
    DamageTakenEvent onDamageTaken { get; }
    void Damage(float damage);
    void Heal(float healing);
    void FillToMaxHealth();
    void SilentKill();
    float GetHealth();
    float GetMaxHealth();
}

public abstract class Health : MonoBehaviour, IHealth {
    [SerializeField] private DeathEvent _onDie;
    [SerializeField] private DeathEvent _onSilenceDie;
    [SerializeField] private DamageTakenEvent _onDamageTaken;

    public DeathEvent onDie {
        get { return _onDie; }
        private set { _onDie = value; }
    }

    public DeathEvent onSilenceDie {
        get { return _onSilenceDie; }
        private set { _onSilenceDie = value; }
    }

    public DamageTakenEvent onDamageTaken {
        get { return _onDamageTaken; }
        private set { _onDamageTaken = value; }
    }

    public abstract void FillToMaxHealth();
    public abstract void Damage(float damage);
    public abstract void Heal(float healing);
    public abstract void SilentKill();
    public abstract float GetMaxHealth();
    public abstract float GetHealth();
}