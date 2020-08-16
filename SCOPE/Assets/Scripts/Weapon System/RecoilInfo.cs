using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RecoilInfo {
    public float intensity;
    public Vector3 direction;
    public float recoveryRate;

    public RecoilInfo(float intensity, Vector3 direction, float recoveryRate){
        this.intensity = intensity;
        this.direction = direction;
        this.recoveryRate = recoveryRate;
    }
}

