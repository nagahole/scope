using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loops over a lis
/// </summary>
public class CyclingSpawnerGetter : MonoBehaviour, ISpawnerGetter
{
    [SerializeField] private MonoBehaviour[] spawnerObjects;

    private ISpawner[] _spawners;
    private ISpawner[] spawners {
        get {
            if(_spawners == null) {
                spawnerIndex = 0;

                _spawners = new ISpawner[spawnerObjects.Length];
                for (int i = 0; i < spawnerObjects.Length; i++) {
                    _spawners[i] = spawnerObjects[i] as ISpawner;
                }
            }
            return _spawners;
        }
        set {
            _spawners = value;
        }
    }

    private int spawnerIndex;

    public ISpawner[] GetSpawners() {
        return spawners;
    }

    public ISpawner GetNext() {
        return spawners[spawnerIndex];
    }

    public ISpawner MoveNext() {
        ISpawner retval = spawners[spawnerIndex];
        spawnerIndex = (spawnerIndex + 1) % spawnerObjects.Length;
        return retval;
    }
}
