using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NagaUnityUtilities;

public class BasicSpawner : MonoBehaviour, ISpawner {
    [SerializeField] ObjectPoolInstance objectToSpawn;
    [SerializeField] Vector3 targetScale = new Vector3(1, 1, 1);
    [SerializeField] float spaceBetweenSpawns = 1f;
    [SerializeField] int maxAttemptsInSpawning = 15;
    [SerializeField] LayerMask targetLayerMask;
    [Space]
    [SerializeField] [Tooltip("If there are multiple, they will be cycled through, in the order of their index")]
    MonoBehaviour[] positionGeneratorScripts;

    private IPositionGenerator[] positionGenerators;

    private int positionGenIndex;

    private void OnValidate() {
        if (GetComponents<ISpawner>().Length > 1)
            Debug.LogError($"{gameObject} has more than one {typeof(ISpawner).Name}");

        positionGenerators = new IPositionGenerator[positionGeneratorScripts.Length];
        for (int i = 0; i < positionGeneratorScripts.Length; i++) {
            positionGenerators[i] = positionGeneratorScripts[i].GetComponent<IPositionGenerator>();
            if (positionGenerators[i] == null) {
                Debug.LogError($"{gameObject}'s position generator object {positionGeneratorScripts[i].gameObject}" +
                    $"doesn't implement {typeof(IPositionGenerator).Name}");
            }
        }
    }

    private void Awake() {
        positionGenIndex = 0;
        positionGenerators = new IPositionGenerator[positionGeneratorScripts.Length];
        for(int i = 0; i < positionGeneratorScripts.Length; i++) {
            positionGenerators[i] = positionGeneratorScripts[i].GetComponent<IPositionGenerator>();
        }
    }

    public void CreatePool(int amountToPool) {
        GenericObjectPooler.CreatePool(objectToSpawn, amountToPool);
    }

    public (bool successful, IHealth health) TryUntilSuccessfulSpawn() {
        (bool successful, IHealth health) retVal = (false, null);
        int tries = 0;
        while (!retVal.successful && tries < maxAttemptsInSpawning) {
            tries++;
            retVal = TrySpawn();
        }
        return retVal;
    }

    private (bool successful, IHealth health) TrySpawn() {
        Vector3 pos = NextPos();
        Collider[] results = new Collider[1];
        Physics.OverlapSphereNonAlloc(pos, spaceBetweenSpawns / 2f, results, targetLayerMask);
        if (results[0] == null) { //none nearby
            return (true, Spawn(pos));
        }
        return (false, null);
    }

    private Vector3 NextPos() {
        if (positionGenIndex >= positionGenerators.Length)
            positionGenIndex = 0;
        positionGenIndex++;
        return positionGenerators[positionGenIndex - 1].GeneratePosition();
    }

    private IHealth Spawn(Vector3 pos) {
        var obj = GenericObjectPooler.RequestObject(objectToSpawn, false);
        obj.transform.localScale = targetScale;
        obj.transform.position = pos;
        obj.gameObject.SetActive(true);
        IHealth health = obj.gameObject.GetComponent<IHealth>();
        health.MaxHealth();
        Debug.DrawLine(pos, pos + Vector3.up * 10, Color.red, 1f);
        return health;
    }
}
