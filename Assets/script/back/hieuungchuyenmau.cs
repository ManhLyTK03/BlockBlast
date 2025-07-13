using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorFade : MonoBehaviour
{
    public Image image;              // Image cần áp dụng hiệu ứng

    public void doiMau(Color color,float time)
    {
        StartCoroutine(FadeToWhiteTransparent(color,time));
    }

    IEnumerator FadeToWhiteTransparent(Color color,float time)
    {
        // Bắt đầu với màu đỏ đậm, không trong suốt
        Color startColor = color;
        Color endColor = new Color(1f, 1f, 1f, 0f);   // trắng, alpha = 0

        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / time;
            image.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        image.color = endColor; // Đảm bảo màu cuối cùng là trắng trong suốt
    }
}
