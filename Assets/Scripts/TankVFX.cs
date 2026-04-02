using UnityEngine;

public class TankVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem dustTrail;
    [SerializeField] private ParticleSystem driftSmoke;

    private MovementController movementController;
    private Rigidbody2D rb;

    void Awake()
    {
        movementController = GetComponent<MovementController>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool isMoving = rb.linearVelocity.sqrMagnitude > 0.1f;
        bool isDrifting = movementController.IsDrifting;

        SetEmission(dustTrail, isMoving && !isDrifting, 30f);
        SetEmission(driftSmoke, isMoving && isDrifting, 50f);
    }

    private void SetEmission(ParticleSystem ps, bool enabled, float rate = 100f)
    {
        var emission = ps.emission;
        emission.enabled = enabled;
        emission.rateOverTime = enabled ? rate : 0f;
    }
}
