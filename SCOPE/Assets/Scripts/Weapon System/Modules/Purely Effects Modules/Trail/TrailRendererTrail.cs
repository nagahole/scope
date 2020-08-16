#pragma warning disable 0649

using System.Collections.Generic;
using UnityEngine;
using MEC;
using NagaUnityUtilities;


[RequireComponent(typeof(IWeaponFirePos))]
public class TrailRendererTrail : Trail {
    [Header("Customisations")]
    [SerializeField] protected float velocity;
    [SerializeField] protected float lifespan;
    [SerializeField] protected float velocityRandomness; // += random
    [SerializeField] protected float startposRandomness; //Starts between (0 - value) along direction 
    [Space]
    [SerializeField] protected ObjectPoolInstance trailRenderer;
    [SerializeField] protected int trailsToPool = 100;

    private IWeaponFirePos weaponFirepos;

    private void Awake() {
        weaponFirepos = GetComponent<IWeaponFirePos>();
    }

    //Since this can be reused very easily and just generally be used for any kind of trail, might refactor this
    //into a parent/base class
    private void Start() {
        GenericObjectPooler.CreatePool(trailRenderer, trailsToPool);
    }

    public override void CreateTrail(ShootInfo shootInfo) {
        Vector3 start = weaponFirepos.GetWeaponFirePos();
        Vector3 end = shootInfo.end;

        start = NagaUtils.GetPointAlongDirection(start, end - start, Random.value * startposRandomness);

        TrailRenderer trail = GenericObjectPooler.RequestObject(trailRenderer).gameObject.GetComponent<TrailRenderer>();

        trail.gameObject.transform.position = start;
        trail.gameObject.SetActive(true);

        trail.Clear();

        Timing.RunCoroutine(_TweenTrail(trail, end));
    }

    protected virtual IEnumerator<float> _TweenTrail(TrailRenderer trail, Vector3 end) {
        float timeAlive = 0;
        float localVelocity = velocity + (0.5f - Random.value) * velocityRandomness;
        while (timeAlive <= lifespan && trail.transform.position != end) {
            yield return Timing.WaitForOneFrame;

            trail.transform.position = Vector3.MoveTowards(trail.transform.position, end, localVelocity * Time.deltaTime);

            timeAlive += Time.deltaTime;
        }
        trail.gameObject.SetActive(false);
    }
}
