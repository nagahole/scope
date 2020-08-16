using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseIOInfoProvider 
{
    Vector2 deltaMouseMovement { get; }
    Vector2 mouseMovementRate { get; }

    Vector3 LookDirection();
    Vector3 LookStartPos();
    Quaternion LookRotation();

    /// <summary>
    /// Intensity from 0-1
    /// </summary>
    /// <param name="intensity"></param>
    void AddRecoil(float intensity, float angle);

    void Lock();
    void Unlock();
}
