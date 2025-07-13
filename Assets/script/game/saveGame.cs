using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveOnQuit : MonoBehaviour
{
    public GameObject[] prefabBoxs;
    public Sprite[] imageGrids;
    public GameObject prefabObstacle;
    public GameObject prefabIce;
    public GameObject backGrid;
    private int[] intBoxs;
    private int[] angles;
    private int[] grids;
    private int[] gridObstacles;
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
        }
        if (grids == null || grids.Length == 0)
        {

        }
        else
        {
            setGrid();
        }
    }

    private void setGrid()
    {
        int randomImage = Random.Range(0, imageGrids.Length);
        for (int i = 0; i < backGrid.transform.childCount; i++)
        {
            backGrid.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            backGrid.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = imageGrids[randomImage];
        }
        // Gọi coroutine để ẩn các image có grids[i] == 0
        StartCoroutine(HideGridImages());
    }
    private IEnumerator HideGridImages()
    {
        for (int i = 0; i < backGrid.transform.childCount; i++)
        {
            if (grids[i] == 0)
            {
                yield return new WaitForSeconds(0.01f); // chờ 0.2 giây giữa các lần ẩn
                backGrid.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }
            else
            {
                if (gridObstacles[i] / 10 == 1)
                {
                    GameObject Obj = Instantiate(prefabObstacle, backGrid.transform.GetChild(i));
                    Obj.name = prefabObstacle.name;
                    Obj.GetComponent<obstacle>().activeObstacle(gridObstacles[i] % 10);
                }
                else if (gridObstacles[i] / 10 == 2)
                {
                    GameObject Obj = Instantiate(prefabIce, backGrid.transform.GetChild(i));
                    Obj.name = prefabIce.name;
                    Obj.GetComponent<Ice>().setIce(gridObstacles[i] % 10);
                }
                backGrid.transform.GetChild(i).tag = "ticker";
                backGrid.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = imageGrids[grids[i] - 1];
            }
        }
        backGrid.GetComponent<GridSpawner>().setUpStart();
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
        grids = new int[0];
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
        gridObstacles = new int[backGrid.transform.childCount];
        for (int i = 0; i < backGrid.transform.childCount; i++)
        {
            if (backGrid.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Image>().color.a == 1 && backGrid.transform.GetChild(i).tag == "ticker")
            {
                for (int y = 0; y < imageGrids.Length; y++)
                {
                    if (backGrid.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Image>().sprite.name == imageGrids[y].name)
                    {
                        grids[i] = y + 1;
                    }
                }
                if (backGrid.transform.GetChild(i).transform.childCount > 1)
                {
                    if (backGrid.transform.GetChild(i).GetChild(1).gameObject.name == "Ice")
                    {
                        gridObstacles[i] = 20 + backGrid.transform.GetChild(i).GetChild(1).gameObject.GetComponent<Ice>().intObstancle;
                    }
                    else if (backGrid.transform.GetChild(i).GetChild(1).gameObject.name == "obstacle")
                    {
                        gridObstacles[i] = 10 + backGrid.transform.GetChild(i).GetChild(1).gameObject.GetComponent<obstacle>().intObstancle;
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
        // **lưu độ dài gridObstacles**
        PlayerPrefs.SetInt("gridObstacles_Length", gridObstacles.Length);
        for (int i = 0; i < gridObstacles.Length; i++)
            PlayerPrefs.SetInt($"gridObstacles_{i}", gridObstacles[i]);

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
        // **load gridObstacles**
        int lengridObstacles = PlayerPrefs.GetInt("gridObstacles_Length", 0);
        if (lengridObstacles > 0)
        {
            gridObstacles = new int[lengridObstacles];
            for (int i = 0; i < lengridObstacles; i++)
                gridObstacles[i] = PlayerPrefs.GetInt($"gridObstacles_{i}", 0);
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

        // 3. Xoá PlayerPrefs cho gridObstacles
        int lengridObstacles = PlayerPrefs.GetInt("gridObstacles_Length", 0);
        for (int i = 0; i < lengridObstacles; i++)
            PlayerPrefs.DeleteKey($"gridObstacles_{i}");
        PlayerPrefs.DeleteKey("gridObstacles_Length");

        // 4. Lưu lại
        PlayerPrefs.Save();

        // 5. Reset mảng runtime
        intBoxs = new int[0];
        angles  = new int[0];
        grids   = new int[0];
        gridObstacles   = new int[0];
    }
}
