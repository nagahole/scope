using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner
{
    (bool successful, IHealth health) TryUntilSuccessfulSpawn(params Vector3[] blacklistedPositions);
    void CreatePool(int amountToPool);
}
