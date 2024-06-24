using UnityEngine;
using UnityEngine.Advertisements;

public class BannerController : MonoBehaviour
{
    [SerializeField] string androidAdUnityId = "Banner_Android";
    [SerializeField] string iOSAdUnityId = "Banner_iOS";

    string _adUnityId = null;

    private void Start()
    {
#if UNITY_IOS
    _adUnityId = iOSAdUnityId;
#elif UNITY_ANDROID
        _adUnityId = androidAdUnityId;
#else
        _adUnityId = androidAdUnityId;
        gameObject.SetActive(false);
#endif
    }

    public void Show()
    {
        BannerLoadOptions loadOptions = new() 
        { 
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load(_adUnityId, loadOptions);
    }

    private void OnBannerLoaded()
    {
        Advertisement.Banner.Show(_adUnityId);
    }

    private void OnBannerError(string message)
    {
        Debug.Log($"Banner error: {message}");
    }
}
