using UnityEngine;

public interface IInventoryService {
    GameObject HeldWeapon();
    void EnableWeapon();
    void DisableWeapon();
}