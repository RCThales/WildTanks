using UnityEngine;

public abstract class Bullet : MonoBehaviour
{

    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float lifeTime = 3f;
    [SerializeField] protected float damage = 1f;
    public abstract void Initialize(Vector2 direction);


    private void OnTriggerEnter2D(Collider2D collision)
    {

        Health health = collision.GetComponent<Health>();

        if (collision.CompareTag("Enemy"))
        {
            health?.TakeDamage(damage);
            Destroy(gameObject);
        }
        /*
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        */
    }


}

