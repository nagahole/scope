using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class ScriptableSingletons : MonoBehaviour
{
    [SerializeField] ScriptableObject[] objects;

    public void Awake() {
        Timing.RunCoroutine(PeriodicallyLogObjects());
    }

    private IEnumerator<float> PeriodicallyLogObjects() {
        while (true) {
            yield return Timing.WaitForSeconds(5);
            foreach (ScriptableObject obj in objects) {
                obj.GetType();
            }
        }
    }
}
