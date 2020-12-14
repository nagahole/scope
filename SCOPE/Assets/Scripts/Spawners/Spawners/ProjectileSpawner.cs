using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NagaUnityUtilities;

public class ProjectileSpawner : BasicSpawner {
    [Header("Projectile Spawner Customisations")]
    [SerializeField] private Vector2 magnitude;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float angle;
    [SerializeField] private int visualizationLines = 30;

    private void OnValidate() {
        magnitude.y = Mathf.Max(magnitude.x, magnitude.y);
    }

    private void OnDrawGizmos() {
        Vector3 offset = transform.position;
        Quaternion rot = Quaternion.Euler(rotation);

        for(int i = 0; i < visualizationLines; i++) {
            Vector3 outerPos = Quaternion.Euler(new Vector3(0,(i/ (float) visualizationLines) * 360,0)) * //Rotates everything around the perimeter of the circle
                (Quaternion.Euler(new Vector3(0, 0, angle)) * (Vector3.up * magnitude.y)); //Rotates a vertical straight line up on a slant to its angle
            outerPos = rot * outerPos;

            Vector3 innerPos = Quaternion.Euler(new Vector3(0, (i / (float)visualizationLines) * 360, 0)) *
                (Quaternion.Euler(new Vector3(0, 0, angle)) * (Vector3.up * magnitude.x));

            innerPos = rot * innerPos;
            Gizmos.DrawLine(offset + innerPos, offset + outerPos);
        }
    }

    private Vector3 GenerateRandomVelocityVector() {
        Vector3 retVal = Vector3.up * Random.Range(magnitude.x, magnitude.y); //base line pointing straight up vertically
        retVal = Quaternion.Euler(0,0, Random.Range(0, angle)) * retVal; //rotates it on a slant
        retVal = Quaternion.Euler(0, Random.Range(0, 360), 0) * retVal; //Rotates it randomly across the circle/disc/fov
        retVal = Quaternion.Euler(rotation) * retVal;
        Debug.DrawLine(transform.position, transform.position + retVal, Color.cyan, 5);
        return retVal;
    }

    protected override IHealth Spawn(Vector3 pos) {
        var obj = GenericObjectPooler.RequestObject(objectToSpawn, false);
        obj.transform.localScale = targetScale;
        obj.transform.position = pos;
        obj.gameObject.SetActive(true);
        IHealth health = obj.gameObject.GetComponent<IHealth>();
        health.FillToMaxHealth();
        Debug.DrawLine(pos, pos + Vector3.up * 10, Color.red, 1f);

        obj.GetComponent<Rigidbody>().velocity = GenerateRandomVelocityVector();
        return health;
    }
}
