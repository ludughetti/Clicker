using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAdController : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidAdUnityId = "Rewarded_Android";
    [SerializeField] private string iOSAdUnityId = "Rewarded_iOS";

    public Action OnRewardsAdWatched = delegate { };

    private string _adUnityId = null;
    private bool _isAdLoaded = false;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_IOS
        _adUnityId = iOSAdUnityId;
#elif UNITY_ANDROID
        _adUnityId = androidAdUnityId;
#endif
    }

    public void Initialize()
    {
        Advertisement.Load(_adUnityId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        _isAdLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading ad unity: {_adUnityId}. {error.ToString()} - {message}");
    }

    public void ShowRewardedAd()
    {
        if(_isAdLoaded)
            Advertisement.Show(_adUnityId, this);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("RewardedAd show failed");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Showing rewarded ad");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Rewarded ad was clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Showing rewarded ad is completed");
        if(_adUnityId.Equals(placementId) && UnityAdsShowCompletionState.COMPLETED.Equals(showCompletionState))
        {
            OnRewardsAdWatched.Invoke();
        }
    }
}
