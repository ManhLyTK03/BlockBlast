using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = false;
    public Text text;
    private string _gameId;

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
#if UNITY_IOS
        _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
#elif UNITY_EDITOR
        _gameId = _androidGameId;
#endif
        text.text = "0:" + _androidGameId;

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
            text.text = "1:" + _androidGameId;
        }
        else
        {
            text.text = "2:" + _androidGameId;
        }
    }

    public void OnInitializationComplete()
    {
        text.text = "3:" + _testMode;
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        text.text = $"Unity Ads Initialization Failed: {error} - {message}";
        Debug.LogError($"Unity Ads Initialization Failed: {error} - {message}");
    }
}
