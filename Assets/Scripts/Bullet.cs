using UnityEngine;

public abstract class Bullet : MonoBehaviour
{

    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float lifeTime = 3f;
    [SerializeField] protected float damage = 1f;
    [SerializeField] private GameObject hitVFXPrefab;
    public float Damage => damage;
    public abstract void Initialize(Vector2 direction);

    private void SpawnHitVFX()
    {
        if (hitVFXPrefab != null)
            Instantiate(hitVFXPrefab, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();

        if (collision.CompareTag("Enemy"))
        {
            health?.TakeDamage(damage);
            SpawnHitVFX();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            SpawnHitVFX();
            Destroy(gameObject);
        }
    }


}

