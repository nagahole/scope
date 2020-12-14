using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NagaUnityUtilities;

public class RadialFlickPositionGenerator : MonoBehaviour, IPositionGenerator
{
    [SerializeField] private Vector2 flickRadius;
    [SerializeField] private Vector3 rotation;

    [SerializeField] private int visualizationLines;

    private void OnValidate() {
        if (flickRadius.y < flickRadius.x)
            flickRadius.y = flickRadius.x;
    }

    private void OnDrawGizmos() {
        Quaternion rot = Quaternion.Euler(rotation);
        for(int i = 0; i < visualizationLines; i++) {
            var nestedRot = Quaternion.Euler(0, 0, (360f / visualizationLines) * i);
            Vector3 v1 = new Vector3(0, flickRadius.x, 0);
            Vector3 v2 = new Vector3(0, flickRadius.y, 0);

            v1 = rot * (nestedRot * v1) + transform.position;
            v2 = rot * (nestedRot * v2) + transform.position;

            Gizmos.DrawLine(v1, v2);
        }
    }

    public Vector3 GeneratePosition() {
        return Quaternion.Euler(rotation) * (new Vector2().RandomNormalized() * Random.Range(flickRadius.x, flickRadius.y)) + transform.position;
    }

}
