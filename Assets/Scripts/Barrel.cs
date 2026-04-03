using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Barrel : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float recoilDistance = 0.4f;
    [SerializeField] private float recoilReturnSpeed = 10f;
    [SerializeField] private float squishAmount = 0.25f;

    private float shootRotationOffset = -90f;
    private Vector3 restPosition;
    private Vector3 restScale;
    private float currentRecoil = 0f;

    void Awake()
    {
        restPosition = transform.localPosition;
        restScale = transform.localScale;
    }

    void Update()
    {
        currentRecoil = Mathf.Lerp(currentRecoil, 0f, recoilReturnSpeed * Time.deltaTime);

        if (currentRecoil < 0.001f)
        {
            currentRecoil = 0f;
            transform.localPosition = restPosition;
            transform.localScale = restScale;
            return;
        }

        float squish = squishAmount * Mathf.Clamp01(currentRecoil / recoilDistance);
        transform.localPosition = restPosition - new Vector3(transform.up.x, transform.up.y, 0f) * currentRecoil;
        transform.localScale = new Vector3(restScale.x * (1f + squish), restScale.y * (1f - squish), restScale.z);
    }

    public void Shoot(Vector2 direction, Bullet overridePrefab = null)
    {
        Bullet prefab = overridePrefab != null ? overridePrefab : bulletPrefab;
        if (prefab == null) return;
        Bullet bullet = Instantiate(prefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, shootRotationOffset));
        bullet.Initialize(direction);
        currentRecoil = recoilDistance;
    }

    public void RotateTowardsDirection(Vector2 direction)
    {
        if (direction.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
