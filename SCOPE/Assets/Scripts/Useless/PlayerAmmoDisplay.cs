#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerAmmoDisplay : MonoBehaviour, IAmmoDisplayService {
    [Header("UI")]
    [SerializeField] TextMeshProUGUI weaponName;
    [SerializeField] TextMeshProUGUI main;
    [SerializeField] TextMeshProUGUI secondary;
    [Space]
    [SerializeField] float marginBetweenMainAndSecondary = 0.1f;

    [SerializeField] float snapSpeed = 20f;

    [SerializeField] Transform weaponTrackPoint;
    [SerializeField] Transform playerCamera;
    [Space]
    [SerializeField] Vector3 positionOffset;
    [SerializeField] Vector3 rotationOffset;

    private Rigidbody rigidBody;

    private void Awake() {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        SnapToLocation();
    }

    private void SnapToLocation() {
        
        var offset = playerCamera.TransformVector(positionOffset);
        var target = weaponTrackPoint.position + offset;

        //rigidBody.AddForce((target - transform.position) * snapSpeed * Time.fixedDeltaTime);
        transform.position = Vector3.Lerp(transform.position, target, 1 - Mathf.Exp(-snapSpeed * Time.fixedDeltaTime));

        transform.LookAt(playerCamera);
        transform.Rotate(rotationOffset);
    }

    public void SetAmmo(AmmoInfo info) {
        main.text = info.MainText();
        secondary.text = info.SecondaryText();

        
        secondary.rectTransform.anchoredPosition = new Vector2(main.rectTransform.anchoredPosition.x + main.preferredWidth + marginBetweenMainAndSecondary, 
            secondary.rectTransform.anchoredPosition.y);
    }

    public void SetWeaponName(string name) {
        weaponName.text = name;
    }
}
