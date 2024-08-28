using UnityEngine;
using UnityEngine.UI; // Required for accessing UI elements

public class DPIFinder : MonoBehaviour
{
    private Text dpiText;

    void Start()
    {
        // Get the Text component attached to the same GameObject
        dpiText = GetComponent<Text>();

        if (dpiText == null)
        {
            Debug.LogError("No Text component found on this GameObject.");
            return;
        }

        // Get the DPI of the screen
        float dpi = Screen.dpi;

        // Check if DPI is available
        if (dpi > 0)
        {
            dpiText.text = "DPI: " + dpi;
        }
        else
        {
            dpiText.text = "DPI : NA";
        }
    }
}