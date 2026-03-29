using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private MovementController movementController;
    private PlayerTracking playerTracking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementController = GetComponent<MovementController>();
        playerTracking = GetComponentInChildren<PlayerTracking>();
    }

    // Update is called once per frame
    void Update()
    {

        movementController.Move(ReadPlayerDirection());
        movementController.RotateTowardsDirection(ReadPlayerDirection(), 180f);

        if (playerTracking.playerInRange)
        {
            //Shoot in the future
        }

    }

    private Vector2 ReadPlayerDirection()
    {
        Vector2 direction = playerTracking.playerPosition - (Vector2)transform.position;
        return direction.normalized;

    }

}
