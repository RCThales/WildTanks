using DG.Tweening;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float lifeTime = 3f;
    [SerializeField] protected int minDamage = 1;
    [SerializeField] protected int maxDamage = 3;
    [SerializeField] [Range(0f, 1f)] private float critChance = 0.1f;
    [SerializeField] private GameObject hitVFXPrefab;
    [SerializeField] private DamageNumber damageNumberPrefab;

    // Used by TriBullet to scale damage down
    private float damageMultiplier = 1f;

    public abstract void Initialize(Vector2 direction);

    public void SetDamageMultiplier(float multiplier) => damageMultiplier = multiplier;

    private void SpawnHitVFX()
    {
        if (hitVFXPrefab != null)
        {
            GameObject vfx = Instantiate(hitVFXPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, 2f);
        }
    }

    private void SpawnDamageNumber(int damage, bool isCrit, Vector2 position)
    {
        if (damageNumberPrefab == null) return;
        DamageNumber num = Instantiate(damageNumberPrefab, (Vector3)position + Vector3.up * 1.5f, Quaternion.identity);
        num.Setup(damage, isCrit);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            bool isCrit = Random.value < critChance;
            int raw = Mathf.RoundToInt(Random.Range(minDamage, maxDamage) * damageMultiplier);
            int damage = isCrit ? raw * 2 : raw;

            Health health = collision.GetComponent<Health>();
            if (health != null) health.TakeDamage(damage);

            SpawnDamageNumber(damage, isCrit, collision.transform.position);
            SpawnHitVFX();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Items"))
        {
            Health health = collision.GetComponent<Health>();
            if (health != null) health.TakeDamage(1);

            Transform frame = collision.transform.parent;
            if (frame != null)
                frame.DOShakePosition(0.25f, 0.15f, 25, 90, false, true, ShakeRandomnessMode.Harmonic);

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
