using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    public int itemCost;
    public Button buyButton;
    public GameObject panelToHide;
    public CanvasGroup buttonCanvasGroup;
    public Button buttonAds;
    public item classItem;
    public RewardedAdsButton rewardedAds;

    // set panelShop
    public Image imageItem;
    public Sprite[] spriteItems;
    public Text textItems;
    public string[] stringItems;

    private void Start()
    {
        UpdateButtonState();
    }

    private void OnEnable()
    {
        CoinManager.Instance.OnCoinsChanged += UpdateButtonState;
        UpdateButtonState();
    }

    private void OnDisable()
    {
        CoinManager.Instance.OnCoinsChanged -= UpdateButtonState;
    }

    public void TryPurchase()
    {
        if (CoinManager.Instance.CurrentCoins >= itemCost)
        {
            CoinManager.Instance.SpendCoins(itemCost);
            if (panelToHide != null)
                panelToHide.SetActive(false);
        }
    }

    private void UpdateButtonState()
    {
        bool canBuy = CoinManager.Instance.CurrentCoins >= itemCost;
        buyButton.interactable = canBuy;

        if (buttonCanvasGroup != null)
        {
            buttonCanvasGroup.alpha = canBuy ? 1f : 0.5f;
            buttonCanvasGroup.interactable = canBuy;
            buttonCanvasGroup.blocksRaycasts = canBuy;
        }
    }
    public void setPanelShop(int intImage)
    {
        imageItem.sprite = spriteItems[intImage - 1];
        textItems.text = stringItems[intImage - 1];
        buttonAds.onClick.RemoveAllListeners();
        buyButton.onClick.RemoveAllListeners();
        if (intImage == 1)
        {
            buttonAds.onClick.AddListener(() => classItem.OnWatchAdToRotation(rewardedAds));
            buyButton.onClick.AddListener(() => classItem.addRotate(1));
        }
        if (intImage == 2)
        {
            buttonAds.onClick.AddListener(() => classItem.OnWatchAdToDestroy(rewardedAds));
            buyButton.onClick.AddListener(() => classItem.addDestroy(1));
        }
        if (intImage == 3)
        {
            buttonAds.onClick.AddListener(() => classItem.OnWatchAdToBoom(rewardedAds));
            buyButton.onClick.AddListener(() => classItem.addBoom(1));
        }
        buttonAds.onClick.AddListener(() => hiddenPanel());
        buyButton.onClick.AddListener(() => hiddenPanel());
        buyButton.onClick.AddListener(() => TryPurchase());
        panelToHide.SetActive(true);
    }
    public void hiddenPanel()
    {
        panelToHide.SetActive(false);
    }
}
