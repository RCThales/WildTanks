using System.Collections;
using UnityEngine;

public class HitInvincibility : MonoBehaviour
{
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private int blinkCount = 3;
    [SerializeField] private string enemyLayerName = "Enemy";

    private SpriteRenderer[] spriteRenderers;
    public bool IsInvincible { get; private set; }

    void Awake()
    {
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

        int enemyLayer = LayerMask.NameToLayer(enemyLayerName);
        if (enemyLayer >= 0)
            Physics2D.IgnoreLayerCollision(gameObject.layer, enemyLayer, true);

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

        if (enemyLayer >= 0)
            Physics2D.IgnoreLayerCollision(gameObject.layer, enemyLayer, false);

        IsInvincible = false;
    }
}
