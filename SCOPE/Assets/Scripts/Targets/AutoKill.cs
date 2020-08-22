using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

[RequireComponent(typeof(IHealth))]
public class AutoKill : MonoBehaviour
{
    [SerializeField] private float secondsBeforeDeath;

    private CoroutineHandle coroutine;
    // Start is called before the first frame update
    void OnEnable()
    {
        coroutine = Timing.RunCoroutine(_AutoKill());
    }

    private void OnDisable() {
        Timing.KillCoroutines(coroutine);
    }

    private IEnumerator<float> _AutoKill() {
        yield return Timing.WaitForSeconds(secondsBeforeDeath);
        GetComponent<IHealth>().SilentKill();
    }
}
