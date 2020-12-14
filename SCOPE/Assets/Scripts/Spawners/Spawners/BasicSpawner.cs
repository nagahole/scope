using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NagaUnityUtilities;

public class BasicSpawner : MonoBehaviour, ISpawner {
    [SerializeField] protected ObjectPoolInstance objectToSpawn;
    [SerializeField] protected Vector3 targetScale = new Vector3(1, 1, 1);
    [SerializeField] protected float spaceBetweenSpawns = 1f;
    [SerializeField] protected int maxAttemptsInSpawning = 15;
    [SerializeField] protected LayerMask targetLayerMask;
    [Space]
    [SerializeField] [Tooltip("If there are multiple, they will be cycled through, in the order of their index")]
    protected MonoBehaviour[] positionGeneratorScripts;

    protected IPositionGenerator[] positionGenerators;

    private int positionGenIndex;

    private void Awake() {
        positionGenIndex = 0;
        positionGenerators = new IPositionGenerator[positionGeneratorScripts.Length];
        for(int i = 0; i < positionGeneratorScripts.Length; i++) {
            positionGenerators[i] = positionGeneratorScripts[i] as IPositionGenerator;
        }
    }

    public void CreatePool(int amountToPool) {
        GenericObjectPooler.CreatePool(objectToSpawn, amountToPool);
    }

    public (bool successful, IHealth health) TryUntilSuccessfulSpawn(params Vector3[] blacklistedPositions) {
        (bool successful, IHealth health) retVal = (false, null);
        int tries = 0;
        while (!retVal.successful && tries < maxAttemptsInSpawning) {
            tries++;
            retVal = TrySpawn(blacklistedPositions);
        }
        return retVal;
    }

    private (bool successful, IHealth health) TrySpawn(params Vector3[] blacklistedPositions) {
        Vector3 newPosition = NextPos();
        Collider[] results = new Collider[1];
        Physics.OverlapSphereNonAlloc(newPosition, spaceBetweenSpawns / 2f, results, targetLayerMask);
        if (results[0] == null) { //none nearby

            for(int i = 0; i < blacklistedPositions.Length; i++) {
                float distance = (blacklistedPositions[i] - newPosition).sqrMagnitude;

                if (distance <= spaceBetweenSpawns) {
                    return (false, null);
                }
            }

            return (true, Spawn(newPosition));
        }
        return (false, null);
    }

    protected Vector3 NextPos() {
        if (positionGenIndex >= positionGenerators.Length)
            positionGenIndex = 0;
        positionGenIndex++;
        return positionGenerators[positionGenIndex - 1].GeneratePosition();
    }

    protected virtual IHealth Spawn(Vector3 pos) {
        var obj = GenericObjectPooler.RequestObject(objectToSpawn, false);
        obj.transform.localScale = targetScale;
        obj.transform.position = pos;
        obj.gameObject.SetActive(true);
        IHealth health = obj.gameObject.GetComponent<IHealth>();
        health.FillToMaxHealth();
        Debug.DrawLine(pos, pos + Vector3.up * 10, Color.red, 1f);
        return health;
    }
}
