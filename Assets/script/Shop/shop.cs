using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shop : MonoBehaviour
{
    public GameObject panelShopADS;
    public Button buttonCoin;
    public Button buttonADS;
    public Sprite spriteButtonTrue;
    public Sprite spriteButtonFalse;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void chuyenShop(bool isChoice)
    {
        if (isChoice)
        {
            buttonADS.GetComponent<Image>().sprite = spriteButtonTrue;
            buttonCoin.GetComponent<Image>().sprite = spriteButtonFalse;
        }
        else
        {
            buttonADS.GetComponent<Image>().sprite = spriteButtonFalse;
            buttonCoin.GetComponent<Image>().sprite = spriteButtonTrue;
        }
        panelShopADS.SetActive(isChoice);
    }
    public void OnWatchAdToAddRotation(RewardedAdsButton rewardedAds)
    {
        rewardedAds.ShowAd(() =>
        {
            PlayerPrefs.SetInt("RotateCount", PlayerPrefs.GetInt("RotateCount", 0) + 2);
        });
    }
    public void OnWatchAdToAddDestroy(RewardedAdsButton rewardedAds)
    {
        rewardedAds.ShowAd(() =>
        {
            PlayerPrefs.SetInt("DestroyCount", PlayerPrefs.GetInt("DestroyCount", 0) + 2);
        });
    }
    public void OnWatchAdToAddBoom(RewardedAdsButton rewardedAds)
    {
        rewardedAds.ShowAd(() => {
            PlayerPrefs.SetInt("BoomCount", PlayerPrefs.GetInt("BoomCount", 0) + 2);
        });
    }
}
