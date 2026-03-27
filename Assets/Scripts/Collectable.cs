using UnityEngine;

public class Collectable : MonoBehaviour
{
    

    [SerializeField]
    private int value = 10;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.Instance.AddPoints(value);
            Destroy(gameObject);
        }
    }


}
