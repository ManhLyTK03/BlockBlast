using UnityEngine;
using System.Collections.Generic;
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
            }
        }
    }

    public void setUpStart()
    {
        // Kiểm tra từng hàng
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (gridObjects[row, col].tag != "ticker")
                {

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
        }

        lostGame.GetComponent<lostGame>().CheckLost(true);
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

        Color colorBack = Color.blue;
        // Gọi check thua nếu có ô bị xoá
        if (scoreGrid > 0)
        {
            // Xoá các ô đã được đánh dấu
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (toClear[row, col])
                    {
                        if (gridObjects[row, col].transform.childCount > 1)
                        {
                            if (gridObjects[row, col].transform.GetChild(1).gameObject.name == "Ice")
                            {
                                gridObjects[row, col].transform.GetChild(1).gameObject.GetComponent<Ice>().setObstancle();
                            }
                            else if (gridObjects[row, col].transform.GetChild(1).gameObject.name == "obstacle")
                            {
                                gameObject.GetComponent<addItem>().setStar();
                                Destroy(gridObjects[row, col].transform.GetChild(1).gameObject);
                            }
                        }
                        Image img = gridObjects[row, col].transform.GetChild(0).GetComponent<Image>();
                        img.transform.GetChild(0).gameObject.SetActive(false);
                        colorBack = img.transform.GetChild(0).GetComponent<Image>().color;
                        img.rectTransform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                        img.GetComponent<Canvas>().sortingOrder = 120;

                        if (gridObjects[row, col].transform.childCount > 1 && gridObjects[row, col].transform.GetChild(1).gameObject.name == "Ice" && gridObjects[row, col].transform.GetChild(1).gameObject.GetComponent<Ice>().intObstancle >= 0)
                        {

                        }
                        else
                        {
                            DuplicateImage(gridObjects[row, col].transform.GetChild(0).GetComponent<Image>());
                            gridObjects[row, col].tag = "gridItem";
                            gridObjects[row, col].transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                        }
                    }
                }
            }

            if (scoreGrid >= 2)
            {
                Color colorAlpha = new Color(colorBack.r, colorBack.g, 0.5f + (0.2f - scoreGrid / 5f));
                gameObject.GetComponent<ColorFade>().doiMau(colorBack,(float)scoreGrid/2f);
            }
            // Debug.Log(gameObject.GetComponent<ColorFade>().fadeDuration);
            lostGame.GetComponent<lostGame>().CheckLost(true);
            // Cộng điểm
            if (ScoreManager.Instance != null)
            {
                MusicGame.Rung();
                ScoreManager.Instance.AddScore(10 * scoreGrid * scoreGrid * rows);
            }
        }

        lostGame.GetComponent<SaveOnQuit>().isSave();

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
                    // Thay đổi scale
                    img.rectTransform.localScale = new Vector3(1f, 1f, 1f);
                    img.transform.GetChild(0).gameObject.SetActive(true);
                    SetImageColorByName(img);
                    img.GetComponent<Canvas>().sortingOrder = 140;
                }
                else
                {
                    img.sprite = originalSprites[row, col]; // Đổi lại sprite gốc
                    // đặt lại scale
                    img.rectTransform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                    img.transform.GetChild(0).gameObject.SetActive(false);
                    img.GetComponent<Canvas>().sortingOrder = 120;
                }
            }
        }
    }
    public void ContinueLost(GameObject panelLost)
    {
        panelLost.SetActive(false);
        // Bước 1: Tìm tất cả các grid có tag "ticker"
        List<Transform> tickerGrids = new List<Transform>();
        foreach (Transform grid in transform)
        {
            if (grid.tag == "ticker")
            {
                tickerGrids.Add(grid);
            }
        }

        // Bước 2: Shuffle danh sách ngẫu nhiên (Fisher-Yates)
        for (int i = 0; i < tickerGrids.Count; i++)
        {
            Transform temp = tickerGrids[i];
            int randomIndex = Random.Range(i, tickerGrids.Count);
            tickerGrids[i] = tickerGrids[randomIndex];
            tickerGrids[randomIndex] = temp;
        }

        // Bước 3: Xóa một nửa
        int halfCount = 2 * tickerGrids.Count / 3;
        for (int i = 0; i < halfCount; i++)
        {
            DuplicateImage(tickerGrids[i].GetChild(0).GetComponent<Image>());
            tickerGrids[i].gameObject.tag = "gridItem";
            tickerGrids[i].GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            if (tickerGrids[i].childCount > 1)
            {
                Destroy(tickerGrids[i].GetChild(1).gameObject);
                gameObject.GetComponent<addItem>().setStar();
            }
        }
        lostGame.GetComponent<lostGame>().CheckLost(true);
        lostGame.GetComponent<SaveOnQuit>().isSave();
    }


    public void DuplicateImage(Image originalImage)
    {
        // Clone GameObject của image gốc, đặt parent và giữ local transform
        GameObject newGO = Instantiate(
            originalImage.gameObject,
            originalImage.transform.parent,
            false   // worldPositionStays = false để giữ nguyên localPosition, anchor, pivots, sizeDelta...
        );
        // Nếu GameObject clone có component Canvascheck thì loại bỏ nó
        Canvas canvascheck = newGO.GetComponent<Canvas>();
        if (canvascheck != null)
        {
            DestroyImmediate(canvascheck);
        }

        newGO.transform.SetParent(originalImage.transform);

        // thay đổi Sprite
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

    
    public void SetImageColorByName(Image image)
    {
        if (image.sprite == null || string.IsNullOrEmpty(image.sprite.name))
        {
            Debug.LogWarning("Image is null or name is empty");
            return;
        }

        Color color;
        if (TryGetColorByName(image.sprite.name.ToLower(), out color))
        {
            image.transform.GetChild(0).GetComponent<Image>().color = color;
        }
        else
        {
            Debug.LogWarning($"No color found for name: {image.name}");
        }
    }
    private bool TryGetColorByName(string name, out Color color)
    {
        switch (name)
        {
            case "green":
                color = new Color(0.565f, 0.933f, 0.565f); // #90EE90
                return true;
            case "blue1":
                color = new Color(0.529f, 0.808f, 0.922f); // #87CEEB
                return true;
            case "yellow":
                color = new Color(1.0f, 0.973f, 0.690f); // #FFF8B0
                return true;
            case "blue2":
                color = new Color(0.118f, 0.565f, 1.0f); // #1E90FF
                return true;
            case "brown":
                color = new Color(0.871f, 0.722f, 0.529f); // #DEB887
                return true;
            case "orange":
                color = new Color(1.0f, 0.800f, 0.6f); // #FFCC99
                return true;
            case "pink":
                color = new Color(1.0f, 0.75f, 0.796f); // #FFC0CB
                return true;
            case "violet":
                color = new Color(0.855f, 0.439f, 0.839f); // #DA70D6
                return true;
            default:
                color = Color.white; // Default fallback
                return false;
        }
    }
}
