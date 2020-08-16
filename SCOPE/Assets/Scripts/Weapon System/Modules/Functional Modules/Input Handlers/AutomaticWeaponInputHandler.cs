using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutomaticWeaponInputHandler : WeaponInputHandler
{
    public override bool FireInputReceived() {
        return Input.GetButton("Fire1");
    }
}
