using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillNotifier : MonoBehaviour
{
    public static readonly UnityEvent onKill = new UnityEvent();

    public void NotifyKill() {
        Debug.Log("KILLED");
        onKill.Invoke();
    }
}
