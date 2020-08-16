using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ISpawnInvoker))]
public class SpawnOnStart : MonoBehaviour, ISpawnStarter
{
    private void Start() {
        GetComponent<ISpawnInvoker>().StartSpawning();
    }
}
