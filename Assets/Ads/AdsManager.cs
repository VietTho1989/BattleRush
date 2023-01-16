using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Events;
using UnityEngine.Networking;
using GoogleMobileAds.Common;
//using AppsFlyerSDK;
using UnityEngine.SceneManagement;

namespace BattleRushS
{
    public enum ADSTYPE
    {
        Rewarded,
        Interstitial
    }

    public class AdsManager : MonoBehaviour
    {

        public const string REMOVE_AD = "REMOVE_AD";


        private static AdsManager _instance;
        public static AdsManager Instance => _instance;

        private RewardedAd rewardedAd;
        private UnityAction rewardAdDoneCallback;
        private BannerView bannerView;
        private InterstitialAd interstitial;

        private bool _showing;

        [Header("Id Test")]
        public string bannerIdTest = "ca-app-pub-3940256099942544/6300978111";
        public string interstitialIdTest = "ca-app-pub-3940256099942544/8691691433";
        public string rewardIdTest = "ca-app-pub-3940256099942544/5224354917";
        public string appOpenIdTest = "ca-app-pub-3940256099942544/3419835294";

        [Header("Id Android")]
        [SerializeField] string bannerIdAndroid = "";
        [SerializeField] string interstitialIdAndroid = "";
        [SerializeField] string rewardIdAndroid = "";
        [SerializeField] string appOpenIdAndroid = "";

        [Header("Id IOS")]
        [SerializeField] string bannerIdIOS = "";
        [SerializeField] string interstitialIdIOS = "";
        [SerializeField] string rewardIdIOS = "";
        [SerializeField] string appOpenIdIOS = "";

        private AppOpenAd appOpenAd;
        private bool isShowingOpenAd = false;
        private bool IsAppOpenAdAvailable
        {
            get
            {
                return appOpenAd != null && (DateTime.UtcNow - loadTime).TotalHours < 4;
            }
        }

        private DateTime loadTime;

        // Use this for initialization
        private void Awake()
        {
            if (_instance == null) _instance = this;
            else Destroy(gameObject);
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            MobileAds.Initialize((initStatus) =>
            {
                Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
                foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
                {
                    string className = keyValuePair.Key;
                    AdapterStatus status = keyValuePair.Value;
                    switch (status.InitializationState)
                    {
                        case AdapterState.NotReady:
                            // The adapter initialization did not complete.
                            MonoBehaviour.print("Adapter: " + className + " not ready.");
                            break;
                        case AdapterState.Ready:
                            // The adapter was successfully initialized.
                            MonoBehaviour.print("Adapter: " + className + " is initialized.");
                            break;
                    }
                }
            });

            RequestRewardAd();
            RequestInterstitial();

            // Load an app open ad when the scene starts
            LoadAdOpenApp();

            // Listen to application foreground and background events.
            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;

        }

        private void RequestRewardAd()
        {
            _showing = false;
            string adUnitId;
#if UNITY_ANDROID
            adUnitId = rewardIdAndroid;
#elif UNITY_IPHONE
            adUnitId = rewardIdIOS;
#else
            adUnitId = "unexpected_platform";
#endif

#if EnviromentTest
            this.rewardedAd = new RewardedAd(rewardIdTest);
#else
            this.rewardedAd = new RewardedAd(adUnitId);
#endif

            // Called when the user should be rewarded for interacting with the ad.
            this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
            // this.rewardedAd.OnAdFailedToLoad += RewardedAd_OnAdFailedToLoad;
            this.rewardedAd.OnAdFailedToShow += RewardedAd_OnAdFailedToShow;
            this.rewardedAd.OnAdClosed += RewardedAd_OnAdClosed;
            this.rewardedAd.OnPaidEvent += HandlePaidEventRewarded;

            // Create an empty ad request.

            AdRequest request = new AdRequest.Builder()
            .Build();
            this.rewardedAd.LoadAd(request);
        }

        private void RewardedAd_OnAdClosed(object sender, EventArgs e)
        {
            RequestRewardAd();
            RequestInterstitial();
            string currentScene = SceneManager.GetActiveScene().name;
            //FirebaseManager.Instance.LogEventScreen(currentScene, currentScene);
        }

