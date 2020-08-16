using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

[RequireComponent(typeof(IHealth))]
public class AutoKill : MonoBehaviour
{
    [SerializeField] private float secondsBeforeDeath;
    // Start is called before the first frame update
    void OnEnable()
    {
        Timing.RunCoroutine(_AutoKill().CancelWith(gameObject));
    }

    private IEnumerator<float> _AutoKill() {
        yield return Timing.WaitForSeconds(secondsBeforeDeath);
        GetComponent<IHealth>().SilentKill();
    }
}
