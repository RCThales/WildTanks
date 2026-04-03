using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private GameObject deathVFXPrefab;

    private PlayerHealthUI playerHealthUI;
    private HitInvincibility hitInvincibility;
    private int currentHealth;

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

    public void TakeDamage(int damage)
    {
        if (hitInvincibility != null && hitInvincibility.IsInvincible) return;

        currentHealth -= damage;
        playerHealthUI?.UpdateUI(currentHealth, maxHealth);
        hitInvincibility?.TriggerInvincibility();

        if (currentHealth <= 0)
            Die();
    }

    public void Kill(bool dropLoot = false) => Die(dropLoot);

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        playerHealthUI?.UpdateUI(currentHealth, maxHealth);
    }

    private void Die(bool dropLoot = true)
    {
        if (deathVFXPrefab != null)
        {
            GameObject vfx = Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, 3f);
        }

        if (CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
        else if (CompareTag("Items"))
        {
            GetComponent<UpgradeItem>()?.OnChosen();
        }
        else
        {
            if (dropLoot) GetComponent<LootDropper>()?.Drop(transform.position);
            Destroy(gameObject);
        }
    }
}
