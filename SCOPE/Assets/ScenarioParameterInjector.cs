using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioParameterInjector : MonoBehaviour
{
    [SerializeField] UnityEvent injection;

    private void Awake() {
        injection.Invoke();
    }
}
