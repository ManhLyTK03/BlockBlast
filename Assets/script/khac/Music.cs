using UnityEngine;
using UnityEngine.UI;

public class MusicGame : MonoBehaviour
{
    public static bool isRung;
    public static bool isMusic;
    public static bool isAmthanh;
    public GameObject buttonRung;
    public GameObject buttonMusic;
    public GameObject buttonAmthanh;

    void Start()
    {
        isRung = PlayerPrefs.GetInt("IsRung", 1) == 1;
        isMusic = PlayerPrefs.GetInt("IsMusic", 1) == 1;
        isAmthanh = PlayerPrefs.GetInt("IsAmthanh", 1) == 1;

        buttonRung.transform.GetChild(0).gameObject.SetActive(isRung);
        buttonRung.transform.GetChild(1).gameObject.SetActive(!isRung);
        buttonMusic.transform.GetChild(0).gameObject.SetActive(isMusic);
        buttonMusic.transform.GetChild(1).gameObject.SetActive(!isMusic);
        buttonAmthanh.transform.GetChild(0).gameObject.SetActive(isAmthanh);
        buttonAmthanh.transform.GetChild(1).gameObject.SetActive(!isAmthanh);
    }

    public static void Rung()
    {
        if (isRung)
        {
            Handheld.Vibrate();
        }
    }

    public void ClickRung()
    {
        isRung = !isRung;
        PlayerPrefs.SetInt("IsRung", isRung ? 1 : 0); // Lưu dưới dạng int
        PlayerPrefs.Save(); // Đảm bảo lưu ngay
        buttonRung.transform.GetChild(0).gameObject.SetActive(isRung);
        buttonRung.transform.GetChild(1).gameObject.SetActive(!isRung);
    }
}
