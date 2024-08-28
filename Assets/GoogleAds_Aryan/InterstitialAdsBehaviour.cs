using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterstitialAdsBehaviour : MonoBehaviour
{
    [SerializeField] int countForAd = 0;
    [SerializeField] AdmobAdsScript admobAdsScript;
    [SerializeField] float delayToLoad = 2f;

    private void Start()
    {
        StartCoroutine(LoadInterstitialAfterDelay());
    }

    public void incrementCount()
    {
        countForAd++;
        if (countForAd % 3 == 0)
        {
            StartCoroutine(ShowInterstitialAfterDelay());

            StartCoroutine(LoadInterstitialAfterDelay());
        }
    }

    private IEnumerator LoadInterstitialAfterDelay()
    {
        yield return new WaitForSeconds(delayToLoad);

        admobAdsScript.LoadInterstitialAd();
    }

    private IEnumerator ShowInterstitialAfterDelay()
    {
        yield return new WaitForSeconds(0.75f);

        admobAdsScript.ShowInterstitialAd();

    }
}
