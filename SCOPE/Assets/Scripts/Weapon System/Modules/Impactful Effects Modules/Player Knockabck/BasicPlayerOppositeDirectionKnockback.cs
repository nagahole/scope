#pragma warning disable 0649

using System;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerOppositeDirectionKnockback : PlayerKnockback {
    [Header("Customisations")]
    [SerializeField] float knockbackIntensity;

    public override void ApplyKnockback() {
        Vector3 direction = PlayerInfoHandler.sharedInstance.GetMouseIOService().LookDirection() * -1f;

        PlayerInfoHandler.sharedInstance.GetPlayerPositionService().AddVelocity(direction.normalized * knockbackIntensity);
    }
}

