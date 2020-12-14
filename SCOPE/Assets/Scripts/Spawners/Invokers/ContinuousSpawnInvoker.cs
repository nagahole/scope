using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class ContinuousSpawnInvoker : SpawnInvoker
{
    [Header("CUSTOMISATIONS")]
    [SerializeField] private int maxAtATime = 5;
    [SerializeField] private Vector2 timeBetwenSpawns = new Vector2(1,1);

    private CoroutineHandle spawnCoroutine;

    [SerializeField] [ReadOnly] private int existing = 0;

    protected override void Awake() {
        base.Awake();

        ISpawner[] spawners = spawnerGetter.GetSpawners();
        for (int i = 0; i < spawners.Length; i++) {
            spawners[i].CreatePool((maxAtATime / spawners.Length) + 1);
        }
    }

    private void OnValidate() {
        timeBetwenSpawns.y = Mathf.Max(timeBetwenSpawns.x, timeBetwenSpawns.y);
    }

    public override void StartSpawning() {
        if (spawnCoroutine != null) {
            spawnCoroutine = Timing.RunCoroutine(_SpawnLoop().CancelWith(gameObject));
        }
    }

    public override void StopSpawning() {
        if (spawnCoroutine != null) {
            Timing.KillCoroutines(spawnCoroutine);
        }
    }

    private void OnDie(DeathInformation e) {
        existing--;
    }

    private IEnumerator<float> _SpawnLoop() {
        while (true) {
            if(existing < maxAtATime) {
                var results = spawnerGetter.MoveNext().TryUntilSuccessfulSpawn();

                if (results.successful) {
                    existing++;
                    results.health.onDie.RemoveListener(OnDie);
                    results.health.onDie.AddListener(OnDie);
                    results.health.onSilenceDie.RemoveListener(OnDie);
                    results.health.onSilenceDie.AddListener(OnDie);
                }
            }


            yield return Timing.WaitForSeconds(Random.Range(timeBetwenSpawns.x, timeBetwenSpawns.y));
        }
    }
}
