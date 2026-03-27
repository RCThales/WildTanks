using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MovementController movementController;
    private float horizontalInput;
    private float verticalInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementController = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
       movementController.Move(ReadInput());
    }

    private Vector2 ReadInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        return new Vector2(horizontalInput, verticalInput).normalized;
    }


}
