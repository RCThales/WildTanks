using UnityEngine;
using UnityEngine.UI;

// Panel shown when both bullet slots are full and player picks a third.
// Set up two buttons in the inspector wired to ChooseSlot1() and ChooseSlot2().
public class WeaponReplaceUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Image slot1Icon;
    [SerializeField] private Image slot2Icon;

    private UpgradeItem pendingItem;
    private PlayerUpgradeManager upgradeManager;

    void Awake()
    {
        upgradeManager = FindAnyObjectByType<PlayerUpgradeManager>();
        panel.SetActive(false);
    }

    public void Show(UpgradeData incoming, UpgradeItem item)
    {
        pendingItem = item;

        if (slot1Icon != null) slot1Icon.sprite = upgradeManager.Slot1Data?.icon;
        if (slot2Icon != null) slot2Icon.sprite = upgradeManager.Slot2Data?.icon;

        panel.SetActive(true);
    }

    public void ChooseSlot1() => Choose(0);
    public void ChooseSlot2() => Choose(1);

    private void Choose(int slot)
    {
        upgradeManager.ReplaceSlot(slot);

        GameManager.Instance.SpendCash(pendingItem.upgradeData.price);
        ShopManager shop = FindAnyObjectByType<ShopManager>();
        shop?.UpdateCashText();

        pendingItem.gameObject.SetActive(false);
        pendingItem = null;
        panel.SetActive(false);

        if (upgradeManager.BulletPicksThisShop >= 2)
            shop?.ForceClose();
    }
}
