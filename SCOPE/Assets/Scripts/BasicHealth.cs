#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NagaUnityUtilities;

public class BasicHealth : Health {
    [Header("CUSTOMISATIONS")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;

    public override void MaxHealth() {
        health = maxHealth;
    }

    public override void Damage(float damage) {
        damage = Mathf.Min(damage, health);

        if (damage < 0) {
            return;
        }

        health -= damage;
        if(health <= 0.00001f) { //In case of floating precision point errors
            Die();
            return;
        }
        
        onDamageTaken.Invoke(damage);
    }

    public override void Heal(float amount) {
        if (amount < 0) {
            return;
        }

        health += Mathf.Min(amount, maxHealth - health); //heals to max
    }

    private void Die() {
        onDie.Invoke();
        gameObject.SetActive(false);
    }

    [ContextMenu("Silently Kill")]
    public override void SilentKill() {
        onSilenceDie.Invoke();
        health = 0;
        gameObject.SetActive(false);
    }
}
