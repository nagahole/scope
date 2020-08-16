using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUserCommunicator : UserCommunicator
{
    private static PlayerInfoHandler player => PlayerInfoHandler.sharedInstance;

    public override void AddRecoil(float intensity, float angle) {
        player.GetMouseIOService().AddRecoil(intensity, angle);
    }

    public override void AddVelocity(Vector3 vel) {
        player.GetPlayerPositionService().AddVelocity(vel);
    }

    public override Vector3 LookDirection() {
        return player.GetMouseIOService().LookDirection();
    }

    public override Quaternion LookRotation() {
        return player.GetMouseIOService().LookRotation();
    }

    public override Vector3 LookStartPos() {
        return player.GetMouseIOService().LookStartPos();
    }

    public override GameObject HeldWeapon() {
        return player.GetInventoryService().HeldWeapon();
    }

    public override void SetAmmoInfo(AmmoInfo info) {
        player.GetAmmoDisplayService().SetAmmo(info);
    }
}
