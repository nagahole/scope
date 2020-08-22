using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

[RequireComponent(typeof(IHealth))]
public class GrowAndShrink : MonoBehaviour
{
    [SerializeField] private bool matchGrowAndShrink;
    [SerializeField] private float growDuration;
    [SerializeField] private float shrinkDuration;

    private IHealth health;

    private void OnValidate() {
        if (matchGrowAndShrink) {
            shrinkDuration = growDuration;
        }
    }

    private void Awake() {
        health = GetComponent<IHealth>();
    }

    private void OnEnable() {
        Timing.RunCoroutine(_GrowAndShrink(transform.localScale).CancelWith(gameObject));
        transform.localScale = Vector3.zero;
    }
    
    private IEnumerator<float> _GrowAndShrink(Vector3 targetScale) {
        float elapsedTime = 0;
        while(elapsedTime < growDuration) {
            yield return Timing.WaitForOneFrame;
            elapsedTime += Time.deltaTime;
            transform.localScale = targetScale * (elapsedTime / growDuration);
            
        }
        elapsedTime = 0;
        while(elapsedTime < shrinkDuration) {
            yield return Timing.WaitForOneFrame;
            elapsedTime += Time.deltaTime;
            transform.localScale = targetScale * (1 - (elapsedTime / shrinkDuration));
        }
        health.SilentKill();
    }
}
