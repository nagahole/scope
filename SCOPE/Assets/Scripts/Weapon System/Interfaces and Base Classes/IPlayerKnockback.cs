using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerKnockback
{
    void ApplyKnockback();
}

public abstract class PlayerKnockback : MonoBehaviour, IPlayerKnockback {
    public abstract void ApplyKnockback();
}