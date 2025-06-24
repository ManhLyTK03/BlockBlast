using UnityEngine;
using UnityEngine.UI;

public class SaveOnQuit : MonoBehaviour
{
    public GameObject[] prefabBoxs;
    public Sprite[] imageGrids;
    public GameObject backGrid;
    private int[] intBoxs;
    private int[] angles;
    int[] grids;
    public HorizontalLayoutGroup layoutGroup;

    private void Awake()
    {
        LoadArrays();
    }
    void Start()
    {
        layoutGroup = GetComponent<HorizontalLayoutGroup>();

        if (intBoxs == null || intBoxs.Length == 0)
        {

        }
        else
        {
            Spawn();
            intBoxs = new int[0];
            angles = new int[0];
        }
        if (grids == null || grids.Length == 0)
        {

        }
        else
        {
            setGrid();
            grids = new int[0];
        }
    }

    private void setGrid()
    {
        for (int i = 0; i < backGrid.transform.childCount; i++)
        {
            if (grids[i] == 0)
            {

            }
            else
            {
                backGrid.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                backGrid.transform.GetChild(i).tag = "ticker";
                backGrid.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                backGrid.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = imageGrids[grids[i] - 1];
            }
        }
        backGrid.GetComponent<GridSpawner>().CheckWinCondition();
    }

    private void Spawn()
    {
        if (transform.childCount == 0)
        {
            layoutGroup.enabled = true;

            for (int i = 0; i < intBoxs.Length; i++)
            {
                GameObject newImage;
                newImage = Instantiate(prefabBoxs[intBoxs[i]]);
                newImage.name = prefabBoxs[intBoxs[i]].name;

                newImage.transform.SetParent(transform, false);

                // Random góc xoay: 0, 90, 180 hoặc 270 độ
                int randomRotation = angles[i];

                newImage.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
                foreach (Transform child in newImage.transform)
                {
                    child.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            Invoke("offLayoutGroup", 0.1f);
        }
    }
    void offLayoutGroup()
    {
        layoutGroup.enabled = false;
    }
    public void isSave()
    {
        Invoke(nameof(SaveGame), .01f);
    }

    private void SaveGame()
    {
        intBoxs = new int[0];
        angles = new int[0];
        foreach (Transform child in transform)
        {
            for (int i = 0; i < prefabBoxs.Length; i++)
            {
                if (child.gameObject.name == prefabBoxs[i].name)
                {
                    AddElement(i, (int)child.GetComponent<DragDrop>().rotationZ);
                }
            }
        }
        grids = new int[backGrid.transform.childCount];
        for (int i = 0; i < backGrid.transform.childCount; i++)
        {
            if (backGrid.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Image>().color.a == 1)
            {
                for (int y = 0; y < imageGrids.Length; y++)
                {
                    if (backGrid.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Image>().sprite.name == imageGrids[y].name)
                    {
                        grids[i] = y + 1;
                    }
                }
            }
        }
        SaveArrays();
    }

    void AddElement(int newValue, int goc)
    {
        // Tăng kích thước mảng lên 1
        System.Array.Resize(ref intBoxs, intBoxs.Length + 1);
        // Gán giá trị mới vào cuối mảng
        intBoxs[intBoxs.Length - 1] = newValue;
        // Tăng kích thước mảng lên 1
        System.Array.Resize(ref angles, angles.Length + 1);
        // Gán giá trị mới vào cuối mảng
        angles[angles.Length - 1] = goc;
    }


    void SaveArrays()
    {
        // lưu độ dài intBoxs
        PlayerPrefs.SetInt("intBoxs_Length", intBoxs.Length);
        for (int i = 0; i < intBoxs.Length; i++)
            PlayerPrefs.SetInt($"intBoxs_{i}", intBoxs[i]);

        // lưu độ dài angles
        PlayerPrefs.SetInt("angles_Length", angles.Length);
        for (int i = 0; i < angles.Length; i++)
            PlayerPrefs.SetInt($"angles_{i}", angles[i]);

        // **lưu độ dài grids**
        PlayerPrefs.SetInt("grids_Length", grids.Length);
        for (int i = 0; i < grids.Length; i++)
            PlayerPrefs.SetInt($"grids_{i}", grids[i]);

        PlayerPrefs.Save();
    }

    void LoadArrays()
    {
        // load intBoxs
        int lenBoxs = PlayerPrefs.GetInt("intBoxs_Length", 0);
        if (lenBoxs > 0)
        {
            intBoxs = new int[lenBoxs];
            for (int i = 0; i < lenBoxs; i++)
                intBoxs[i] = PlayerPrefs.GetInt($"intBoxs_{i}", 0);
        }

        // load angles
        int lenAngles = PlayerPrefs.GetInt("angles_Length", 0);
        if (lenAngles > 0)
        {
            angles = new int[lenAngles];
            for (int i = 0; i < lenAngles; i++)
                angles[i] = PlayerPrefs.GetInt($"angles_{i}", 0);
        }

        // **load grids**
        int lenGrids = PlayerPrefs.GetInt("grids_Length", 0);
        if (lenGrids > 0)
        {
            grids = new int[lenGrids];
            for (int i = 0; i < lenGrids; i++)
                grids[i] = PlayerPrefs.GetInt($"grids_{i}", 0);
        }
    }

    // Hàm reset toàn bộ dữ liệu đã lưu và runtime
    public void ResetSaveData()
    {
        // 1. Xoá PlayerPrefs cho intBoxs
        int lenBoxs = PlayerPrefs.GetInt("intBoxs_Length", 0);
        for (int i = 0; i < lenBoxs; i++)
            PlayerPrefs.DeleteKey($"intBoxs_{i}");
        PlayerPrefs.DeleteKey("intBoxs_Length");

        // 2. Xoá PlayerPrefs cho angles
        int lenAngles = PlayerPrefs.GetInt("angles_Length", 0);
        for (int i = 0; i < lenAngles; i++)
            PlayerPrefs.DeleteKey($"angles_{i}");
        PlayerPrefs.DeleteKey("angles_Length");

        // 3. Xoá PlayerPrefs cho grids
        int lenGrids = PlayerPrefs.GetInt("grids_Length", 0);
        for (int i = 0; i < lenGrids; i++)
            PlayerPrefs.DeleteKey($"grids_{i}");
        PlayerPrefs.DeleteKey("grids_Length");

        // 4. Lưu lại
        PlayerPrefs.Save();

        // 5. Reset mảng runtime
        intBoxs = new int[0];
        angles  = new int[0];
        grids   = new int[0];
    }
}
