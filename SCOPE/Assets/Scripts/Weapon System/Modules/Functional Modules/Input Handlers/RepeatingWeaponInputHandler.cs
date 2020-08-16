using System;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingWeaponInputHandler : WeaponInputHandler {
    public override bool FireInputReceived() {
        return true;
    }
}
