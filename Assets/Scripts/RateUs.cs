using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateUs : MonoBehaviour
{
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Rated"))
        {
            PlayerPrefs.SetInt("Rated", 0);
        } else if (PlayerPrefs.GetInt("Rated") == 1)
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] string gameURL;
    public void GoToPlayStore()
    {
        Application.OpenURL(gameURL);
        PlayerPrefs.SetInt("Rated", 1);
    }
}
