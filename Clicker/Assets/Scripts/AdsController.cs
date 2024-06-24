using UnityEngine;
using UnityEngine.Advertisements;

public class AdsController : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string androidAdUnityId = "5643199";
    [SerializeField] string iOSAdUnityId = "5643198";

    [SerializeField] private GameController gameController;
    [SerializeField] private BannerController bannerController;
    [SerializeField] private InterstitialController interstitialController;
    [SerializeField] private RewardedAdController rewardedAdController;
    [SerializeField] private float timeToAddOnReward = 2f;

    private string _gameId;

    private void OnEnable()
    {
        rewardedAdController.OnRewardsAdWatched += GiveRewardOnAdWatched;
    }

    private void OnDisable()
    {
        rewardedAdController.OnRewardsAdWatched -= GiveRewardOnAdWatched;
    }

    public void OnInitializationComplete()
    {
#if UNITY_ANDROID
        Debug.Log("Unity Ads initialization complete.");
        bannerController.Show();
        interstitialController.Initialize();
        rewardedAdController.Initialize();
#endif
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads initialization failed: {error.ToString()} - {message}");
        gameObject.SetActive(false);
    }

    private void Awake()
    {
#if UNITY_IOS
        _gameId = iOSAdUnityId;
#elif UNITY_ANDROID
        _gameId = androidAdUnityId;
#else
        _gameId = androidAdUnityId;
        gameObject.SetActive(false);
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, true, this);
        }
    }

    public void ShowAdsAfterTurnEnded()
    {
        interstitialController.ShowInterstitial();
    }

    private void GiveRewardOnAdWatched()
    {
        gameController.AddTime(timeToAddOnReward);
    }
}