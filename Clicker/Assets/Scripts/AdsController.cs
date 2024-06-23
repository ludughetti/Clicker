using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsController : MonoBehaviour, IUnityAdsInitializationListener
{
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
        Debug.Log("Unity Ads initialization complete.");
        bannerController.Show();
        interstitialController.Initialize();
        rewardedAdController.Initialize();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads initialization failed: {error.ToString()} - {message}");
    }

    private void Awake()
    {
#if UNITY_IOS
        _gameId = "5643198";
#elif UNITY_ANDROID
        _gameId = "5643199";
#elif UNITY_EDITOR
        _gameId = "5643199";
#endif

        if(!Advertisement.isInitialized && Advertisement.isSupported)
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
