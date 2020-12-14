using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScenarioManagement;

[RequireComponent(typeof(ISpawnInvoker))]
public class SpawnOnTimerStart : OnTimerStart, ISpawnStarter
{
    protected override void OnScenarioStarted() {
        GetComponent<ISpawnInvoker>().StartSpawning();
    }
}
