#pragma warning disable 0649

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

[RequireComponent(typeof(Camera))]
public class MouseLook : MonoBehaviour, IMouseIOInfoProvider
{
    [Header("Dependencies")]
    [SerializeField] private Transform playerBody;

    [Header("Settings")]
    [SerializeField] private Vector2 recoilRecoveryRate;
    [SerializeField] private Vector2 recoilIntensityScale;
    [SerializeField] private Vector2 recoilRotationMax;

    private float mouseX, mouseY; //delta mouse movement
    private Vector2 rotation = new Vector2(0,0);
    private Vector2 recoilRotation = new Vector2(0,0);

    private Camera povCamera;

    private Vector2 startingRotation;

    private bool rotationLocked = false;

    public Vector2 deltaMouseMovement {
        get { return new Vector2(mouseX, mouseY); }
    }

    public Vector2 mouseMovementRate {
        get { return deltaMouseMovement / Time.deltaTime; }
    }

    private void Awake() {
        povCamera = GetComponent<Camera>();
        startingRotation.x = transform.localEulerAngles.x;
        startingRotation.y = playerBody.localEulerAngles.y;
        Timing.RunCoroutine(_LockToStartRotation(0.2f)); //To prevent jumping of camera
    }

    private void Start() {
        povCamera.fieldOfView = Settings.activeSettings.fov;
    }

    private void Update() {
        if (!rotationLocked) {
            GetMouseMovement();
            CalculateRotations();
        }
    }

    private void LateUpdate() {
        //ResetRecoil();
        ApplyRotations();
    }

    private IEnumerator<float> _LockToStartRotation(float time) {
        float timeElapsed = 0;
        while(timeElapsed < time) {
            rotation = startingRotation;
            timeElapsed += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
    }

    public void Lock() {
        rotationLocked = true;
    }

    public void Unlock() {
        rotationLocked = false;
    }

    private void ResetRecoil() {
        recoilRotation.x = Mathf.Lerp(recoilRotation.x, 0, 1 - Mathf.Exp(-recoilRecoveryRate.x * Time.deltaTime));
        recoilRotation.y = Mathf.Lerp(recoilRotation.y, 0, 1 - Mathf.Exp(-recoilRecoveryRate.y * Time.deltaTime));
    }

    private void GetMouseMovement() {
        if (Settings.activeSettings.rawInput) {
            mouseX = Input.GetAxisRaw("Mouse X") * Settings.activeSettings.mouseSensitivity * Settings.activeSettings.mouseSensitivityAxis.x;
            mouseY = Input.GetAxisRaw("Mouse Y") * Settings.activeSettings.mouseSensitivity * Settings.activeSettings.mouseSensitivityAxis.y;
        } else {
            mouseX = Input.GetAxis("Mouse X") * Settings.activeSettings.mouseSensitivity * Settings.activeSettings.mouseSensitivityAxis.x;
            mouseY = Input.GetAxis("Mouse Y") * Settings.activeSettings.mouseSensitivity * Settings.activeSettings.mouseSensitivityAxis.y;
        }
        
    }

    private void CalculateRotations() {
        rotation.x -= mouseY;
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);

        rotation.y += mouseX;
    }

    private void ApplyRotations() {
        //VERTICAL AXIS
        transform.localRotation = Quaternion.Euler(rotation.x, 0f, 0f); //recoil and rotation has different x and y axis 
        //transform.Rotate(-Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime, 0, 0);
        // - recoilRotation.x

        /*
         *  Rotation X = vertical Y = horizontal
         *  
         * Recoil rotation X = horizontal Y = vertical
         * 
         *  might change this later
         */

        //HORIZONTAL AXIS
        //playerBody.Rotate(0, Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime, 0);
        playerBody.localRotation = Quaternion.Euler(playerBody.localRotation.x, rotation.y, playerBody.localRotation.z);
        //+recoilRotation.y;

        //NOTE RECOIL STUFF REMOVED
    }

    public Vector3 LookDirection() {
        return transform.forward;
    }

    public Vector3 LookStartPos() {
        return transform.position;
    }

    public Quaternion LookRotation() {
        return transform.rotation;
    }

    public void AddRecoil(float intensity, float angle) {
        Vector2 recoilVector = (Quaternion.Euler(new Vector3(0,0,angle)) * Vector3.right) * intensity;

        recoilRotation += recoilVector * recoilIntensityScale;

        recoilRotation.x = Mathf.Clamp(recoilRotation.x, -recoilRotationMax.x, recoilRotationMax.x);
        recoilRotation.y = Mathf.Clamp(recoilRotation.y, -recoilRotationMax.y, recoilRotationMax.y);
    }
}
