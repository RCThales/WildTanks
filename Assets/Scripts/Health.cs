using UnityEngine;

public class Health : MonoBehaviour
{
    private float currentHealth;

    [SerializeField]
    private float maxHealth = 5f;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle death logic here, such as playing an animation, disabling the object, etc.
        Destroy(gameObject);
    }

}
