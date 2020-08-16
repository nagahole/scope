using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class FadeoutTrail : TrailRendererTrail
{
    protected override IEnumerator<float> _TweenTrail(TrailRenderer trail, Vector3 end) {
        float timeAlive = 0;
        float localVelocity = velocity + (0.5f - Random.value) * velocityRandomness;
        TrailRenderer tr = trail.GetComponent<TrailRenderer>();
        float startSize = tr.startWidth;
        float endSize = tr.endWidth;
        while (timeAlive <= lifespan) {
            float timeValue = 1 - (timeAlive / lifespan);

            tr.startWidth = timeValue * startSize;
            tr.endWidth = timeValue * endSize;

            yield return Timing.WaitForOneFrame;

            trail.transform.position = Vector3.MoveTowards(trail.transform.position, end, localVelocity * Time.deltaTime);

            timeAlive += Time.deltaTime;
        }
        trail.startWidth = startSize;
        trail.endWidth = endSize;
        trail.gameObject.SetActive(false);
    }
}

/* OLD SYSTEM
 * RIP TRANSPARENCY DONT WORK :(
 * protected override IEnumerator<float> _TweenTrail(TrailRenderer trail, Vector3 end) {
        float timeAlive = 0;
        float localVelocity = velocity + (0.5f - Random.value) * velocityRandomness;
        TrailRenderer tr = trail.GetComponent<TrailRenderer>();
        Color32 scolor = tr.startColor;
        Color32 ecolor = tr.endColor;
        while (timeAlive <= lifespan) {
            Color32 tempSColor = scolor;
            Color32 tempEColor = ecolor;

            var e = (byte) (255 - ((timeAlive / lifespan) * 255));

            tempSColor.a = e;
            tempEColor.a = e;

            trail.startColor = tempSColor;
            trail.endColor = tempEColor;

            Debug.Log(trail.gameObject);

            tr.startColor = tempSColor;
            tr.endColor = tempSColor;
            yield return Timing.WaitForOneFrame;

            trail.transform.position = Vector3.MoveTowards(trail.transform.position, end, localVelocity * Time.deltaTime);

            timeAlive += Time.deltaTime;
        }
        tr.startColor = scolor;
        tr.endColor = ecolor;
        trail.gameObject.SetActive(false);
    }

*/