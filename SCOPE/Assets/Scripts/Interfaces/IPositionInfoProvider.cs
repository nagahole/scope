using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPositionInfoProvider 
{
    Vector3 Position();

    void AddVelocity(Vector3 vel);
}

