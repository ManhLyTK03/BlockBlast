using UnityEngine;
using UnityEngine.UI;

public class AutoRotate2D : MonoBehaviour
{
    [Tooltip("SpriteRenderer của GameObject cần điều chỉnh")]
    public SpriteRenderer spriteRenderer;


    [Tooltip("Tốc độ xoay theo độ mỗi giây (dương = quay thuận chiều kim đồng hồ)")]
    public float rotationSpeed = 90f;

    void Update()
    {
        // Xoay quanh trục Z, nhân với Time.deltaTime để mượt và độc lập với số khung hình
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
    public void MatchToImageLocal(GameObject image)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Image referenceImage = image.GetComponent<Image>();
        // 1. Panel là parent chung của cả 2
        RectTransform panelRT = referenceImage.rectTransform.parent as RectTransform;
        if (panelRT == null)
        {
            Debug.LogError("Image không có parent RectTransform!");
            return;
        }

        // 2. Lấy 4 góc của Image trong world space
        RectTransform imgRT = referenceImage.rectTransform;
        Vector3[] worldCorners = new Vector3[4];
        imgRT.GetWorldCorners(worldCorners);  
        //    [0] bottom-left, [2] top-right

        // 3. Chuyển các world-corners về local space của panel
        Vector3[] localCorners = new Vector3[4];
        for (int i = 0; i < 4; i++)
            localCorners[i] = panelRT.InverseTransformPoint(worldCorners[i]);

        // 4. Tính size và center trong panel-local
        Vector3 bl = localCorners[0];
        Vector3 tr = localCorners[2];
        Vector2 sizeLocal  = tr - bl;
        Vector3 centerLocal= (bl + tr) * 0.5f;
        // Giữ nguyên z-local của sprite (nếu cần)
        centerLocal.z = spriteRenderer.transform.localPosition.z;

        // 5. Tính scale dựa trên kích thước gốc của sprite (bounds.size là world-unit khi scale = 1)
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        Vector3 newScale = spriteRenderer.transform.localScale;
        newScale.x = sizeLocal.x / spriteSize.x;
        newScale.y = sizeLocal.y / spriteSize.y;
        // (giữ nguyên newScale.z nếu bạn có scale 3D)

        // 6. Gán vị trí và scale
        spriteRenderer.transform.localPosition = centerLocal;
        spriteRenderer.transform.localScale    = newScale;
    }
}
