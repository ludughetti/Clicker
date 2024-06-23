using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialController : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string androidAdUnityId = "Interstitial_Android";
    [SerializeField] string iOSAdUnityId = "Interstitial_iOS";

    string _adUnityId = null;
    bool _isAdLoaded = false;

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

    public void ShowInterstitial()
    {
        if(_isAdLoaded)
            Advertisement.Show(_adUnityId, this);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("Interstitial show failed");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Showing interstitial");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Interstitial was clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Showing interstitial is completed");
        _isAdLoaded = false;
        Initialize();
    }
}
