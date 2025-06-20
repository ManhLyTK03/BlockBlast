using UnityEngine;
using UnityEngine.UI;

// Bắt buộc thành phần phải có GridLayoutGroup để script hoạt động
[RequireComponent(typeof(GridLayoutGroup))]
public class GridResizer : MonoBehaviour
{
    public int rows = 8;       // Số hàng của lưới
    public int columns = 8;    // Số cột của lưới
    public static float cellWidth, cellHeight;

    private GridLayoutGroup grid;       // Tham chiếu đến GridLayoutGroup
    private RectTransform rt;           // Tham chiếu đến RectTransform của GameObject này

    void Start()
    {
        // Lấy các thành phần cần thiết
        grid = GetComponent<GridLayoutGroup>();
        rt = GetComponent<RectTransform>();

        ResizeCells(); // Cập nhật kích thước ô khi bắt đầu
    }

    void OnRectTransformDimensionsChange()
    {
        // Hàm này được gọi tự động khi RectTransform thay đổi kích thước
        ResizeCells();
    }

    // Hàm tính toán và cập nhật lại kích thước từng ô trong lưới
    void ResizeCells()
    {
        if (rt == null || grid == null) return; // Kiểm tra an toàn

        // Thay đổi chiều cao
        Vector2 size = rt.sizeDelta;
        size.y = rt.rect.width*columns/rows; // đặt chiều cao mới
        rt.sizeDelta = size;

        // Lấy kích thước thực tế của lưới (trừ đi padding)
        float width = rt.rect.width - grid.padding.left - grid.padding.right;
        float height = rt.rect.height - grid.padding.top - grid.padding.bottom;

        // Tính kích thước của từng ô sao cho vừa khít lưới
        cellWidth = width / columns;
        cellHeight = height / rows;

        // Cập nhật lại các thuộc tính của GridLayoutGroup
        grid.cellSize = new Vector2(cellWidth, cellHeight);
        grid.spacing = Vector2.zero;
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
    }
}
