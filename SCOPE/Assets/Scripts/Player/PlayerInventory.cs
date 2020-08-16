#pragma warning disable 0649
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IInventoryService
{
    [Header("CUSTOMISATIONS")]
    [SerializeField] private GameObject currentWeapon;

    private bool weaponEnabled;

    public GameObject HeldWeapon() {
        if(weaponEnabled == false) {
            return null;
        }
        return currentWeapon;
    }

    public void EnableWeapon() {
        weaponEnabled = true;
    }

    public void DisableWeapon() {
        weaponEnabled = false;
    }
}
