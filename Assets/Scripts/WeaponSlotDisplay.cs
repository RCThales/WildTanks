using UnityEngine;

// Attach to each CurrentWeapons frame. Assign slotIndex 0 or 1 in the Inspector.
public class WeaponSlotDisplay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer iconRenderer;
    [SerializeField] private int slotIndex; // 0 = slot 1, 1 = slot 2

    private static readonly Color emptyColor = new Color(0.4f, 0.4f, 0.4f, 1f);

    private PlayerUpgradeManager upgradeManager;

    void Start()
    {
        upgradeManager = FindAnyObjectByType<PlayerUpgradeManager>();
        Refresh();
    }

    public void Refresh()
    {
        if (iconRenderer == null) return;

        UpgradeData data = slotIndex == 0 ? upgradeManager?.Slot1Data : upgradeManager?.Slot2Data;

        if (data != null && data.icon != null)
        {
            iconRenderer.sprite = data.icon;
            iconRenderer.color = Color.white;
        }
        else
        {
            iconRenderer.sprite = null;
            iconRenderer.color = emptyColor;
        }
    }
}
