using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnerGetter
{
    ISpawner[] GetSpawners();
    ISpawner GetNext();
    ISpawner MoveNext();
}
