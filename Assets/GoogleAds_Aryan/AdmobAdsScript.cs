using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Events;

public class AdmobAdsScript : MonoBehaviour
{
    const string appId = "ca-app-pub-6069243694115928~7751394989";

    const bool isTesting = true;
    private string bannerId = "ca-app-pub-6069243694115928/1016695742";
    private string rewardedId = "ca-app-pub-6069243694115928/6075351233";
    private string interstitialId = "ca-app-pub-6069243694115928/9618908793";
    [SerializeField] float timeBetweenBanners = 20f;


    private void Awake()
    {
        if (isTesting)
        {
            bannerId = "ca-app-pub-3940256099942544/6300978111";
            rewardedId = "ca-app-pub-3940256099942544/5224354917";
            interstitialId = "ca-app-pub-3940256099942544/1033173712";
        }
    }

    private void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(InitializationStatus =>
        {
            print("Ads initialised.");
        });

        StartCoroutine(LoadBannerAdWithDelay(2f));
    }


    #region Banner ads
    BannerView bannerView;

    /// <summary>
    /// Creates a 320x50 banner view at top of the screen.
    /// </summary>
    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (bannerView != null)
        {
            DestroyBannerAd();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(bannerId, AdSize.IABBanner, AdPosition.Bottom);
    }

    public void ListenToBannerEvents()
    {
        // Raised when an ad is loaded into the banner view.
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());
            eventOnLoadSuccess.Invoke();
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : " + error);
            NetworkError();

        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    public void LoadBannerAd()
    {
        CreateBannerView();
        ListenToBannerEvents();
        
        if (bannerView == null) {
            CreateBannerView();
        }
        
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        print("Loading banner ad now.");
        bannerView.LoadAd(adRequest); // show the banner on the screen.

        //start coroutine to load another ad with delay
        StartCoroutine(LoadBannerAdWithDelay(timeBetweenBanners));
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }

    }

    private IEnumerator LoadBannerAdWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadBannerAd();
    }
    #endregion


    #region rewarded ads

    private RewardedAd rewardedAd;

    public void LoadRewardedAd()
    {
        if (rewardedAd!=null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(rewardedId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null) {
                print("Rewarded ad failed to load due to " + error);
                NetworkError();
                return;
            }
            print("rewarded ad loaded");
            eventOnLoadSuccess.Invoke();
            rewardedAd = ad;
            RewardedAdEvents(rewardedAd);
        });
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward)=>
            {
                print("Give hint to player");
                EnableHint();

                //Loads next rewarded ad
                LoadRewardedAd();
            });
        } else
        {
            print("Rewarded ad not ready.");
        }
        
        if (isTesting && !Application.isEditor)
        {
            print("Give hint to player");
            EnableHint();

            //Loads next rewarded ad
            LoadRewardedAd();
        }
    }

    public void ShowRewardedAdToSkip()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                print("Give hint to player");
                SkipLevel();

                //Loads next rewarded ad
                LoadRewardedAd();
            });
        }
        else
        {
            print("Rewarded ad not ready.");
        }
    }

    public void RewardedAdEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    [SerializeField] RewardedAdsBehaviour behaviour;
    void EnableHint()
    {
        behaviour = FindObjectOfType<RewardedAdsBehaviour>();
        if (behaviour != null)
        {
            behaviour.GiveHint();
        }
    }

    void SkipLevel()
    {
        behaviour = FindObjectOfType<RewardedAdsBehaviour>();
        if (behaviour != null)
        {
            behaviour.SkipLevel();
        }
    }

    #endregion


    #region interstitial ads

    private InterstitialAd interstitialAd;

    public void LoadInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        InterstitialAd.Load(interstitialId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error!=null || ad==null)
            {
                print("interstitial ad failed to load");
                return;
            }

            print("interstitial ad loaded");

            interstitialAd = ad;
            InterstitialEvent(interstitialAd);
        });
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        } else
        {
            print("interstitial ad not ready.");
        }
    }

    public void InterstitialEvent(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }


    #endregion


    #region network error handling

    public UnityEvent eventOnLoadFailed;
    public UnityEvent eventOnLoadSuccess;

    void NetworkError()
    {
        eventOnLoadFailed.Invoke();
        StartCoroutine(NetworkErrorTryLoadAgain());
    }

    IEnumerator NetworkErrorTryLoadAgain()
    {
        yield return new WaitForSeconds(3);

        LoadBannerAd();
        LoadRewardedAd();
        LoadInterstitialAd();
    }

    #endregion
}
