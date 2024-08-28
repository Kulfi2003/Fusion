using UnityEngine;
using System.Collections;

public class DirectionalShake : MonoBehaviour
{
    public float shakeDuration = 0.2f;  // Duration of the shake effect
    public float shakeAmount = 0.1f;    // Amount of shake on the x and z axes
    public float returnSpeed = 2.0f;    // Speed at which the camera returns to the original position

    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake(Vector3 direction)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeCoroutine(direction));
    }

    private IEnumerator ShakeCoroutine(Vector3 direction)
    {
        float elapsed = 0.0f;
        Vector3 targetPosition = originalPosition + -(direction * shakeAmount);

        // Move the camera to the target position
        while (elapsed < shakeDuration)
        {
            transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, elapsed / shakeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPosition;

        // Smoothly move the camera back to the original position
        elapsed = 0.0f;
        while (transform.localPosition != originalPosition)
        {
            transform.localPosition = Vector3.Lerp(targetPosition, originalPosition, elapsed * returnSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        shakeCoroutine = null;
    }
}
