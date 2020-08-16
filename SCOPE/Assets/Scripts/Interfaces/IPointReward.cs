#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IPointReward
{
    void RewardPoints(float points);
}

public abstract class PointReward : MonoBehaviour, IPointReward {
    public virtual void RewardPoints(float points) 
    {
        PlayerInfoHandler.sharedInstance.GetScoreSystemService().NotifyPoints(points);
    }
}

