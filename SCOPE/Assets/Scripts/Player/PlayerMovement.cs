#pragma warning disable 0649

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using MEC;
using System.Linq;
using NagaUnityUtilities;

public class PlayerMovement : MonoBehaviour, IPositionInfoProvider
{
    [System.Serializable]
    private class MovementModeAccelerationDictionary : SerializableDictionaryBase<MovementMode, float> { }

    private enum BunnyhopAccuracyMode : byte {
        sync,
        successful
    }

    private enum MovementMode : byte { //might make public in the future
        walking,
        running,
        airStrafe,
        crouching,
    }

    private enum MovementSwitchMode : byte {
        toggle,
        keyheldWalk,
        keyheldRun
    }

    private enum CrouchSwitchMode : byte {
        toggle,
        keyheld
    }

    [SerializeField] private CharacterController controller;

    [Header("Horizontal Movement")]
    [SerializeField] private MovementModeAccelerationDictionary movementModeAccelerationDictionary = new MovementModeAccelerationDictionary {
        { MovementMode.walking, 10f },
        { MovementMode.running, 20f },
        { MovementMode.airStrafe, 2f },
        { MovementMode.crouching, 4f }
    };
    [Space]
    [SerializeField] private float horizontalDrag = 5f;
    [SerializeField] private float horizontalAirDrag = 10f;

    [Header("Running Controls")]
    [SerializeField] private KeyCode movementSwitchKey = KeyCode.LeftShift;
    [SerializeField] private MovementSwitchMode movementSwitchMode;
    [SerializeField] private bool frozen;
    private bool toggleRun;

    [Header("Crouching Controls")]
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private CrouchSwitchMode crouchSwitchMode = CrouchSwitchMode.keyheld;
    [SerializeField] private float crouchScale = 0.6f;

    [Header("Vertical Movement")]
    [SerializeField] private bool allowAutoJump = true;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;

    [Header("FOV")]
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private float runnningFOV, normalFOV, fovChangeRate;

    [Header("Bunny Hopping")]
    [SerializeField] private bool allowBunnyhopping = true;
    [SerializeField] private float bunnyhopSpeedBonus = 1.2f;
    [SerializeField] private float maxRotationDifferenceInDegrees = 30f; //How smooth you have to be for a strafe to count. The greater the more leniant
    [SerializeField] private float requiredStrafeSpeedInDegrees = 120; //How fast you have to rotate/strafe in degs/sec for you to gain a speed boost
    [SerializeField] private float failedBunnyhopSpeedReductionPenalty = 4f;
    [Space]
    [SerializeField] private int pastBunnyhopsToAnalyze = 1000;
    [SerializeField] BunnyhopAccuracyMode accuracyMode = BunnyhopAccuracyMode.successful;
    
    private int successfulBunnyhops = 0, unsuccessfulBunnyhops = 0;
    private Queue<bool> successfulBunnyhopHistory = new Queue<bool>();
    

    [Header("Ground Checking")]
    [SerializeField] private Transform groundCheck;
    [Tooltip("Size of ground checking collider")]
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private bool isGrounded;
    private MovementMode movementMode = MovementMode.walking;

    private Vector3 horizontalVelocity;
    private Vector3 verticalVelocity;

    private Vector3 lastPos;
    private Vector3 deltaPos {
        get {
            return transform.position - lastPos;
        }
    }

    private IMouseIOInfoProvider mouseLookScript;

    public float horizontalSpeed {
        get {
            return horizontalVelocity.magnitude;
        }
    }

    public float verticalSpeed {
        get {
            return verticalVelocity.magnitude;
        }
    }

    public float bunnyhopAccuracy {
        get {
            return ((float) successfulBunnyhops / (successfulBunnyhops + unsuccessfulBunnyhops));
        }
    }

    /*
     * 
     *  FIELDS AND PROPERTIES END
     * 
     */

    private void Awake() {
        mouseLookScript = NagaUtils.FindObjectOfType<IMouseIOInfoProvider>();
    }

    private void Start() {
        Timing.RunCoroutine(_GetLastPos().CancelWith(gameObject));
    }

    private void Update()
    {
        if (Time.time == 0)
            return;
        CheckInput();
        if (!frozen) {
            HandleHorizontalMovement();
            HandleVerticalMovement();
            //AdjustFOV();
        }
        ApplyHorizontalDrag();
        ApplyGravity();
    }

