using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    private int points = 0;
    [SerializeField] TextMeshProUGUI pointsText;

    private void Awake()
    {
        if (Instance == null)
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
        pointsText.text = points.ToString();
    }

}