        private void RewardedAd_OnAdFailedToShow(object sender, AdErrorEventArgs e)
        {
            Debug.LogError(" ========== ADS FAIL TO SHOW " + e.Message);
        }

        private void RewardedAd_OnAdFailedToLoad(object sender, AdErrorEventArgs e)
        {
            Debug.LogError(" ========== ADS FAIL TO LOAD " + e.Message);
        }


        public void HandleRewardedAdLoaded(object sender, EventArgs args)
        {

        }

        public void HandleUserEarnedReward(object sender, Reward args)
        {
            rewardAdDoneCallback?.Invoke();
            _showing = false;
        }

        private void HandlePaidEventRewarded(object sender, AdValueEventArgs args)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                AdValue advalue = args.AdValue;
                RewardedAd idAt = (RewardedAd)sender;
                string adSourceValue = GetNetworkName(idAt.GetResponseInfo().GetLoadedAdapterResponseInfo().AdSourceName);
                /*if (FirebaseManager.Instance.firebaseInitialized)
                {
                    Firebase.Analytics.Parameter[] LTVParameters =
                    {
                        new Firebase.Analytics.Parameter("value", advalue.Value/1000000f),
                        new Firebase.Analytics.Parameter("precision", advalue.Precision.ToString()),
                        new Firebase.Analytics.Parameter("ad_format", "rewarded"),
                        new Firebase.Analytics.Parameter("currency", advalue.CurrencyCode),
                        new Firebase.Analytics.Parameter("ad_platform", "googleadmob"),
                        new Firebase.Analytics.Parameter("ad_source", adSourceValue),
                    };
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("paid_ad_impression", LTVParameters);
                }*/
                /*Dictionary<string, string> eventValues = new Dictionary<string, string>();
                eventValues.Add("event_revenue", $"{advalue.Value/1000000f}");
                eventValues.Add("event_revenue_currency", advalue.CurrencyCode.ToString());
                eventValues.Add("monetization_network", "googleadmob");
                eventValues.Add("mediation_network", adSourceValue);
                eventValues.Add("ad_type", "rewarded");
                eventValues.Add("ad_unit", "rewarded");
                AppsFlyer.sendEvent("af_revenue", eventValues);*/
            });
        }

        public bool IsShowing()
        {
            return _showing;
        }

        #region Banner
        public void RequestBanner()
        {
#if UNITY_ANDROID
            string adUnitId = bannerIdAndroid;
#elif UNITY_IPHONE
            string adUnitId = bannerIdIOS;
#else
            string adUnitId = "unexpected_platform";
#endif
            bool removeBanner = PlayerPrefs.GetInt(REMOVE_AD, 0) == 1;
            if (bannerView != null)
            {
                bannerView.Destroy();
            }
            if (removeBanner) return;
            AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
#if EnviromentTest
            bannerView = new BannerView(bannerIdTest, adaptiveSize, AdPosition.Bottom);
#else
            bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);
