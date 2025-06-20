using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton
    public Text scoreText;
    public Text highScoreText;

    private int score = 0;
    private int highScore = 0;

    // Thời gian hiệu ứng (giây)
    [SerializeField] private float countDuration = 0.5f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        scoreText.text = "0";
        highScoreText.text = highScore.ToString();
    }

    public void AddScore(int amount)
    {
        int oldScore = score;
        score += amount;

        // Cập nhật high score nếu cần
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            highScoreText.text = highScore.ToString();
        }

        // Chạy hiệu ứng đếm số
        StopAllCoroutines();  // dừng các coroutine cũ (nếu lần trước vẫn đang chạy)
        StartCoroutine(CountUp(oldScore, score, countDuration));
    }

    private IEnumerator CountUp(int from, int to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // Nội suy tuyến tính
            int current = Mathf.RoundToInt(Mathf.Lerp(from, to, elapsed / duration));
            scoreText.text = current.ToString();
            yield return null;
        }
        // Đảm bảo hiển thị đúng giá trị cuối
        scoreText.text = to.ToString();
    }
}
