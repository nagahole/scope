using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner
{
    (bool successful, IHealth health) TryUntilSuccessfulSpawn();
    void CreatePool(int amountToPool);
}
