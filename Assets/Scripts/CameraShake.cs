using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static event System.Action ShakeEvent; // Public event that can be invoked

    private Vector3 originalPosition;
    public float shakeDuration = 0.5f; // Duration of the shake
    public float shakeMagnitude = 0.3f; // Magnitude of the shake
    public float delayBeforeShake = 0.1f; // Delay before the shake starts

    private void OnEnable()
    {
        ShakeEvent += TriggerShake;
    }

    private void OnDisable()
    {
        ShakeEvent -= TriggerShake;
    }

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void TriggerShake()
    {
        StopAllCoroutines();
        StartCoroutine(Shake());
    }

    private System.Collections.IEnumerator Shake()
    {
        // Add delay before shake starts
        yield return new WaitForSeconds(delayBeforeShake);

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            Vector3 randomPoint = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = new Vector3(randomPoint.x, originalPosition.y, randomPoint.z); // Shake only in x and z

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition; // Reset position
    }

    // Static method to invoke the shake event
    public static void TriggerCameraShake()
    {
        ShakeEvent?.Invoke();
    }
}
