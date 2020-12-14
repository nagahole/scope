using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthJoiner : Health
{
    [SerializeField] MonoBehaviour sharedHealthObject;
    [SerializeField] float damageMultiplier = 1;

    private IHealth sharedHealth;

    private void Awake() {
        sharedHealth = sharedHealthObject as IHealth;
    }

    public override void Damage(float damage) {
        sharedHealth.Damage(damage * damageMultiplier);
        Debug.Log(damageMultiplier);
    }

    public override void Heal(float healing) {
        sharedHealth.Heal(healing);
    }

    public override void FillToMaxHealth() {
        sharedHealth.FillToMaxHealth();
    }

    public override void SilentKill() {
        sharedHealth.SilentKill();
    }

    public override float GetMaxHealth() {
        return sharedHealth.GetMaxHealth();
    }

    public override float GetHealth() {
        return sharedHealth.GetHealth();
    }
}