#endif



            // Called when an ad request has successfully loaded.
            bannerView.OnAdLoaded += OnBannerLoaded;
            // Called when an ad request failed to load.
            bannerView.OnAdFailedToLoad += OnBannerFailedToLoad;
            // Called when an ad is clicked.
            bannerView.OnAdOpening += OnBannerOpened;
            // Called when the user returned from the app after an ad click.
            bannerView.OnAdClosed += OnBannerClosed;
            bannerView.OnPaidEvent += this.HandlePaidEventBanner;

            AdRequest request = new AdRequest.Builder()
            .Build();

            bannerView.LoadAd(request);
            //FirebaseManager.Instance.LogShow_Banner_Ads();
            //FirebaseManager.Instance.LogShow_Banner_And_Interstitial_Ads();
        }

        private void OnBannerLoaded(object sender, EventArgs args)
        {
            Debug.unityLogger.Log("OnBannerLoaded event received");
        }

        private void OnBannerFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.unityLogger.Log("OnBannerFailedToLoad event received with message: " + args.LoadAdError.GetMessage());
        }

        private void OnBannerOpened(object sender, EventArgs args)
        {
            Debug.unityLogger.Log("OnBannerOpened event received");
        }

        private void OnBannerClosed(object sender, EventArgs args)
        {
            Debug.unityLogger.Log("OnBannerClosed event received");
        }

        public void DestroyBanner()
        {
            if (bannerView != null)
            {
                bannerView.Destroy();
            }
        }

        private void HandlePaidEventBanner(object sender, AdValueEventArgs args)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                AdValue advalue = args.AdValue;
                BannerView idAt = (BannerView)sender;
                string adSourceValue = GetNetworkName(idAt.GetResponseInfo().GetLoadedAdapterResponseInfo().AdSourceName);
                /*if (FirebaseManager.Instance.firebaseInitialized)
                {
                    Firebase.Analytics.Parameter[] LTVParameters =
                    {
                        new Firebase.Analytics.Parameter("value", advalue.Value/1000000f),
                        new Firebase.Analytics.Parameter("precision", advalue.Precision.ToString()),
                        new Firebase.Analytics.Parameter("ad_format", "banner"),
                             new Firebase.Analytics.Parameter("currency", advalue.CurrencyCode),
                        new Firebase.Analytics.Parameter("ad_platform","googleadmob"),
                         new Firebase.Analytics.Parameter("ad_source",adSourceValue),
                    };
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("paid_ad_impression", LTVParameters);
                }*/
                // AppsFlyer
                /*{
                    Dictionary<string, string> eventValues = new Dictionary<string, string>();
                    eventValues.Add("event_revenue", $"{advalue.Value / 1000000f}");
                    eventValues.Add("event_revenue_currency", advalue.CurrencyCode.ToString());
                    eventValues.Add("monetization_network", "googleadmob");
                    eventValues.Add("mediation_network", adSourceValue);
                    eventValues.Add("ad_type", "banner");
                    eventValues.Add("ad_unit", "banner");
                    AppsFlyer.sendEvent("af_revenue", eventValues);
                }*/               
            });
        }
        #endregion

        #region interstitial
        //ca-app-pub-6345235397962673/5185180824

        public void RequestInterstitial()
        {
            _showing = false;
#if UNITY_ANDROID
            string adUnitId = interstitialIdAndroid;
#elif UNITY_IPHONE
            string adUnitId = interstitialIdIOS;
#else
            string adUnitId = "unexpected_platform";
#endif

#if EnviromentTest
            interstitial = new InterstitialAd(interstitialIdTest);
#else
            interstitial = new InterstitialAd(adUnitId);
#endif
            // Called when an ad request has successfully loaded.
            interstitial.OnAdLoaded += OnInterstitialLoaded;
            // Called when an ad request failed to load.
            interstitial.OnAdFailedToLoad += OnInterstitialFailedToLoad;
            // Called when an ad is shown.
            interstitial.OnAdOpening += OnInterstitialOpened;
            // Called when the ad is closed.
            interstitial.OnAdClosed += Interstitial_OnAdClosed;

            interstitial.OnPaidEvent += HandlePaidEventFull;

            AdRequest request = new AdRequest.Builder()
            .Build();

            interstitial.LoadAd(request);
        }

        private void OnInterstitialLoaded(object sender, EventArgs args)
        {
            Debug.unityLogger.Log("Loaded event received");
        }

        private void OnInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.unityLogger.Log("OnInterstitialFailedToLoad event received with message: " + args.LoadAdError.GetMessage());
        }

        private void OnInterstitialOpened(object sender, EventArgs args)
        {
            Debug.unityLogger.Log("OnInterstitialOpened event received");
        }

        private void HandlePaidEventFull(object sender, AdValueEventArgs args)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                AdValue advalue = args.AdValue;
                InterstitialAd idAt = (InterstitialAd)sender;
                string adSourceValue = GetNetworkName(idAt.GetResponseInfo().GetLoadedAdapterResponseInfo().AdSourceName);
                /*if (FirebaseManager.Instance.firebaseInitialized)
                {
                    Firebase.Analytics.Parameter[] LTVParameters =
                    {
                new Firebase.Analytics.Parameter("value", advalue.Value/1000000f),
                new Firebase.Analytics.Parameter("precision", advalue.Precision.ToString()),
                new Firebase.Analytics.Parameter("ad_format", "interstitial"),
                     new Firebase.Analytics.Parameter("currency", advalue.CurrencyCode),
                new Firebase.Analytics.Parameter("ad_platform", "googleadmob"),
                 new Firebase.Analytics.Parameter("ad_source", adSourceValue),

            };
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("paid_ad_impression", LTVParameters);
                }*/
                // AppsFlyer
                /*{
                    Dictionary<string, string> eventValues = new Dictionary<string, string>();
                    eventValues.Add("event_revenue", $"{advalue.Value / 1000000f}");
                    eventValues.Add("event_revenue_currency", advalue.CurrencyCode.ToString());
                    eventValues.Add("monetization_network", "googleadmob");
                    eventValues.Add("mediation_network", adSourceValue);
                    eventValues.Add("ad_type", "interstitial");
                    eventValues.Add("ad_unit", "interstitial");
                    AppsFlyer.sendEvent("af_revenue", eventValues);
                } */               
            });
        }

        private void Interstitial_OnAdClosed(object sender, EventArgs e)
        {
            rewardAdDoneCallback?.Invoke();
            RequestRewardAd();
            RequestInterstitial();
            string currentScene = SceneManager.GetActiveScene().name;
            //FirebaseManager.Instance.LogEventScreen(currentScene, currentScene);
        }
        #endregion

        #region Google App Open Ad


        public void LoadAdOpenApp()
        {
#if UNITY_ANDROID
            string adUnitId = appOpenIdAndroid;
#elif UNITY_IPHONE
            string adUnitId = appOpenIdIOS;
#else
            string adUnitId = "unexpected_platform";
#endif
            AdRequest request = new AdRequest.Builder().Build();

#if EnviromentTest
           AppOpenAd.LoadAd(appOpenIdTest, ScreenOrientation.Portrait, request, ((appOpenAd, error) =>
            {
                if (error != null)
                {
                    // Handle the error.
                    Debug.LogFormat("Failed to load the ad. (reason: {0})", error.LoadAdError.GetMessage());
                    return;
                }

                // App open ad is loaded.
                this.appOpenAd = appOpenAd;
                loadTime = DateTime.UtcNow;
            }));
#else

            // Load an app open ad for portrait orientation
            AppOpenAd.LoadAd(adUnitId, ScreenOrientation.Portrait, request, ((appOpenAd, error) =>
            {
                if (error != null)
                {
                    // Handle the error.
                    Debug.LogFormat("Failed to load the ad. (reason: {0})", error.LoadAdError.GetMessage());
                    return;
                }

                // App open ad is loaded.
                this.appOpenAd = appOpenAd;
            }));
#endif
        }

        public void ShowOpenAdIfAvailable()
        {
            if (!IsAppOpenAdAvailable || isShowingOpenAd)
            {
                return;
            }

            appOpenAd.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent;
            appOpenAd.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent;
            appOpenAd.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent;
            appOpenAd.OnAdDidRecordImpression += HandleAdDidRecordImpression;
            appOpenAd.OnPaidEvent += HandlePaidEvent;

            appOpenAd.Show();
        }

        private void HandleAdDidDismissFullScreenContent(object sender, EventArgs args)
        {
            Debug.Log("Closed app open ad");
            // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
            appOpenAd = null;
            isShowingOpenAd = false;
            //LoadAdOpenApp();
        }

        private void HandleAdFailedToPresentFullScreenContent(object sender, AdErrorEventArgs args)
        {
            Debug.LogFormat("Failed to present the ad (reason: {0})", args.AdError.GetMessage());
            // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
            appOpenAd = null;
            LoadAdOpenApp();
        }

        private void HandleAdDidPresentFullScreenContent(object sender, EventArgs args)
        {
            Debug.Log("Displayed app open ad");
            isShowingOpenAd = true;
        }

        private void HandleAdDidRecordImpression(object sender, EventArgs args)
        {
            Debug.Log("Recorded ad impression");
        }

        private void HandlePaidEvent(object sender, AdValueEventArgs args)
        {
            Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
                    args.AdValue.CurrencyCode, args.AdValue.Value);

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                AdValue advalue = args.AdValue;
                AppOpenAd idAt = (AppOpenAd)sender;
                string adSourceValue = GetNetworkName(idAt.GetResponseInfo().GetLoadedAdapterResponseInfo().AdSourceName);
                /*if (FirebaseManager.Instance.firebaseInitialized)
                {
                    Firebase.Analytics.Parameter[] LTVParameters =
                    {
                new Firebase.Analytics.Parameter("value", advalue.Value/1000000f),
                new Firebase.Analytics.Parameter("precision", advalue.Precision.ToString()),
                new Firebase.Analytics.Parameter("ad_format", "app_open"),
                new Firebase.Analytics.Parameter("currency", advalue.CurrencyCode),
                new Firebase.Analytics.Parameter("ad_platform","googleadmob"),
                 new Firebase.Analytics.Parameter("ad_source", adSourceValue),

            };
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("paid_ad_impression", LTVParameters);
                }*/

                /*Dictionary<string, string> eventValues = new Dictionary<string, string>();
                eventValues.Add("event_revenue", $"{advalue.Value/1000000f}");
                eventValues.Add("event_revenue_currency", advalue.CurrencyCode.ToString());
                eventValues.Add("monetization_network", "googleadmob");
                eventValues.Add("mediation_network", adSourceValue);
                eventValues.Add("ad_type", "app_open");
                eventValues.Add("ad_unit", "app_open");
                AppsFlyer.sendEvent("af_revenue", eventValues);*/
            });
        }

        private void OnAppStateChanged(AppState state)
        {
            // Display the app open ad when the app is foregrounded.
            UnityEngine.Debug.Log("App State is " + state);
            if (state == AppState.Foreground)
            {
                ShowOpenAdIfAvailable();
            }
        }
        #endregion


        public bool IsLoaded(ADSTYPE type = ADSTYPE.Rewarded)
        {
            if (type == ADSTYPE.Interstitial)
            {
                return interstitial.IsLoaded();
            }
            return this.rewardedAd.IsLoaded();
        }

        public void ShowAds(UnityAction callback, ADSTYPE adType = ADSTYPE.Rewarded)
        {
            _showing = true;

            if (IsLoaded(adType))
            {
                if (adType == ADSTYPE.Rewarded)
                {
                    rewardAdDoneCallback = callback;

                    if (rewardedAd.IsLoaded())
                    {
                        rewardedAd.Show();
                        return;
                    }
                }
                else
                {
                    //day la quang cao xem ke, kiem tra xem da mua xoa quang cao chua
                    bool removeAds = PlayerPrefs.GetInt(REMOVE_AD, 0) == 1;
                    if (removeAds)
                    {
                        if (callback != null) callback();
                    }
                    else
                    {
                        rewardAdDoneCallback = callback;
                        //check qc gg
                        if (interstitial.IsLoaded())
                        {
                            //FirebaseManager.Instance.LogShow_Interstitial_Ads();
                            //FirebaseManager.Instance.LogShow_Banner_And_Interstitial_Ads();
                            interstitial.Show();
                            return;
                        }
                    }
                }
            }
            else
            {
#if UNITY_EDITOR
                callback?.Invoke();
#endif
            }
            //#endif
        }

        protected static string GetNetworkName(string fullNetworkName, string defaultNetwork = "admob")
        {
            if (string.IsNullOrEmpty(fullNetworkName))
                return defaultNetwork;
            var lower = fullNetworkName.ToLower();
            if (lower.Contains("admob"))
                return "admob";
            if (lower.Contains("max"))
                return "applovinmax";
            if (lower.Contains("fyber"))
                return "fyber";
            if (lower.Contains("appodeal"))
                return "appodeal";
            if (lower.Contains("inmobi"))
                return "inmobi";
            if (lower.Contains("vungle"))
                return "vungle";
            if (lower.Contains("admost"))
                return "admost";
            if (lower.Contains("topon"))
                return "topon";
            if (lower.Contains("tradplus"))
                return "tradplus";
            if (lower.Contains("chartboost"))
                return "chartboost";
            if (lower.Contains("appodeal"))
                return "appodeal";
            if (lower.Contains("google"))
                return "googleadmanager";
            if (lower.Contains("google"))
                return "googlead";
            if (lower.Contains("facebook") || lower.Contains("meta"))
                return "facebook";
            if (lower.Contains("applovin") || lower.Contains("max"))
                return "applovin";
            if (lower.Contains("ironsource"))
                return "ironsource";
            if (lower.Contains("unity"))
                return "unity";
            if (lower.Contains("mintegral"))
                return "mtg";
            return defaultNetwork;
        }
    }
}
