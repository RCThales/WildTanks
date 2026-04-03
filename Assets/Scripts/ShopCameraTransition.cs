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
    private Coroutine activeTransition;

    void Awake()
    {
        composer = virtualCamera.GetComponent<CinemachinePositionComposer>();
        defaultOrthoSize = virtualCamera.Lens.OrthographicSize;

        if (composer != null)
            defaultOffset = composer.TargetOffset;
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

    private IEnumerator IntroZoom()
    {
        var lens = virtualCamera.Lens;
        lens.OrthographicSize = introStartOrthoSize;
        virtualCamera.Lens = lens;

        float elapsed = 0f;
        while (elapsed < introDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / introDuration);
            lens = virtualCamera.Lens;
            lens.OrthographicSize = Mathf.Lerp(introStartOrthoSize, defaultOrthoSize, t);
            virtualCamera.Lens = lens;
            yield return null;
        }

        lens = virtualCamera.Lens;
        lens.OrthographicSize = defaultOrthoSize;
        virtualCamera.Lens = lens;
    }

    private IEnumerator Transition(float targetSize, Vector3 targetOffset, float delay = 0f)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float startSize = virtualCamera.Lens.OrthographicSize;
        Vector3 startOffset = composer != null ? composer.TargetOffset : Vector3.zero;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / transitionDuration);

            var lens = virtualCamera.Lens;
            lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, t);
            virtualCamera.Lens = lens;

            if (composer != null)
                composer.TargetOffset = Vector3.Lerp(startOffset, targetOffset, t);

            yield return null;
        }

        var finalLens = virtualCamera.Lens;
        finalLens.OrthographicSize = targetSize;
        virtualCamera.Lens = finalLens;

        if (composer != null)
            composer.TargetOffset = targetOffset;
    }
}
