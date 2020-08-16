using UnityEngine;

public class SemiautoWeaponInputHandler : WeaponInputHandler {
    public override bool FireInputReceived() {
        return Input.GetButtonDown("Fire1");
    }
}