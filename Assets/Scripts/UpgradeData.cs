using UnityEngine;

public enum UpgradeType { BulletSlot, Stat }
public enum StatUpgradeType { FireRate, BulletDamage, BulletSpeed, MoveSpeed }

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Upgrades/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public Sprite icon;
    public int price;
    public UpgradeType type;

    [Header("Bullet Upgrade")]
    public Bullet bulletPrefab;

    [Header("Stat Upgrade")]
    public StatUpgradeType statType;
    [Tooltip("Additive modifier. e.g. -0.05 reduces fire cooldown by 5%")]
    public float statValue;
}
