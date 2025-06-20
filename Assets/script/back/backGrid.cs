using UnityEngine;
using UnityEngine.UI;

public class GridSpawner : MonoBehaviour
{
    public GameObject imagePrefab; // Drag prefab here
    public GameObject lostGame;
    public Transform canvas;
    public int rows = 8;
    public int columns = 8;

    private Sprite[,] originalSprites;
    private GameObject[,] gridObjects;

    void Start()
    {
        originalSprites = new Sprite[rows, columns];
        gridObjects = new GameObject[rows, columns];
        SpawnGrid();
    }

    void SpawnGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject cell = Instantiate(imagePrefab, transform);
                cell.name = $"Cell_{row}_{col}";
                gridObjects[row, col] = cell;

                // Tính toán màu trắng hoặc xám
                bool isBlack = (row + col) % 2 == 0;
                Image img = cell.GetComponent<Image>();
                if (img != null)
                {
                    img.color = isBlack ? Color.black : new Color32(100, 100, 100, 255);
                }
            }
        }
    }

    public void CheckWinCondition()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<obstacle>().setObstancle();
        }

        int scoreGrid = 0;
        bool[,] toClear = new bool[rows, columns];

        // Kiểm tra từng hàng
        for (int row = 0; row < rows; row++)
        {
            bool rowWin = true;
            for (int col = 0; col < columns; col++)
            {
                if (gridObjects[row, col].tag != "ticker")
                {
                    rowWin = false;
                }
                else
                {
                    Image img = gridObjects[row, col].transform.GetChild(0).GetComponent<Image>();
                    if (img != null)
                    {
                        originalSprites[row, col] = img.sprite; // Lưu sprite gốc
                    }
                }
            }
            if (rowWin)
            {
                scoreGrid++;
                for (int col = 0; col < columns; col++)
                {
                    toClear[row, col] = true;
                }
            }
        }

        // Kiểm tra từng cột
        for (int col = 0; col < columns; col++)
        {
            bool colWin = true;
            for (int row = 0; row < rows; row++)
            {
                if (gridObjects[row, col].tag != "ticker")
                {
                    colWin = false;
                    break;
                }
            }
            if (colWin)
            {
                scoreGrid++;
                for (int row = 0; row < rows; row++)
                {
                    toClear[row, col] = true;
                }
            }
        }

        // Xoá các ô đã được đánh dấu
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (toClear[row, col])
                {
                    DuplicateImage(gridObjects[row, col].transform.GetChild(0).GetComponent<Image>());
                    gridObjects[row, col].tag = "gridItem";
                    // gridObjects[row, col].transform.GetChild(0).gameObject.SetActive(false);
                    gridObjects[row, col].transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                    if (gridObjects[row, col].transform.childCount > 1)
                    {
                        Destroy(gridObjects[row, col].transform.GetChild(1).gameObject);
                        gameObject.GetComponent<addItem>().setStar();
                    }
                }
            }
        }

        // Gọi check thua nếu có ô bị xoá
        if (scoreGrid > 0)
        {
            lostGame.GetComponent<lostGame>().CheckLost(true);
            // Cộng điểm
            if (ScoreManager.Instance != null)
            {
                MusicGame.Rung();
                ScoreManager.Instance.AddScore(10 * scoreGrid * scoreGrid * rows);
            }
        }

    }
    public void checkWin(Sprite newSprite)
    {
        bool[,] toWin = new bool[rows, columns];

        // Kiểm tra từng hàng
        for (int row = 0; row < rows; row++)
        {
            bool rowWin = true;
            for (int col = 0; col < columns; col++)
            {
                if (gridObjects[row, col].tag != "ticker" && gridObjects[row, col].transform.GetChild(0).tag != "boolTicker")
                {
                    rowWin = false;
                    break;
                }
            }
            if (rowWin)
            {
                for (int col = 0; col < columns; col++)
                {
                    toWin[row, col] = true;
                }
            }
        }

        // Kiểm tra từng cột
        for (int col = 0; col < columns; col++)
        {
            bool colWin = true;
            for (int row = 0; row < rows; row++)
            {
                if (gridObjects[row, col].tag != "ticker" && gridObjects[row, col].transform.GetChild(0).tag != "boolTicker")
                {
                    colWin = false;
                    break;
                }
            }
            if (colWin)
            {
                for (int row = 0; row < rows; row++)
                {
                    toWin[row, col] = true;
                }
            }
        }

        // Xoá các ô đã được đánh dấu
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Image img = gridObjects[row, col].transform.GetChild(0).GetComponent<Image>();
                if (toWin[row, col])
                {
                    img.sprite = newSprite;
                }
                else
                {
                    img.sprite = originalSprites[row, col]; // Đổi lại sprite gốc
                }
            }
        }
    }


    public void DuplicateImage(Image originalImage)
    {
        // Clone GameObject của image gốc, đặt parent và giữ local transform
        GameObject newGO = Instantiate(
            originalImage.gameObject,
            originalImage.transform.parent,
            false   // worldPositionStays = false để giữ nguyên localPosition, anchor, pivots, sizeDelta...
        );

        newGO.transform.SetParent(originalImage.transform);

        // Nếu cần thay đổi Sprite hoặc tweaking thêm:
        Image newImage = newGO.GetComponent<Image>();
        newImage.sprite = originalImage.sprite;

        //thêm vật lý 
        newGO.AddComponent<BoxCollider2D>();
        var rb2d = newGO.AddComponent<Rigidbody2D>();


        // Sinh biên độ lực
        float forceMag = Random.Range(7f, 12f);

        // Sinh góc ngẫu nhiên để tạo hướng
        float angle = Random.Range(-15f, 15f);
        Vector2 dir = Quaternion.Euler(0f, 0f, angle) * Vector2.up;

        // Áp lực chính
        rb2d.AddForce(dir * forceMag, ForceMode2D.Impulse);

        // **Thêm xung lực xoắn để vật thể quay**
        float torqueMag = Random.Range(-0.01f, 0.01f);
        rb2d.AddTorque(torqueMag, ForceMode2D.Impulse);

        // Tùy chỉnh trọng lực và drag
        rb2d.gravityScale = 3f;
        rb2d.drag = 0f;

        newGO.transform.SetParent(canvas);


        Destroy(newGO, 3f);
        
        // GameObject instance = Instantiate(prefabImageWin, originalImage.gameObject.transform.position, Quaternion.identity);
        // instance.transform.SetParent(originalImage.transform);
    }
}
