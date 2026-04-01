using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private MovementController movementController;

    private PlayerInputActions inputActions;
    Barrel barrel;

    Health health;
    private float shootCooldown = 0.2f;
    private float lastShootTime = 0f;


    private void OnEnable()
    {
        inputActions.Gameplay.Enable();
        inputActions.Gameplay.Move.performed += OnInputPerformed;
        inputActions.Gameplay.AimController.performed += OnInputPerformed;
        inputActions.Gameplay.AimMouse.performed += OnInputPerformed;
        inputActions.Gameplay.Shoot.performed += OnInputPerformed;
        inputActions.Gameplay.Drift.performed += OnInputPerformed;
    }

    private void OnDisable()
    {
        inputActions.Gameplay.Move.performed -= OnInputPerformed;
        inputActions.Gameplay.AimController.performed -= OnInputPerformed;
        inputActions.Gameplay.AimMouse.performed -= OnInputPerformed;
        inputActions.Gameplay.Shoot.performed -= OnInputPerformed;
        inputActions.Gameplay.Drift.performed -= OnInputPerformed;
        inputActions.Disable();
    }


    void Awake()
    {
        movementController = GetComponent<MovementController>();
        inputActions = new PlayerInputActions();
        barrel = GetComponentInChildren<Barrel>();
        health = GetComponent<Health>();

    }

    void Start()
    {
        InputManager.Instance.SetUsingController(Gamepad.current != null); // Assume controller by default, will be updated on input
    }


    // Update is called once per frame
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

    private void HandleMovement()
    {
        Vector2 input = inputActions.Gameplay.Move.ReadValue<Vector2>();
        movementController.Move(input);
    }
    private void HandleDrift()
    {
        movementController.SetDrift(inputActions.Gameplay.Drift.IsPressed());
    }

    private void OnInputPerformed(InputAction.CallbackContext ctx)
    {
        InputManager.Instance.SetUsingController(ctx.control.device is Gamepad);
    }

    private void HandleRotation()
    {
        Vector2 direction = InputManager.Instance.UsingController ?
            GetControllerAimDirection() : GetMouseAimDirection();

        // Rotate barrel only, not the tank body
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
        if (inputActions.Gameplay.Shoot.IsPressed() && Time.time - lastShootTime >= shootCooldown)
        {

            lastShootTime = Time.time;
            Debug.DrawRay(transform.position, barrel.transform.up * 2, Color.red, 0.5f);
            barrel.Shoot(-barrel.transform.up);

        }
    }


}
