using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        IHealth health = other.GetComponent<IHealth>();
        if(health != null) {
            health.SilentKill();
        }
    }
}
