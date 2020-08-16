using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAimRecoil
{
    void ApplyRecoil();
}

public abstract class AimRecoil : MonoBehaviour, IAimRecoil {
    public abstract void ApplyRecoil();
}