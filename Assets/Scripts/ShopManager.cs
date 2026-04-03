using System.Collections;
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
    [SerializeField] private float openDuration = 0.4f;

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
        shopCanvas.SetActive(true);
        shopCanvasGroup.alpha = 0f;
        nextWaveButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Go to wave {waveManager.CurrentWave + 1}";
        StartCoroutine(FadeIn());
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
