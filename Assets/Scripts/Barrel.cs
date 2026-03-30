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

}
