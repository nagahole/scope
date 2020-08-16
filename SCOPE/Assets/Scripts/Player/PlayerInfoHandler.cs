using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NagaUnityUtilities;

public class PlayerInfoHandler : MonoBehaviour
{
    public static PlayerInfoHandler sharedInstance { get; private set; }

    [SerializeField] Component mouseIO, playerPosition, inventory, ammoDisplay;

    private void Awake() {
        sharedInstance = this;
        mouseIOService = NagaUtils.VerifyGetComponent<IMouseIOInfoProvider>(mouseIO);
        playerPositionService = NagaUtils.VerifyGetComponent<IPositionInfoProvider>(playerPosition);
        inventoryService = NagaUtils.VerifyGetComponent<IInventoryService>(inventory);
        ammoDisplayService = NagaUtils.VerifyGetComponent<IAmmoDisplayService>(ammoDisplay);
        scoreSystemService = NagaUtils.FindObjectOfType<IScoreSystem>(); //Maybe move this (but to where?)

    }

    private IMouseIOInfoProvider mouseIOService;
    private IPositionInfoProvider playerPositionService;
    private IInventoryService inventoryService;
    private IAmmoDisplayService ammoDisplayService;
    private IScoreSystem scoreSystemService;

    public IMouseIOInfoProvider GetMouseIOService() => mouseIOService;
    public IPositionInfoProvider GetPlayerPositionService() => playerPositionService;
    public IInventoryService GetInventoryService() => inventoryService;
    public IAmmoDisplayService GetAmmoDisplayService() => ammoDisplayService;
    public IScoreSystem GetScoreSystemService() => scoreSystemService;
}
