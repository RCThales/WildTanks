using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private MovementController movementController;
    private PlayerTracking playerTracking;

    private Transform player;

    [SerializeField] private float damage = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementController = GetComponent<MovementController>();
        playerTracking = GetComponentInChildren<PlayerTracking>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        Vector2 direction = ReadPlayerDirection();
        movementController.Move(direction);
        movementController.RotateTowardsDirection(direction, 90f);

        if (playerTracking.playerInRange)
        {
            //Shoot in the future
        }

    }

    private Vector2 ReadPlayerShootingDirection()
    {
        Vector2 direction = playerTracking.playerPosition - (Vector2)transform.position;
        return direction.normalized;

    }

    private Vector2 ReadPlayerDirection()
    {
        return ((Vector2)(player.position - transform.position)).normalized;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
