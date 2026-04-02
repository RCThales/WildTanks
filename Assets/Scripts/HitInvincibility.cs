using System.Collections;
using UnityEngine;

public class HitInvincibility : MonoBehaviour
{
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private int blinkCount = 3;

    private Collider2D col;
    private SpriteRenderer[] spriteRenderers;
    public bool IsInvincible { get; private set; }

    void Awake()
    {
        col = GetComponent<Collider2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void TriggerInvincibility()
    {
        if (!IsInvincible)
            StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        IsInvincible = true;
        col.enabled = false;

        float blinkInterval = invincibilityDuration / (blinkCount * 2);
        for (int i = 0; i < blinkCount * 2; i++)
        {
            bool visible = i % 2 == 0;
            foreach (var sr in spriteRenderers)
                sr.enabled = visible;
            yield return new WaitForSeconds(blinkInterval);
        }

        foreach (var sr in spriteRenderers)
            sr.enabled = true;
        col.enabled = true;
        IsInvincible = false;
    }
}
