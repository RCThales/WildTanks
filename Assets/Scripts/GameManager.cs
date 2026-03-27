using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set;}
    private int points = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int pointsToAdd)
    {
        points += pointsToAdd;
        Debug.Log("Points: " + points);
    }
}
