using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    public event System.Action OnCoinsChanged;

    public int CurrentCoins { get; private set; }
    public Text coinText;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCoins();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateCoinUI();
    }

    public void AddCoins(int amount)
    {
        CurrentCoins += amount;
        UpdateCoinUI();
        SaveCoins();
        OnCoinsChanged?.Invoke(); // thông báo
    }

    public void SpendCoins(int amount)
    {
        if (CurrentCoins >= amount)
        {
            CurrentCoins -= amount;
            UpdateCoinUI();
            SaveCoins();
            OnCoinsChanged?.Invoke(); // thông báo
        }
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "" + CurrentCoins;
        }
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt("Coins", CurrentCoins);
    }

    private void LoadCoins()
    {
        PlayerPrefs.SetInt("Coins", 99);
        CurrentCoins = PlayerPrefs.GetInt("Coins", 0);
    }
}
