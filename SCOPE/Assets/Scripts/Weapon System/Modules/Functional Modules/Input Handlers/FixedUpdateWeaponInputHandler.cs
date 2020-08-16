using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class FixedUpdateWeaponInputHandler : WeaponInputHandler
{
    private bool mouseDown = false;

    protected override void Update() {
        
    }


    protected virtual void FixedUpdate() {
        if (Input.GetButtonDown("Fire1")) {
            mouseDown = true;
        } else if (Input.GetButtonUp("Fire1")) {
            mouseDown = false;
        }
        base.Update();
    }

    public override bool FireInputReceived() {
        return mouseDown;
    }
}
