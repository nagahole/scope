#pragma warning disable 0649

using UnityEngine;
using NagaUnityUtilities;
using MEC;
using System.Collections.Generic;
using UnityEngine.Events;

public class XAtATimeSpawnInvoker : MonoBehaviour, ISpawnInvoker {
    [Header("CUSTOMISATIONS")]
    [SerializeField] int maxAtATime = 5;
    [SerializeField] MonoBehaviour[] spawnerObjects;

    private ISpawner[] spawners;
    
    private bool doSpawn;
    [SerializeField] [ReadOnly] private int existing = 0;

    private int spawnIndex;

    private void Awake() {
        spawnIndex = 0;
        spawners = new ISpawner[spawnerObjects.Length];
        for(int i = 0; i < spawnerObjects.Length; i++) {
            spawners[i] = spawnerObjects[i].GetComponent<ISpawner>();
            if(spawners[i] == null) {
                Debug.LogError($"SpawnerObjects' {spawnerObjects[i].gameObject} does not implement {typeof(ISpawner).Name}");
            }
        }
        for (int i = 0; i < spawners.Length; i++) {
            spawners[i].CreatePool(maxAtATime + 1);
        }
    }

    public void StartSpawning() {
        doSpawn = true;
        SpawnToMax();
    }

    private void SpawnToMax() {
        Timing.RunCoroutine(_SpawnToMax().CancelWith(gameObject));
    }

    private IEnumerator<float> _SpawnToMax() {
        yield return Timing.WaitForOneFrame;
        int a = maxAtATime - existing;
        for (int i = 0; i < a; i++) {
            yield return Timing.WaitForOneFrame;
            if (existing >= maxAtATime)
                break;

            var results = spawners[spawnIndex++ % spawners.Length].TryUntilSuccessfulSpawn();

            if (results.successful) {
                existing++;
                results.health.onDie.RemoveListener(OnDie);
                results.health.onDie.AddListener(OnDie);
                results.health.onSilenceDie.RemoveListener(OnDie);
                results.health.onSilenceDie.AddListener(OnDie);
            }
        }
    }

    private void OnDie() {
        existing--;
        SpawnToMax();
    }

    public void StopSpawning() {
        doSpawn = false;
    }

    private bool TrySpawn() {
        if (doSpawn) {
            
        }
        return false;
    }

    private void Spawn(Vector3 pos) {
        existing++;
        
    }

    private void OnSpawnDeath() {
        existing--;
        if (doSpawn) {
            SpawnToMax();
        }
    }

    
    
}
