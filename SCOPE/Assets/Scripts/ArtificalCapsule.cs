using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificalCapsule : MonoBehaviour
{
    [SerializeField] Transform topSphere, cylinder, bottomSphere;
    [Space]
    [SerializeField] private float radius;
    [SerializeField] private float height;

    private void OnValidate() {
        cylinder.localScale = new Vector3(radius * 2f, (height / 2f) -  radius, radius * 2f);
        topSphere.localScale = Vector3.one * radius * 2f;
        bottomSphere.localScale = Vector3.one * radius * 2f;

        topSphere.localPosition = new Vector3(0, (height / 2f) - radius, 0);
        bottomSphere.localPosition = new Vector3(0, -(height / 2f) + radius, 0);
    }
}
