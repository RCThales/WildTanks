using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private CanvasGroup shopCanvasGroup;
    [SerializeField] private Button nextWaveButton;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private TextMeshProUGUI breakTimerText;
    [SerializeField] private TextMeshProUGUI cashText;
    [SerializeField] private float openDuration = 0.4f;
    [SerializeField] private List<UpgradeData> upgradePool;

    void OnEnable()
    {
        nextWaveButton.onClick.AddListener(OnNextWavePressed);
    }

    void OnDisable()
    {
        nextWaveButton.onClick.RemoveListener(OnNextWavePressed);
    }

    public void UpdateBreakTimer(float remaining)
    {
        breakTimerText.text = $"Time Left:{(int)remaining}";
    }

    public void OpenShop()
    {
        RandomizeItems();
        shopCanvas.SetActive(true);
        shopCanvasGroup.alpha = 0f;
        nextWaveButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Go to wave {waveManager.CurrentWave + 1}";
        UpdateCashText();
        StartCoroutine(FadeIn());
    }

    public void UpdateCashText()
    {
        if (cashText != null)
            cashText.text = $"${GameManager.Instance.Cash}";
    }

    private void RandomizeItems()
    {
        if (upgradePool == null || upgradePool.Count == 0) return;

        UpgradeItem[] items = Resources.FindObjectsOfTypeAll<UpgradeItem>();

        List<UpgradeData> bulletPool = new();
        List<UpgradeData> statPool = new();
        foreach (var data in upgradePool)
        {
            if (data.type == UpgradeType.BulletSlot) bulletPool.Add(data);
            else statPool.Add(data);
        }

        Shuffle(bulletPool);
        Shuffle(statPool);

        int bulletIdx = 0, statIdx = 0;
        foreach (var item in items)
        {
            if (item.slotType == UpgradeType.BulletSlot)
                item.upgradeData = bulletIdx < bulletPool.Count ? bulletPool[bulletIdx++] : null;
            else
                item.upgradeData = statIdx < statPool.Count ? statPool[statIdx++] : null;

            item.ApplyIcon();
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < openDuration)
        {
            elapsed += Time.deltaTime;
            shopCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / openDuration);
            yield return null;
        }
        shopCanvasGroup.alpha = 1f;
    }

    public void ForceClose() => StartCoroutine(FadeOutAndClose());

    private void OnNextWavePressed()
    {
        StartCoroutine(FadeOutAndClose());
    }

    private IEnumerator FadeOutAndClose()
    {
        float elapsed = 0f;
        while (elapsed < openDuration)
        {
            elapsed += Time.deltaTime;
            shopCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / openDuration);
            yield return null;
        }
        shopCanvasGroup.alpha = 0f;
        shopCanvas.SetActive(false);
        waveManager.SkipBreak();
    }
}
