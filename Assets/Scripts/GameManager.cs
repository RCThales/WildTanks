using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI pointsText;

    private int points;
    public int Cash { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddPoints(int amount)
    {
        points += amount;
        pointsText.text = points.ToString();
    }

    public void AddCash(int amount)
    {
        Cash += amount;
    }

    public void SpendCash(int amount)
    {
        Cash = Mathf.Max(0, Cash - amount);
    }
}
