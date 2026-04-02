using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private MovementController movementController;
    private PlayerTracking playerTracking;
    private Transform player;
    private Rigidbody2D playerRb;

    [SerializeField] private float damage = 1f;
    [SerializeField] private float stoppingDistance = 1.2f;
    [SerializeField] private float driftAngleThreshold = 25f;
    [SerializeField] private float predictionTime = 0.6f;
    [SerializeField] private float separationRadius = 3f;
    [SerializeField] private float separationWeight = 1.2f;
    [SerializeField] private LayerMask enemyLayer;

    void Start()
    {
        movementController = GetComponent<MovementController>();
        playerTracking = GetComponentInChildren<PlayerTracking>();
        player = GameObject.FindWithTag("Player").transform;
        playerRb = player.GetComponentInParent<Rigidbody2D>();
        if (playerRb == null)
            playerRb = player.GetComponentInChildren<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 predicted = (Vector2)player.position + playerRb.linearVelocity * predictionTime;
        Vector2 toTarget = (predicted - (Vector2)transform.position).normalized;
        toTarget = (toTarget + GetOrbitalSeparation()).normalized;

        float signedAngle = Vector2.SignedAngle(-transform.up, toTarget);
        movementController.SetDrift(Mathf.Abs(signedAngle) > driftAngleThreshold);

        float steer = Mathf.Clamp(-signedAngle / 90f, -1f, 1f);
        float throttle = Vector2.Distance(transform.position, player.position) > stoppingDistance ? 1f : 0f;

        movementController.Move(new Vector2(steer, throttle));
        movementController.RotateTowardsDirection(toTarget, 90f);
    }

    private Vector2 GetOrbitalSeparation()
    {
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, separationRadius, enemyLayer);

        float myAngle = Mathf.Atan2(
            transform.position.y - player.position.y,
            transform.position.x - player.position.x
        );

        float angularPush = 0f;
        foreach (var col in nearby)
        {
            if (col.gameObject == gameObject) continue;

            float otherAngle = Mathf.Atan2(
                col.transform.position.y - player.position.y,
                col.transform.position.x - player.position.x
            );

            float delta = Mathf.DeltaAngle(myAngle * Mathf.Rad2Deg, otherAngle * Mathf.Rad2Deg);
            float dist = Vector2.Distance(transform.position, col.transform.position);
            angularPush -= Mathf.Sign(delta) / Mathf.Max(dist, 0.1f);
        }

        float pushAngle = myAngle + angularPush * separationWeight * Time.fixedDeltaTime;
        return new Vector2(Mathf.Cos(pushAngle), Mathf.Sin(pushAngle)) - (Vector2)(transform.position - player.position).normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            collision.collider.GetComponent<Health>().TakeDamage(damage);
    }
}
