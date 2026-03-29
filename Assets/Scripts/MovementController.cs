using UnityEngine;

public class MovementController : MonoBehaviour
{

    [SerializeField] private float speed = 5f;

    public void Move(Vector2 movement)
    {
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    public void RotateTowardsDirection(Vector2 direction, float offset = 0)
    {

        if (direction.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + offset;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
