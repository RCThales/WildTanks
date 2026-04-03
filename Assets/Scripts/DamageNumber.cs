using System.Collections;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 1.5f;
    [SerializeField] private float duration = 0.9f;

    private TextMeshPro tmp;

    void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
        tmp.GetComponent<MeshRenderer>().sortingOrder = 200;
    }

    public void Setup(int damage, bool isCrit)
    {
        if (isCrit)
        {
            tmp.text = $"{damage}";
            tmp.color = Color.red;
            transform.localScale = Vector3.one * 1.4f;
        }
        else
        {
            tmp.text = damage.ToString();
            tmp.color = Color.white;
            transform.localScale = Vector3.one;
        }

        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Color startColor = tmp.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = startPos + Vector3.up * (floatSpeed * t);
            tmp.color = new Color(startColor.r, startColor.g, startColor.b, 1f - t);
            yield return null;
        }

        Destroy(gameObject);
    }
}
