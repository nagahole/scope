using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StandardHitMissDeterminer : HitMissDeterminer
{
    public void DetermineHit(ShootInfo[] info) {
        for(int i = 0; i < info.Length; i++) {
            if(info[i].hitHealthComponent) {
                onHit.Invoke();
                return;
            }
        }
        onMiss.Invoke();
    }
}
