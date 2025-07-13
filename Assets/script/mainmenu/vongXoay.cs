using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpinWheel : MonoBehaviour
{
    public GameObject wheel;
    public Button spinButton;
    public int itemCount = 8;

    private bool isSpinning = false;
    private bool canSpinToday = true;
    private float[] prizeAngles;

    void Start()
    {
        spinButton.onClick.AddListener(Spin);
        InitAngles();

        if (HasSpunToday())
        {
            canSpinToday = false;
        }
    }

    void InitAngles()
    {
        prizeAngles = new float[itemCount];
        float segmentAngle = 360f / itemCount;
        for (int i = 0; i < itemCount; i++)
        {
            prizeAngles[i] = i * segmentAngle;
        }
    }

    public void Spin()
    {
        if (!isSpinning && canSpinToday)
        {
            StartCoroutine(SpinTheWheel());
        }
    }

    IEnumerator SpinTheWheel()
    {
        isSpinning = true;

        int index = Random.Range(0, itemCount);
        float finalAngle = prizeAngles[index];
        float totalRotation = 360f * 5 + finalAngle;

        float duration = 5f;
        float elapsed = 0f;

        float startAngle = wheel.transform.eulerAngles.z;
        float endAngle = -totalRotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = 1 - Mathf.Pow(1 - t, 4);
            float currentAngle = Mathf.Lerp(startAngle, endAngle, easedT);
            wheel.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            yield return null;
        }

        wheel.transform.rotation = Quaternion.Euler(0, 0, endAngle);

        addItemSpin(index);
        gameObject.GetComponent<ImageCloneEffect>().PlayEffect(index);

        isSpinning = false;
        canSpinToday = false;

        PlayerPrefs.SetString("LastSpinDate", System.DateTime.Now.ToString("yyyy-MM-dd"));
        PlayerPrefs.Save();
    }

    void addItemSpin(int intItem)
    {
        switch (intItem)
        {
            case 0: CoinManager.Instance.AddCoins(10); break;
            case 1: addBoom(1); break;
            case 2: CoinManager.Instance.AddCoins(2); break;
            case 3: addRotate(1); break;
            case 4: CoinManager.Instance.AddCoins(5); break;
            case 5: addDestroy(1); break;
            case 6: CoinManager.Instance.AddCoins(4); break;
            case 7: CoinManager.Instance.AddCoins(1); break;
            default: break;
        }
    }
    public void addRotate(int intAdd)
    {
        PlayerPrefs.SetInt("RotateCount", PlayerPrefs.GetInt("RotateCount", 0) + intAdd);
    }
    public void addDestroy(int intAdd)
    {
        PlayerPrefs.SetInt("DestroyCount", PlayerPrefs.GetInt("DestroyCount", 0) + intAdd);
    }
    public void addBoom(int intAdd)
    {
        PlayerPrefs.SetInt("BoomCount", PlayerPrefs.GetInt("BoomCount", 0) + intAdd);
    }

    bool HasSpunToday()
    {
        string lastSpinDate = PlayerPrefs.GetString("LastSpinDate", "");
        string today = System.DateTime.Now.ToString("yyyy-MM-dd");
        return lastSpinDate == today;
    }

    public void OnWatchAdToSpin(RewardedAdsButton rewardedAds)
    {
        if (!canSpinToday)
        {
            rewardedAds.ShowAd(() => {
                AdsSpin();
            });
        }
    }

    public void AdsSpin()
    {
        canSpinToday = true;
    }
}
