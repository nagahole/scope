using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NagaUnityUtilities;

public class CuboidPositionGenerator : MonoBehaviour, IPositionGenerator
{
    [SerializeField] Vector3 center;
    [SerializeField] Vector3 size;
    [SerializeField] Vector3 rotation;
    [SerializeField] bool relativeToTransform = false;
    

    public Vector3 GeneratePosition() {
        return Quaternion.Euler(rotation) * new Vector3((Random.value - 0.5f) * size.x,
            (Random.value - 0.5f) * size.y,
            (Random.value - 0.5f) * size.z) + (relativeToTransform? transform.position : new Vector3(0,0,0)) + center;
    }

    private void OnDrawGizmos() {
        NagaUtils.DrawCuboid(center + (relativeToTransform? transform.position : new Vector3(0,0,0)) , size, Quaternion.Euler(rotation));
    }

}
