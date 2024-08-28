using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RewardedAdsBehaviour : MonoBehaviour
{
    AdmobAdsScript admobAdsScript;

    public UnityEvent OnHintGiven;
    public UnityEvent OnSkipLevel;

    // Start is called before the first frame update
    void Start()
    {
        admobAdsScript = FindObjectOfType<AdmobAdsScript>();
        admobAdsScript.LoadRewardedAd();
    }

    [SerializeField] HintSystem[] hintSystems;
    public void GiveHint()
    {
        StartCoroutine(GiveHintWithDelay());
    }

    private IEnumerator GiveHintWithDelay()
    {
        yield return new WaitForSeconds(2f);

        print("HINT SYSTEM SHOULD BE ACTIVE NOW.");
        //Remove anything that was in there before
        hintSystems = null;
        hintSystems = FindObjectsOfType<HintSystem>(true);
        if (hintSystems != null)
        {
            for (int i = 0; i < hintSystems.Length; i++)
            {
                hintSystems[i].gameObject.SetActive(true);
            }
        }
        OnHintGiven.Invoke();
    }

    public void SkipLevel()
    {
        OnSkipLevel.Invoke();
    }
}
