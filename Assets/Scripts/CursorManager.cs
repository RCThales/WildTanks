using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

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
    private void OnEnable()
    {
        InputManager.Instance.OnDeviceChanged += HandleDeviceChanged;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnDeviceChanged -= HandleDeviceChanged;
    }

    private void HandleDeviceChanged(bool usingController)
    {
        Cursor.visible = !usingController;
        Cursor.lockState = usingController ?
        CursorLockMode.Locked : CursorLockMode.None;
    }

}