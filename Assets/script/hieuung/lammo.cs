
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SetPanelTransparency : MonoBehaviour
{
    [Range(0f, 1f)]
    public float transparency = 0f; // Mặc định là 0 (ẩn)

    public float fadeDuration = 1f; // Thời gian fade (giây)

    private Coroutine currentFadeCoroutine;

    public void anPanel(Transform panel)
    {
        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
        currentFadeCoroutine = StartCoroutine(FadePanel(panel, 0f)); // Ẩn dần
    }

    public void hienPanel(Transform panel)
    {
        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
        currentFadeCoroutine = StartCoroutine(FadePanel(panel, 1f)); // Hiện dần
    }

    IEnumerator FadePanel(Transform parent, float targetAlpha)
    {
        // Lấy tất cả các thành phần cần thay đổi alpha
        var images = parent.GetComponentsInChildren<Image>(true);
        var texts = parent.GetComponentsInChildren<Text>(true);
        var tmpTexts = parent.GetComponentsInChildren<TMP_Text>(true);

        // Lưu alpha ban đầu
        float startAlpha = images.Length > 0 ? images[0].color.a : 1f;

        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / fadeDuration);

            foreach (var img in images)
            {
                if (img.tag != "other")
                {
                    Color c = img.color;
                    c.a = alpha;
                    img.color = c;
                }
            }

            foreach (var txt in texts)
            {
                Color c = txt.color;
                c.a = alpha;
                txt.color = c;
            }

            foreach (var tmp in tmpTexts)
            {
                Color c = tmp.color;
                c.a = alpha;
                tmp.color = c;
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo alpha cuối cùng là chính xác
        foreach (var img in images)
        {
            if (img.tag != "other")
            {
                Color c = img.color;
                c.a = targetAlpha;
                img.color = c;
            }
        }

        foreach (var txt in texts)
        {
            Color c = txt.color;
            c.a = targetAlpha;
            txt.color = c;
        }

        foreach (var tmp in tmpTexts)
        {
            Color c = tmp.color;
            c.a = targetAlpha;
            tmp.color = c;
        }

        transparency = targetAlpha;
    }
}
