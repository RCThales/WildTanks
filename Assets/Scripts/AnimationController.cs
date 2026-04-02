using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private MovementController movementController;

    private static readonly int IsRunning = Animator.StringToHash("isRunning");

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        movementController = GetComponentInParent<MovementController>();
    }

    void Update()
    {
        bool isMoving = rb.linearVelocity.sqrMagnitude > 0.1f;
        bool isDrifting = movementController != null && movementController.IsDrifting;
        animator.SetBool(IsRunning, isMoving && !isDrifting);
    }
}
