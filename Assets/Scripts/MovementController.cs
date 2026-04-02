using UnityEngine;

public class MovementController : MonoBehaviour
{

    // ── Movement ─────────────────────────────────────────────────────────────
    [Header("Movement")]
    [Tooltip("Top speed when driving normally.")]
    [SerializeField] private float speed = 5f;
    [Tooltip("Force applied per FixedUpdate while accelerating.")]
    [SerializeField] private float acceleration = 20f;

    // ── Grip & Steering ───────────────────────────────────────────────────────
    [Header("Grip & Steering")]
    [Tooltip("How much lateral velocity is kept each frame normally. Lower = tighter cornering.")]
    [Range(0f, 1f)]
    [SerializeField] private float gripFactor = 0.5f;
    [Tooltip("How much lateral velocity is kept each frame while drifting. Higher = more slide.")]
    [Range(0f, 1f)]
    [SerializeField] private float driftFactor = 0.98f;
    [Tooltip("How fast grip recovers after releasing drift. Lower = longer recovery.")]
    [Range(0.5f, 10f)]
    [SerializeField] private float gripRecoverySpeed = 3f;

    // ── Drift ─────────────────────────────────────────────────────────────────
    [Header("Drift")]
    [Tooltip("Top speed while drifting, as a fraction of normal speed.")]
    [Range(0.3f, 1f)]
    [SerializeField] private float driftSpeedMultiplier = 0.7f;
    [Tooltip("How sharply speed bleeds down to the drift cap. 1 = gradual, 4 = near-instant.")]
    [Range(1, 4)]
    [SerializeField] private int driftBrakeLevel = 2;

    // ── Visual ────────────────────────────────────────────────────────────────
    [Header("Visual")]
    [Tooltip("Child sprite object to tilt during drift (must NOT be the physics root).")]
    [SerializeField] private Transform bodyTransform;
    [Tooltip("Max rotation angle of the body sprite when sliding sideways.")]
    [Range(0f, 90f)]
    [SerializeField] private float driftTiltAngle = 15f;

    // ── Runtime state (not shown in Inspector) ────────────────────────────────
    private Rigidbody2D rb;
    private bool isDrifting = false;
    private float currentGrip;
    private float currentTilt = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentGrip = gripFactor;
    }

    public bool IsDrifting => isDrifting;

    public void SetDrift(bool drifting)
    {
        isDrifting = drifting;
    }

    public void Move(Vector2 input)
    {
        Rotate(input.x);
        ApplyLateralGrip();
        ApplyThrust(input.y);
        UpdateBodyTilt();
    }

    private void Rotate(float horizontalInput)
    {
        float rotation = -horizontalInput * 150f * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotation);
    }

    private void ApplyLateralGrip()
    {
        float targetGrip = isDrifting ? driftFactor : gripFactor;
        float lerpSpeed = isDrifting ? 20f : gripRecoverySpeed;
        currentGrip = Mathf.Lerp(currentGrip, targetGrip, lerpSpeed * Time.fixedDeltaTime);

        Vector2 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        localVelocity.x *= currentGrip;
        rb.linearVelocity = transform.TransformDirection(localVelocity);
    }

    private void ApplyThrust(float forwardInput)
    {
        if (forwardInput != 0)
        {
            rb.AddForce(transform.up * (-forwardInput * acceleration), ForceMode2D.Force);
            ClampSpeed();
        }
        else if (!isDrifting)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.2f);
        }
    }

    private void ClampSpeed()
    {
        float maxSpeed = isDrifting ? speed * driftSpeedMultiplier : speed;
        float currentSpeed = rb.linearVelocity.magnitude;
        if (currentSpeed <= maxSpeed) return;

        float brakeRate = isDrifting ? driftBrakeLevel * 2f * Time.fixedDeltaTime : 1f;
        rb.linearVelocity = rb.linearVelocity.normalized * Mathf.Lerp(currentSpeed, maxSpeed, brakeRate);
    }

    private void UpdateBodyTilt()
    {
        if (bodyTransform == null) return;

        Vector2 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        float targetTilt = -(localVelocity.x / speed) * driftTiltAngle;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, 10f * Time.fixedDeltaTime);
        bodyTransform.localRotation = Quaternion.Euler(0f, 0f, currentTilt);
    }

    public void MoveTowards(Vector2 worldDirection)
    {
        rb.AddForce(worldDirection * acceleration, ForceMode2D.Force);
        if (rb.linearVelocity.magnitude > speed)
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
    }

    public void RotateTowardsDirection(Vector2 direction, float offset = 0)
    {
        if (direction.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + offset;
            rb.MoveRotation(angle);
        }
    }
}
