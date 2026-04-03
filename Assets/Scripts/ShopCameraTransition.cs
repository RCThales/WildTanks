using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class ShopCameraTransition : MonoBehaviour
{
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private float transitionDuration = 0.6f;
    [SerializeField] private float shopTransitionDelay = 0.5f;

    [Header("Shop State")]
    [SerializeField] private float shopOrthoSize = 6f;
    [SerializeField] private float shopOffsetX = 0.55f;
    [SerializeField] private float shopOffsetY = -0.85f;

    [Header("Intro")]
    [SerializeField] private float introStartOrthoSize = 2f;
    [SerializeField] private float introDuration = 1.2f;

    private CinemachinePositionComposer composer;
    private float defaultOrthoSize;
    private Vector3 defaultOffset;
    private Vector3 currentOffset;
    private float currentOrthoSize;
    private Coroutine activeTransition;

    void Awake()
    {
        composer = virtualCamera.GetComponent<CinemachinePositionComposer>();
        defaultOrthoSize = virtualCamera.Lens.OrthographicSize;
        if (composer != null)
            defaultOffset = composer.TargetOffset;

        currentOffset = defaultOffset;
        currentOrthoSize = defaultOrthoSize;
    }

    void Start()
    {
        StartCoroutine(IntroZoom());
    }

    public void TransitionToShop()
    {
        StartTransition(shopOrthoSize, new Vector3(shopOffsetX, shopOffsetY, 0f), shopTransitionDelay);
    }

    public void TransitionToGame()
    {
        StartTransition(defaultOrthoSize, defaultOffset);
    }

    private void StartTransition(float targetSize, Vector3 targetOffset, float delay = 0f)
    {
        if (activeTransition != null) StopCoroutine(activeTransition);
        activeTransition = StartCoroutine(Transition(targetSize, targetOffset, delay));
    }

    private void ApplyState(float orthoSize, Vector3 offset)
    {
        var lens = virtualCamera.Lens;
        lens.OrthographicSize = orthoSize;
        virtualCamera.Lens = lens;

        if (composer != null)
            composer.TargetOffset = offset;

        currentOrthoSize = orthoSize;
        currentOffset = offset;
    }

    private IEnumerator IntroZoom()
    {
        ApplyState(introStartOrthoSize, defaultOffset);

        float elapsed = 0f;
        while (elapsed < introDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / introDuration);
            ApplyState(Mathf.Lerp(introStartOrthoSize, defaultOrthoSize, t), defaultOffset);
            yield return null;
        }

        ApplyState(defaultOrthoSize, defaultOffset);
    }

    private IEnumerator Transition(float targetSize, Vector3 targetOffset, float delay = 0f)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);

        float startSize = currentOrthoSize;
        Vector3 startOffset = currentOffset;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / transitionDuration);
            ApplyState(Mathf.Lerp(startSize, targetSize, t), Vector3.Lerp(startOffset, targetOffset, t));
            yield return null;
        }

        ApplyState(targetSize, targetOffset);
    }
}
