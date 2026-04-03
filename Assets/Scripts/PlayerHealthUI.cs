using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBarFill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void UpdateUI(int currentHealth, int maxHealth)
    {
        if (healthText != null && healthBarFill != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }
}
