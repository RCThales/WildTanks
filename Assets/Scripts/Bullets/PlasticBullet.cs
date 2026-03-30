using UnityEngine;

public class PlasticBullet : Bullet
{
    private Vector2 direction;


    public override void Initialize(Vector2 direction)
    {
        this.direction = direction;
        Destroy(gameObject, lifeTime);
    }


    private void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }



}
