using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField]
    private Bullet bulletPrefab;
    [SerializeField]
    private Transform firePoint;
    private float shootRotationOffset = -90f; // Adjust for bullet sprites facing right by default

    public void Shoot(Vector2 direction)
    {
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, shootRotationOffset));
        bullet.Initialize(direction);
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
