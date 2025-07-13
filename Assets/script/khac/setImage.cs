using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class setImage : MonoBehaviour
{
    public bool isWidth;
    private Image img;
    private RectTransform rt;
    public float targetWidth;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        img = GetComponent<Image>();
        rt = img.GetComponent<RectTransform>();

        if (isWidth)
        {
            targetWidth = rt.rect.width;

            // Lấy kích thước gốc của sprite (pixel)
            float originalWidth = img.sprite.rect.width;
            float originalHeight = img.sprite.rect.height;

            // Tính chiều cao mới để giữ tỉ lệ
            float newHeight = targetWidth * (originalHeight / originalWidth);

            // Áp dụng kích thước
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
        }
        else
        {
            // Lấy kích thước mục tiêu dựa trên chiều cao RectTransform
            float targetHeight = rt.rect.height;

            // Lấy kích thước gốc của sprite (pixel)
            float originalWidth = img.sprite.rect.width;
            float originalHeight = img.sprite.rect.height;

            // Tính chiều rộng mới để giữ tỉ lệ (aspect ratio)
            float newWidth = targetHeight * (originalWidth / originalHeight);

            // Áp dụng kích thước
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        }
    }
}
