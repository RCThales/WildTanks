using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UpgradeItem : MonoBehaviour
{
    [SerializeField] public UpgradeData upgradeData;
    [SerializeField] public UpgradeType slotType;
    [SerializeField] private SpriteRenderer frameRenderer;
    [SerializeField] private TextMeshProUGUI priceText;

    private static readonly Color emptyFrameColor = new Color(0.4f, 0.4f, 0.4f, 1f);

    private static readonly List<UpgradeItem> all = new List<UpgradeItem>();
    public static IReadOnlyList<UpgradeItem> All => all;

    private Health health;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        health = GetComponent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        all.Add(this);
        ApplyIcon();
    }

    public void ApplyIcon()
    {
        bool hasData = upgradeData != null && upgradeData.icon != null;

        if (spriteRenderer != null)
            spriteRenderer.sprite = hasData ? upgradeData.icon : null;

        if (frameRenderer != null)
            frameRenderer.color = hasData ? Color.white : emptyFrameColor;

        if (priceText != null)
        {
            priceText.gameObject.SetActive(hasData);
            if (hasData) priceText.text = $"${upgradeData.price}";
        }

        gameObject.SetActive(hasData);
    }

    void OnDisable() => all.Remove(this);

    public void OnChosen()
    {
        PlayerUpgradeManager upgradeManager = FindAnyObjectByType<PlayerUpgradeManager>();
        if (upgradeManager == null || upgradeData == null) { gameObject.SetActive(false); return; }

        if (GameManager.Instance.Cash < upgradeData.price)
        {
            transform.parent?.DOShakePosition(0.35f, 0.25f, 30, 90, false, true, ShakeRandomnessMode.Harmonic);
            return;
        }

        var result = upgradeManager.ApplyUpgrade(upgradeData);

        if (result == UpgradeResult.NeedsReplacement)
        {
            FindAnyObjectByType<WeaponReplaceUI>()?.Show(upgradeData, this);
            return;
        }

        GameManager.Instance.SpendCash(upgradeData.price);
        FindAnyObjectByType<ShopManager>()?.UpdateCashText();

        gameObject.SetActive(false);

        if (upgradeData.type == UpgradeType.BulletSlot && upgradeManager.BulletPicksThisShop >= 2)
            FindAnyObjectByType<ShopManager>()?.ForceClose();
    }

    public void ResetForShop()
    {
        gameObject.SetActive(true);
        health?.ResetHealth();
    }
}
