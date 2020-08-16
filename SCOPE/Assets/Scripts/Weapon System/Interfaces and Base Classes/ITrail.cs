using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrail
{
    void CreateTrails(ShootInfo[] shootInfo);

    void CreateTrail(ShootInfo shootInfo);
}

public abstract class Trail : MonoBehaviour, ITrail {
    public abstract void CreateTrail(ShootInfo shootInfo);

    public void CreateTrails(ShootInfo[] shootInfo) {
        for (int i = 0; i < shootInfo.Length; i++) {
            CreateTrail(shootInfo[i]);
        }
    }
}