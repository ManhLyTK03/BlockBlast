using UnityEngine;
using UnityEngine.UI;

public class MatchBorderSize : MonoBehaviour
{
    [SerializeField] private Image box;     // boom
    [SerializeField] private Image border;  // vien
    public RectTransform tamBoom;
    private RectTransform rtBox;
    private RectTransform rtBorder;
    private float borderBox = 10f;          // do day vien
    [SerializeField] float smoothTime = 0.1f; // khoảng thời gian trễ (giây)
    
    private Vector3 _velocity = Vector3.zero;

    void Start()
    {
        // Lấy RectTransform của box và border
        rtBox = box.rectTransform;
        rtBorder = gameObject.GetComponent<RectTransform>();
        tamBoom = rtBox;
    }
    void Update()
    {
        // Cách 1: Gán sizeDelta trực tiếp
        rtBorder.sizeDelta = rtBox.sizeDelta + new Vector2(borderBox, borderBox);
        rtBorder.localScale = rtBox.localScale;
        // Cập nhật vị trí viền = SmoothDamp(từ vị trí hiện tại hướng về vị trí box)
        rtBorder.position = Vector3.SmoothDamp(
            rtBorder.position,
            tamBoom.position,
            ref _velocity,
            smoothTime
        );
    }
}
