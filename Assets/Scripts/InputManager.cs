using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance { get; private set; }
    public bool UsingController { get; private set; }

    public event System.Action<bool> OnDeviceChanged;
    void Awake()
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

    public void SetUsingController(bool value)
    {
        if (UsingController == value) return;
        UsingController = value;
        OnDeviceChanged?.Invoke(value);
    }


}
