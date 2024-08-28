using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    // Event that takes a float parameter for the target alpha
    public event Action<float> OnAlphaChange;

    [SerializeField] private Image targetImage;
    [SerializeField] private float lerpDuration = 1.0f; // Duration for the lerp

    private void Awake()
    {
        //Enabling canvas through script so that I don't have to fucking see the white fucking screen all the fucking time
        GetComponent<Canvas>().enabled = true;
    }

    private void Start()
    {
        

        // Subscribe to the event
        OnAlphaChange += LerpAlpha;
        LerpAlpha(0f);
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        OnAlphaChange -= LerpAlpha;
    }

    public void LerpAlpha(float targetAlpha)
    {
        StartCoroutine(LerpAlphaCoroutine(targetAlpha));
    }

    private IEnumerator LerpAlphaCoroutine(float targetAlpha)
    {
        Color initialColor = targetImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(initialColor.a, targetAlpha, elapsedTime / lerpDuration);
            targetImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, newAlpha);
            yield return null;
        }

        // Ensure the final alpha is set to the target value
        targetImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, targetAlpha);
    }

    // Method to trigger the event
    public void SetTargetAlpha(float alpha)
    {
        OnAlphaChange?.Invoke(alpha);
    }
}
