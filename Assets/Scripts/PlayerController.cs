using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private MovementController movementController;

    private PlayerInputActions inputActions;


    private void OnEnable()
    {
        inputActions.Gameplay.Enable();
        inputActions.Gameplay.Move.performed += OnInputPerformed;
        inputActions.Gameplay.AimController.performed += OnInputPerformed;
        inputActions.Gameplay.AimMouse.performed += OnInputPerformed;
        inputActions.Gameplay.Shoot.performed += OnInputPerformed;
    }

    private void OnDisable()
    {
        inputActions.Gameplay.Move.performed -= OnInputPerformed;
        inputActions.Gameplay.AimController.performed -= OnInputPerformed;
        inputActions.Gameplay.AimMouse.performed -= OnInputPerformed;
        inputActions.Gameplay.Shoot.performed -= OnInputPerformed;
        inputActions.Disable();
    }


    void Awake()
    {
        movementController = GetComponent<MovementController>();
        inputActions = new PlayerInputActions();

    }

    void Start()
    {
        InputManager.Instance.SetUsingController(Gamepad.current != null); // Assume controller by default, will be updated on input
    }


    // Update is called once per frame
    void Update()
    {

        HandleMovement();
        HandleRotation();
        HandleShoot();
    }

    private void HandleMovement()
    {
        Vector2 moveInput = inputActions.Gameplay.Move.ReadValue<Vector2>();
        movementController.Move(moveInput);
    }


    private void OnInputPerformed(InputAction.CallbackContext ctx)
    {
        InputManager.Instance.SetUsingController(ctx.control.device is Gamepad);
    }

    private void HandleRotation()
    {
        Vector2 direction = InputManager.Instance.UsingController ? GetControllerAimDirection() : GetMouseAimDirection();
        movementController.RotateTowardsDirection(direction);
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
        if (inputActions.Gameplay.Shoot.triggered)
        {

            Debug.Log("Shoot!");
        }
    }


}