    private void FixedUpdate() {
        
        
    }

    private void CheckInput() {
        CheckGrounded();
        CheckCrouchMode();
        CheckMovementMode();
        
    }

    private IEnumerator<float> _GetLastPos() {
        while (true) {
            Vector3 temp = transform.position;
            yield return Timing.WaitForOneFrame;
            lastPos = temp;
        }
    }

    private void CheckGrounded() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //reduces vertical velocity when grounded
        if (isGrounded && verticalVelocity.y < 0) {
            verticalVelocity.y = -2f;
        }
    }

    private void CheckCrouchMode() {
        if(crouchSwitchMode == CrouchSwitchMode.toggle) {
            if (Input.GetKeyDown(crouchKey)) {
                ToggleCrouch();
            }
        } else {
            if (Input.GetKey(crouchKey)) {
                AttemptToCrouch();
            } else {
                AttemptToUncrouch();
            }
        }
    }

    private void ToggleCrouch() {
        if (movementMode == MovementMode.crouching) {
            AttemptToUncrouch();
        } else {
            AttemptToCrouch();
        }
    }

    private void CheckMovementMode() {
        if (movementMode == MovementMode.crouching)
            return;

        if (movementSwitchMode == MovementSwitchMode.toggle) {
            if (Input.GetKeyDown(movementSwitchKey)) {
                toggleRun = !(movementMode == MovementMode.running);
                SetRun(toggleRun);
            }
        } else {
            if (Input.GetKey(movementSwitchKey)) {
                SetRun(movementSwitchMode == MovementSwitchMode.keyheldRun);
            } else {
                SetRun(movementSwitchMode == MovementSwitchMode.keyheldWalk);
            }
        }
    }

    private void SetRun(bool b) {
        movementMode = b ? MovementMode.running : MovementMode.walking;
    }

    private void ApplyHorizontalDrag() {
        ScaleHorizontalVelocity(1 / (isGrounded? horizontalDrag : horizontalAirDrag));
    }

    private void HandleHorizontalMovement() {
        if (isGrounded) {
            CalculateGroundHorizontalVelocity();
        } else {
            CalculateAirHorizontalVelocity();
        }
        controller.Move(horizontalVelocity * Time.deltaTime);
    }

    private void CalculateGroundHorizontalVelocity() {
        float acceleration = movementModeAccelerationDictionary[movementMode];// movementMode ? runAcceleration : walkAcceleration;

        horizontalVelocity += GetMovementVector() * acceleration * Time.deltaTime;
    }

    private void CalculateAirHorizontalVelocity() {
        Vector3 move = GetMovementVector();

        if(move == transform.forward) {
            ScaleHorizontalVelocity(1 / failedBunnyhopSpeedReductionPenalty);
        }

        horizontalVelocity += move * movementModeAccelerationDictionary[MovementMode.airStrafe] * Time.deltaTime;

        if (allowBunnyhopping) {
            HandleBunnyHopping(move);
        }
    }

    private void AdjustFOV() {
        var fov = fpsCamera.fieldOfView;
        if (movementMode == MovementMode.running) {
            fpsCamera.fieldOfView = Mathf.Lerp(fov, runnningFOV, 1 - Mathf.Exp(-fovChangeRate * Time.deltaTime));
        } else {
            fpsCamera.fieldOfView = Mathf.Lerp(fov, normalFOV, 1 - Mathf.Exp(-fovChangeRate * Time.deltaTime));
        }
    }

    /*
     *  HELPER FUNCTINONS FOR CALCULATEAIRHORIZONTALVELOCITYS
     */
    #region
    private void HandleBunnyHopping(Vector3 move) {
        bool isSynced = false, isSuccessful = false;

        if (DirectionWithinTolerance()) {
            if (MouseSyncedWithStrafe(move, GetMouseMovementRate())) {
                horizontalVelocity = Vector3.RotateTowards(horizontalVelocity, transform.forward, 5, 0); // Guides velocity towards the front of the player
                isSynced = true;

                if (MouseStrafeFastEnough(GetMouseMovementRate())) { //gives bonus speed for hitting strafes that are fast enough
                    ScaleHorizontalVelocity(bunnyhopSpeedBonus);
                    isSuccessful = true;
                }
            }
        } else {
            ScaleHorizontalVelocity(1 / failedBunnyhopSpeedReductionPenalty);
        }
        CalculateBunnyhopAccuracy(isSuccessful || (isSynced && accuracyMode == BunnyhopAccuracyMode.sync));
    }

    private Vector2 GetMouseMovementRate() {
        return mouseLookScript.mouseMovementRate;
    }

    private bool DirectionWithinTolerance() {
        float angleDifference = Vector3.Angle(horizontalVelocity, transform.forward);
        return angleDifference < maxRotationDifferenceInDegrees;
    }

    private bool MouseSyncedWithStrafe(Vector3 move, Vector2 mouseMovementRate) {
        return ((move == -transform.right && mouseMovementRate.x < 0) || (move == transform.right && mouseMovementRate.x > 0));
    }

    private bool MouseStrafeFastEnough(Vector2 mouseMovementRate) {
        return Mathf.Abs(mouseMovementRate.x) > requiredStrafeSpeedInDegrees;
    }

    #endregion

    /// <summary>
    /// Movement vector based on WASD inputs
    /// </summary>
    /// <returns> Returns an absolute Vector3 in world coordinates </returns>
    private Vector3 GetMovementVector() {
        float x = Input.GetAxisRaw("Horizontal"); //sideways
        float z = Input.GetAxisRaw("Vertical"); //forward/backwards

        Vector3 move = (transform.right * x) + (transform.forward * z);

        return move.normalized;
    }

    private void HandleVerticalMovement() {
        if (IsJumping() && isGrounded) { 
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        controller.Move(verticalVelocity * Time.deltaTime); //need to * by Time.deltaTime again becuase ΔDISPLACEMENT = VELOCITY * TIME
    }

    private void ApplyGravity() {
        verticalVelocity.y += gravity * Time.deltaTime; //applys gravity
    }

    private bool IsJumping() {
        return (allowAutoJump ? Input.GetButton("Jump") : Input.GetButtonDown("Jump"));
    }

    /// <summary>
    /// Accounts for framerate
    /// </summary>
    /// <param name="factorPerSecond"></param>
    private void ScaleHorizontalVelocity(float factorPerSecond) {
        horizontalVelocity *= Mathf.Pow(factorPerSecond, Time.deltaTime);
    }

    private void AttemptToUncrouch() {
        if (movementMode == MovementMode.crouching) {
            Vector3 scale = transform.localScale;
            float newYScale = scale.y / crouchScale;
            transform.localScale = new Vector3(scale.x, newYScale, scale.z);
            SetRun(toggleRun);

            controller.Move(new Vector3(0, (newYScale - (crouchScale * newYScale)), 0));

            CheckCharacterControllerScalingError();
        }
    }

    private void AttemptToCrouch() {
        if (movementMode != MovementMode.crouching) {
            Vector3 scale = transform.localScale;
            transform.localScale = new Vector3(scale.x, scale.y * crouchScale, scale.z);
            
            movementMode = MovementMode.crouching;

            NagaUtils.ExecuteAfterOneFrame(()=> {
                controller.Move(new Vector3(0, ((crouchScale * scale.y) - scale.y), 0));
            });

            CheckCharacterControllerScalingError();
        }
    }

    private void CheckCharacterControllerScalingError() {
        float heightToWidthRatio = transform.localScale.y / transform.localScale.x;
        if (heightToWidthRatio < 0.5f) {
            controller.radius = transform.localScale.y;
            controller.height = heightToWidthRatio;
        } else {
            controller.radius = heightToWidthRatio / 2;
            controller.height = 2;
        }
    }

    private void CalculateBunnyhopAccuracy(bool b) {
        successfulBunnyhopHistory.Enqueue(b);

        if (b) {
            successfulBunnyhops++;
        } else {
            unsuccessfulBunnyhops++;
        }

        if (successfulBunnyhopHistory.Count > pastBunnyhopsToAnalyze) {
            if (successfulBunnyhopHistory.Dequeue()) { //Removed bunnyhop history was successful
                successfulBunnyhops--;
            } else { //Unsuccessful
                unsuccessfulBunnyhops--;
            }
        }
    }

    public Vector3 Position() {
        return transform.position;
    }

    public void AddVelocity(Vector3 vel) {
        Debug.Log(vel);
        horizontalVelocity += new Vector3(vel.x, 0, vel.z);
        verticalVelocity += new Vector3(0, vel.y, 0);
    }
}