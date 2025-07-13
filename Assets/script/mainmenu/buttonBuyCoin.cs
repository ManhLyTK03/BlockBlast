using UnityEngine;
using UnityEngine.UI;

public class PurchaseButton : MonoBehaviour
{
    public int price; // Giá của vật phẩm
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        CoinManager.Instance.OnCoinsChanged += UpdateButtonState; // Lắng nghe sự kiện thay đổi coin
        UpdateButtonState();
    }

    private void OnDestroy()
    {
        CoinManager.Instance.OnCoinsChanged -= UpdateButtonState;
    }

    private void UpdateButtonState()
    {
        if (CoinManager.Instance.CurrentCoins >= price)
        {
            button.interactable = true;
            // Optional: đổi lại màu nút nếu muốn
        }
        else
        {
            button.interactable = false;
            // Optional: đổi màu để làm nút "tối đi"
        }
    }
}
