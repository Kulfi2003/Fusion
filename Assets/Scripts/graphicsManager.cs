using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class graphicsManager : MonoBehaviour
{
    int gameGraphics;
    CubeToggle button;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("graphics"))
        {
            PlayerPrefs.SetInt("graphics", 1);
        }

        gameGraphics = PlayerPrefs.GetInt("graphics");

        QualitySettings.SetQualityLevel(gameGraphics);
    }

    public void SetQuality(int i)
    {
        PlayerPrefs.SetInt("graphics", i);

        gameGraphics = PlayerPrefs.GetInt("graphics");

        QualitySettings.SetQualityLevel(gameGraphics);
    }
}
