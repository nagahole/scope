using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using NagaUnityUtilities;

public class RandomMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] Rigidbody rb;
    [Space]
    [SerializeField] Vector2 timeBetweenSwitch;
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] float horizontalDrag;
    [Space]
    [SerializeField] LayerMask wallDetectionLayerMask;
    [SerializeField] float wallDetectionRange = 2;
    [SerializeField] int maxAttempts = 4;
    [SerializeField] float turnAnglePerAttempt = 90;
    [SerializeField] float minimumRotationDifference = 90f;
    [Space]
    [SerializeField] LayerMask groundMask;
    [SerializeField] float sphereRadius;
    [SerializeField] GameObject groundCheck;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] [ReadOnly] private bool isGrounded;
    [SerializeField] private float cooldown = 0.02f;

    private Vector2 headedDirection;

    private Vector3 horizontalVelocity;
    private Vector3 verticalVelocity;

    private bool onCooldown;

    // Start is called before the first frame update
    private void Awake() {
        OnEnable();
    }
    private void OnEnable() {
        horizontalVelocity = Vector3.zero;
        verticalVelocity = Vector3.zero;
        onCooldown = true;
        Timing.RunCoroutine(_Cooldown());
        Timing.RunCoroutine(_CalculateHeadingPosition().CancelWith(gameObject));
    }

    private IEnumerator<float> _Cooldown() {
        var elapsedTime = 0f;
        rb.isKinematic = true;
        while (elapsedTime < cooldown) {
            elapsedTime += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        rb.isKinematic = false;
        onCooldown = false;
    }

    private void OnValidate() {
        if(timeBetweenSwitch.y < timeBetweenSwitch.x) {
            Debug.LogWarning($"Range not right");
        }
    }

    private Vector3 headedDirectionV3 => new Vector3(headedDirection.x, 0, headedDirection.y);

    private IEnumerator<float> _CalculateHeadingPosition() {
        headedDirection = new Vector2().RandomNormalized();
        while (true) {
            headedDirection = Quaternion.Euler(0, 0, Random.Range(minimumRotationDifference / 2f, 360 - minimumRotationDifference / 2f)) * headedDirection;
            RotateUntilClearPath();
            
            yield return Timing.WaitForSeconds(Random.Range(timeBetweenSwitch.x, timeBetweenSwitch.y));
        }
    }

    private void RotateUntilClearPath() {
        for (int i = 0; i < maxAttempts; i++) {
            if (!ObstacleAhead()) //no obstacles ahaead
                break;
            headedDirection = Quaternion.Euler(0, 0, turnAnglePerAttempt) * headedDirection;
            //headedDirection = headedDirection.RandomNormalized();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, transform.position + headedDirectionV3 * wallDetectionRange);
    }

    private bool ObstacleAhead() {
        return Physics.Raycast(transform.position, headedDirectionV3, wallDetectionRange, wallDetectionLayerMask);
    }

    // Update is called once per frame
    void Update()
    {
        if (onCooldown)
            return;
        isGrounded = (Physics.CheckSphere(groundCheck.transform.position, sphereRadius, groundMask));

        if (isGrounded) {
            verticalVelocity.y = -1;
        } else {
            verticalVelocity.y += gravity * Time.deltaTime;
        }

        if (ObstacleAhead()) {
            RotateUntilClearPath();
        }

        transform.LookAt(headedDirectionV3 + transform.position);
        horizontalVelocity *= Mathf.Pow(1/horizontalDrag, Time.deltaTime);
        horizontalVelocity += (headedDirectionV3 * acceleration * Time.deltaTime);
        if(horizontalVelocity.sqrMagnitude > maxSpeed) {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
        }

        controller.Move((horizontalVelocity + verticalVelocity) * Time.deltaTime);
    }
}
