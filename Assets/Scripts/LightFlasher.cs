using UnityEngine;
using UnityEngine.Rendering;

public class LightFlasher : MonoBehaviour
{
    public Light targetLight;  // Reference to the Light component
    public float minIntensity = 0f;  // Minimum intensity
    public float maxIntensity = 200f;  // Maximum intensity
    public float speed = 2f;  // Speed of the lerp
    public GameManager manager;

    private float t = 0f;
    private bool increasing = true;

    void Update()
    {
        if (!manager.gameOver)
        {
            if (increasing)
            {
                t += Time.deltaTime * speed;
                if (t >= 1f)
                {
                    t = 1f;
                    increasing = false;
                }
            }
            else
            {
                t -= Time.deltaTime * speed;
                if (t <= 0f)
                {
                    t = 0f;
                    increasing = true;
                }
            }

            targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        }
    }
}
