using UnityEngine;

public enum UpgradeResult { Applied, NeedsReplacement }

public class PlayerUpgradeManager : MonoBehaviour
{
    [SerializeField] private Bullet defaultSlot1Bullet;
    [SerializeField] private Bullet defaultSlot2Bullet;
    [SerializeField] private UpgradeData defaultSlot1Data;
    [SerializeField] private UpgradeData defaultSlot2Data;

    public Bullet Slot1Bullet { get; private set; }
    public Bullet Slot2Bullet { get; private set; }

    public UpgradeData Slot1Data { get; private set; }
    public UpgradeData Slot2Data { get; private set; }
    public UpgradeData PendingBulletUpgrade { get; private set; }

    public int BulletPicksThisShop { get; private set; } = 0;

    public float FireRateModifier    { get; private set; } = 0f;
    public float DamageModifier      { get; private set; } = 0f;
    public float BulletSpeedModifier { get; private set; } = 0f;
    public float MoveSpeedModifier   { get; private set; } = 0f;

    void Awake()
    {
        Slot1Bullet = defaultSlot1Bullet;
        Slot2Bullet = defaultSlot2Bullet;
        Slot1Data = defaultSlot1Data;
        Slot2Data = defaultSlot2Data;
    }

    public void ResetShopPicks() => BulletPicksThisShop = 0;

    public UpgradeResult ApplyUpgrade(UpgradeData data)
    {
        switch (data.type)
        {
            case UpgradeType.BulletSlot:
                if (BulletPicksThisShop >= 2) return UpgradeResult.Applied;
                if (Slot1Data == null)
                {
                    SetSlot1(data);
                    BulletPicksThisShop++;
                    return UpgradeResult.Applied;
                }
                else if (Slot2Data == null)
                {
                    SetSlot2(data);
                    BulletPicksThisShop++;
                    return UpgradeResult.Applied;
                }
                else
                {
                    PendingBulletUpgrade = data;
                    return UpgradeResult.NeedsReplacement;
                }

            case UpgradeType.Stat:
                ApplyStat(data.statType, data.statValue);
                return UpgradeResult.Applied;
        }
        return UpgradeResult.Applied;
    }

    public void ReplaceSlot(int slot)
    {
        if (PendingBulletUpgrade == null) return;

        if (slot == 0) SetSlot1(PendingBulletUpgrade);
        else           SetSlot2(PendingBulletUpgrade);

        PendingBulletUpgrade = null;
        BulletPicksThisShop++;
    }

    private void SetSlot1(UpgradeData data)
    {
        Slot1Data = data;
        Slot1Bullet = data.bulletPrefab;
        RefreshWeaponDisplays();
    }

    private void SetSlot2(UpgradeData data)
    {
        Slot2Data = data;
        Slot2Bullet = data.bulletPrefab;
        RefreshWeaponDisplays();
    }

    private void RefreshWeaponDisplays()
    {
        foreach (var display in FindObjectsByType<WeaponSlotDisplay>(FindObjectsInactive.Include))
            display.Refresh();
    }

    private void ApplyStat(StatUpgradeType stat, float value)
    {
        switch (stat)
        {
            case StatUpgradeType.FireRate:     FireRateModifier     += value; break;
            case StatUpgradeType.BulletDamage: DamageModifier       += value; break;
            case StatUpgradeType.BulletSpeed:  BulletSpeedModifier  += value; break;
            case StatUpgradeType.MoveSpeed:    MoveSpeedModifier    += value; break;
        }
    }
}
