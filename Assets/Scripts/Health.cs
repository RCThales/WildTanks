using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private float currentHealth;

    [SerializeField] private float maxHealth = 5f;
    [SerializeField] private GameObject deathVFXPrefab;

    private PlayerHealthUI playerHealthUI;
    private HitInvincibility hitInvincibility;

    void Awake()
    {
        currentHealth = maxHealth;
        playerHealthUI = GetComponent<PlayerHealthUI>();
        hitInvincibility = GetComponent<HitInvincibility>();

#if UNITY_EDITOR
        if (playerHealthUI == null && CompareTag("Player"))
            Debug.LogWarning("Player is missing PlayerHealthUI component!");
#endif

        playerHealthUI?.UpdateUI(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (hitInvincibility != null && hitInvincibility.IsInvincible) return;

        currentHealth -= damage;
        playerHealthUI?.UpdateUI(currentHealth, maxHealth);
        hitInvincibility?.TriggerInvincibility();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Kill() => Die();

    private void Die()
    {
        if (deathVFXPrefab != null)
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);

        if (CompareTag("Player"))
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }

}
