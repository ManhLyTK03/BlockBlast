
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;

public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null;

    private Action _onAdSuccessCallback; // 👈 Callback để gọi sau khi xem xong

    void Awake()
    {
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif
        _showAdButton.interactable = false;
    }

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        LoadAd();
    }

    public void LoadAd()
    {
        Advertisement.Load(_adUnitId, this);
    }

    public void ShowAd(Action onSuccessCallback)
    {
        _onAdSuccessCallback = onSuccessCallback;
        _showAdButton.interactable = false;
        Advertisement.Show(_adUnitId, this);
        LoadAd(); // Tải tiếp quảng cáo
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (adUnitId.Equals(_adUnitId))
        {
            // _showAdButton.onClick.AddListener(() => ShowAd(_onAdSuccessCallback));
            _showAdButton.interactable = true;
        }
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Ad completed, executing callback!");
            _onAdSuccessCallback?.Invoke(); // 👈 Gọi hàm callback
        }
    }

    // Error Handlers
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Load Failed: {adUnitId} - {error}: {message}");
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Show Failed: {adUnitId} - {error}: {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        _showAdButton.onClick.RemoveAllListeners();
    }
}
