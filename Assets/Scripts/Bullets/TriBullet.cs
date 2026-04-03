using UnityEngine;

public class TriBullet : Bullet
{
    [SerializeField] private PlasticBullet subBulletPrefab;
    [SerializeField] private float spreadAngle = 15f;

    public override void Initialize(Vector2 direction)
    {
        float[] angles = { -spreadAngle, 0f, spreadAngle };
        foreach (float angle in angles)
        {
            Vector2 spreadDir = Rotate(direction.normalized, angle);
            PlasticBullet sub = Instantiate(subBulletPrefab, transform.position, Quaternion.identity);
            sub.SetDamageMultiplier(0.5f);
            sub.Initialize(spreadDir);
        }
        Destroy(gameObject);
    }

    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad), sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }
}
