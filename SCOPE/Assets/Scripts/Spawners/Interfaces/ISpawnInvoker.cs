using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnInvoker
{
    void StartSpawning();
    void StopSpawning();
}

public abstract class SpawnInvoker : MonoBehaviour, ISpawnInvoker {
    [SerializeField] protected MonoBehaviour spawnerGetterObject;
    protected ISpawnerGetter spawnerGetter;

    protected virtual void Awake() {
        spawnerGetter = spawnerGetterObject as ISpawnerGetter;
    }

    public abstract void StartSpawning();
    public abstract void StopSpawning();
}