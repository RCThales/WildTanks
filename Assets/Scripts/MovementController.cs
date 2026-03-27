using UnityEngine;

public class MovementController : MonoBehaviour
{

    [SerializeField] private float speed = 5f;

    public void Move(Vector2 movement)
    {   
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

}
