using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sometimes objects need a brief second on start to initialize some values, but shouldn't be seen while doing this
/// </summary>
public class DisableNearStart : MonoBehaviour
{
    [SerializeField] bool disableSlightlyAfterStart = true;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject obj;
    
    private void Awake() {
        canvas.enabled = false;
        NagaUnityUtilities.NagaUtils.ExecuteAfterOneFrame(() => {
            if (true) {
                if (disableSlightlyAfterStart) {
                    NagaUnityUtilities.NagaUtils.ExecuteAfterOneFrame(Disable);
                } else {
                    Disable();
                }
            }
        });
    }

    private void Disable() {
        canvas.enabled = true;
        obj.SetActive(false);
    }
}
