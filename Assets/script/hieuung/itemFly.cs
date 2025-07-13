using UnityEngine;
using UnityEngine.UI;

public class ImageCloneEffect : MonoBehaviour
{
    public Transform cloneParentOutsidePanel;
    public Image[] originalImage;           // Ảnh gốc cần clone
    public float moveUpDistance = 100f;   // Khoảng cách bay lên
    public float duration = 1f;           // Thời gian hiệu ứng
    private GameObject gameObjectClone;

    void Start()
    {
        //PlayEffect();
    }

    public void PlayEffect(int intPhanthuong)
    {
        // Clone ảnh
        Image clone = Instantiate(originalImage[intPhanthuong], originalImage[intPhanthuong].transform.parent);
        clone.transform.SetAsLastSibling();
        gameObjectClone = clone.gameObject;

        // Loại bỏ script khỏi clone nếu nó tồn tại
        ImageCloneEffect effectScript = clone.GetComponent<ImageCloneEffect>();
        if (effectScript != null)
        {
            Destroy(effectScript); // Ngăn clone tiếp tục tự tạo nữa
        }

        StartCoroutine(FlyAndFade(clone));
    }

    private System.Collections.IEnumerator FlyAndFade(Image img)
    {
        RectTransform rt = img.GetComponent<RectTransform>();

        Vector3 worldStartPos = rt.position;
        Vector3 localUp = rt.up;
        Vector3 worldEndPos = worldStartPos + localUp * moveUpDistance;

        Vector3 startScale = rt.localScale;
        Vector3 endScale = startScale * 5f;

        Color startColor = img.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);

        float elapsed = 0;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            rt.position = Vector3.Lerp(worldStartPos, worldEndPos, t);
            rt.localScale = Vector3.Lerp(startScale, endScale, t);
            img.color = Color.Lerp(startColor, endColor, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo trạng thái cuối
        rt.position = worldEndPos;
        rt.localScale = endScale;
        img.color = endColor;

        Destroy(img.gameObject); // Xoá clone sau hiệu ứng
    }
    void OnDisable()
    {
        Destroy(gameObjectClone); // Xoá clone 
    }

}
