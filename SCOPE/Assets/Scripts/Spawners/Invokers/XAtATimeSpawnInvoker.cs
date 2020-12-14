#pragma warning disable 0649

using UnityEngine;
using NagaUnityUtilities;
using MEC;
using System.Collections.Generic;
using UnityEngine.Events;

public class XAtATimeSpawnInvoker : SpawnInvoker {
    [Header("CUSTOMISATIONS")]
    [SerializeField] private int maxAtATime = 5;
    [SerializeField] private Vector2 timeBetweenInvokes = new Vector2(0,0);
    
    [SerializeField] private bool spawnAwayFromLastKill = false;
    
    private bool doSpawn;
    [SerializeField] [ReadOnly] private int existing = 0;

    private Vector3 lastKillPosition = new Vector3(9999, 9999, 9999);

    protected override void Awake() {
        base.Awake();

        ISpawner[] spawners = spawnerGetter.GetSpawners();
        for (int i = 0; i < spawners.Length; i++) {
            spawners[i].CreatePool((maxAtATime / spawners.Length) + 1);
        }
    }

    private void OnValidate() {
        timeBetweenInvokes.y = Mathf.Max(timeBetweenInvokes.x, timeBetweenInvokes.y);
    }

    public override void StartSpawning() {
        doSpawn = true;
        SpawnToMax();
    }

    public override void StopSpawning() {
        doSpawn = false;
    }

    private void OnDie(DeathInformation e) {
        lastKillPosition = e.deathPosition;
        existing--;
        if (doSpawn) {
            if(timeBetweenInvokes.y == 0) {
                SpawnToMax();
            } else {
                Timing.RunCoroutine(_WaitThenSpawn().CancelWith(gameObject));
            }
        }
    }

    private IEnumerator<float> _WaitThenSpawn() {
        yield return Timing.WaitForSeconds(Random.Range(timeBetweenInvokes.x, timeBetweenInvokes.y));
        SpawnToMax();
    }

    private void SpawnToMax() {
        Timing.RunCoroutine(_SpawnToMax().CancelWith(gameObject));
    }

    private IEnumerator<float> _SpawnToMax() {
        int a = maxAtATime - existing;
        for (int i = 0; i < a; i++) {
            yield return Timing.WaitForOneFrame;
            if (existing >= maxAtATime || !doSpawn)
                break;

            (bool successful, IHealth health) results;

            if (spawnAwayFromLastKill) {
                results = spawnerGetter.MoveNext().TryUntilSuccessfulSpawn(lastKillPosition);
            } else {
                results = spawnerGetter.MoveNext().TryUntilSuccessfulSpawn();
            }

            if (results.successful) {
                existing++;
                results.health.onDie.RemoveListener(OnDie);
                results.health.onDie.AddListener(OnDie);
                results.health.onSilenceDie.RemoveListener(OnDie);
                results.health.onSilenceDie.AddListener(OnDie);
            }
        }
    }

    public void SetA(string identifier) {
        
    }
}
