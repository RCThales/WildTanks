using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private int value = 10;
    [SerializeField] private float attractRadius = 3f;
    [SerializeField] private float attractSpeed = 8f;
    [SerializeField] private GameObject collectVFXPrefab;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null || !player.gameObject.activeInHierarchy) return;
        if (Vector2.Distance(transform.position, player.position) >= attractRadius) return;

        Vector3 next = Vector3.MoveTowards(transform.position, player.position, attractSpeed * Time.deltaTime);
        if (rb != null)
            rb.MovePosition(next);
        else
            transform.position = next;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        GameManager.Instance.AddPoints(value);
        GameManager.Instance.AddCash(value);
        if (collectVFXPrefab != null)
        {
            GameObject vfx = Instantiate(collectVFXPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, 3f);
        }
        Destroy(gameObject);
    }
}
