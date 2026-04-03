using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private MovementController movementController;
    private bool movementEnabled = true;
    private bool shootingEnabled = true;

    private PlayerInputActions inputActions;
    private Barrel barrel;
    private Health health;
    private PlayerUpgradeManager upgradeManager;

    [SerializeField] private float shootCooldown = 0.2f;
    private float lastShoot1Time = 0f;
    private float lastShoot2Time = 0f;

    private void OnEnable()
    {
        inputActions.Gameplay.Enable();
        inputActions.Gameplay.Move.performed += OnInputPerformed;
        inputActions.Gameplay.AimController.performed += OnInputPerformed;
        inputActions.Gameplay.AimMouse.performed += OnInputPerformed;
        inputActions.Gameplay.Shoot.performed += OnInputPerformed;
        inputActions.Gameplay.Shoot2.performed += OnInputPerformed;
        inputActions.Gameplay.Drift.performed += OnInputPerformed;
    }

    private void OnDisable()
    {
        inputActions.Gameplay.Move.performed -= OnInputPerformed;
        inputActions.Gameplay.AimController.performed -= OnInputPerformed;
        inputActions.Gameplay.AimMouse.performed -= OnInputPerformed;
        inputActions.Gameplay.Shoot.performed -= OnInputPerformed;
        inputActions.Gameplay.Shoot2.performed -= OnInputPerformed;
        inputActions.Gameplay.Drift.performed -= OnInputPerformed;
        inputActions.Disable();
    }

    void Awake()
    {
        movementController = GetComponent<MovementController>();
        inputActions = new PlayerInputActions();
        barrel = GetComponentInChildren<Barrel>();
        health = GetComponent<Health>();
        upgradeManager = GetComponent<PlayerUpgradeManager>();
    }

    void Start()
    {
        InputManager.Instance.SetUsingController(Gamepad.current != null);
    }

    void Update()
    {
        HandleRotation();
        HandleShoot();
        HandleDrift();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    public void SetMovementEnabled(bool enabled)
    {
        movementEnabled = enabled;
        if (!enabled)
            movementController.Stop();
    }

    public void SetShootingEnabled(bool enabled)
    {
        shootingEnabled = enabled;
    }

    private void HandleMovement()
    {
        if (!movementEnabled)
        {
            movementController.Stop();
            return;
        }
        Vector2 input = inputActions.Gameplay.Move.ReadValue<Vector2>();
        movementController.Move(input);
    }

    private void HandleDrift()
    {
        movementController.SetDrift(movementEnabled && inputActions.Gameplay.Drift.IsPressed());
    }

    private void OnInputPerformed(InputAction.CallbackContext ctx)
    {
        InputManager.Instance.SetUsingController(ctx.control.device is Gamepad);
    }

    private void HandleRotation()
    {
        Vector2 direction = InputManager.Instance.UsingController ?
            GetControllerAimDirection() : GetMouseAimDirection();
        barrel.RotateTowardsDirection(direction);
    }

    private Vector2 GetControllerAimDirection()
    {
        if (Gamepad.current == null) return Vector2.zero;
        return inputActions.Gameplay.AimController.ReadValue<Vector2>();
    }

    private Vector2 GetMouseAimDirection()
    {
        Vector2 mousePosition = inputActions.Gameplay.AimMouse.ReadValue<Vector2>();
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);
        return worldPoint - (Vector2)transform.position;
    }

    private void HandleShoot()
    {
        if (!shootingEnabled) return;
        float cooldown = Mathf.Max(0.05f, shootCooldown + (upgradeManager?.FireRateModifier ?? 0f));
        Vector2 dir = -barrel.transform.up;

        if (inputActions.Gameplay.Shoot.IsPressed() && Time.time - lastShoot1Time >= cooldown)
        {
            lastShoot1Time = Time.time;
            barrel.Shoot(dir, upgradeManager?.Slot1Bullet);
        }

        if (inputActions.Gameplay.Shoot2.IsPressed() && Time.time - lastShoot2Time >= cooldown)
        {
            lastShoot2Time = Time.time;
            barrel.Shoot(dir, upgradeManager?.Slot2Bullet);
        }
    }
}
