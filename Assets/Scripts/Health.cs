using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private float currentHealth;

    [SerializeField]
    private float maxHealth = 5f;

    private PlayerHealthUI playerHealthUI;

    void Awake()
    {
        currentHealth = maxHealth;
        playerHealthUI = GetComponent<PlayerHealthUI>();

#if UNITY_EDITOR
        if (playerHealthUI == null && CompareTag("Player"))
            Debug.LogWarning("Player is missing PlayerHealthUI component!");
#endif

        playerHealthUI?.UpdateUI(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        playerHealthUI?.UpdateUI(currentHealth, maxHealth);

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
