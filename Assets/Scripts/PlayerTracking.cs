using UnityEngine;

public class PlayerTracking : MonoBehaviour
{

    public Vector2 playerPosition { get; private set; }
    public bool playerInRange { get; private set; }
  

        private void OnTriggerStay2D(Collider2D collision)
    {

           Debug.Log("Something entered: " + collision.gameObject.name);
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            playerPosition = collision.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
