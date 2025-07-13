using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class addItem : MonoBehaviour
{
    public GameObject prefabObstacle;
    public GameObject prefabIce;
    public GameObject panelAdd;
    public RectTransform panelStart;
    public GameObject starObject;
    private int intCheckObstacle;
    private int scoreIce = 10000;
    private int intCheckIce;
    private int activeObstacle = 5;
    private int activeIce = 10;
    public int TrueAdd = 8;
    public int intaddItem = 0;
    private GameObject[] gridObjects;
    private List<GameObject> validGrids = new List<GameObject>();
    IEnumerator Start()
    {
        intCheckObstacle = PlayerPrefs.GetInt("intCheckObstacle", 0);
        intCheckIce = PlayerPrefs.GetInt("intCheckIce", 0);
        intaddItem = PlayerPrefs.GetInt("IntAddItem", 0);

        yield return new WaitForEndOfFrame();
        setPosition();
    }
    public void checkObstacle()
    {
        intCheckObstacle++;
        PlayerPrefs.SetInt("intCheckObstacle", intCheckObstacle); // lưu lại
        if (intCheckObstacle == activeObstacle)
        {
            intCheckObstacle = 0;
            PlayerPrefs.SetInt("intCheckObstacle", intCheckObstacle); // cập nhật lại
            addObstacle(prefabObstacle);
        }
        int score = PlayerPrefs.GetInt("Score", 0);
        activeIce = 11 - score / scoreIce;
        if (activeIce < 5)
        {
            activeIce = 5;
        }
        if (score > scoreIce)
        {
            intCheckIce++;
            PlayerPrefs.SetInt("intCheckIce", intCheckIce); // lưu lại
            if (intCheckIce == activeIce)
            {
                intCheckIce = 0;
                PlayerPrefs.SetInt("intCheckIce", intCheckIce); // cập nhật lại
                addObstacle(prefabIce);
            }
        }
    }

    void addObstacle(GameObject prefab)
    {
        gridObjects = GameObject.FindGameObjectsWithTag("ticker");
        validGrids.Clear(); // Đảm bảo danh sách trống trước khi thêm

        // Lặp qua tất cả gridObjects và chọn những cái có ít hơn 2 con
        foreach (GameObject obj in gridObjects)
        {
            if (obj.transform.childCount < 2)
            {
                validGrids.Add(obj);
            }
        }

        // Nếu có ít nhất một GameObject thỏa mãn, chọn ngẫu nhiên một cái để gán vật cản
        if (validGrids.Count > 0)
        {
            int randomIndex = Random.Range(0, validGrids.Count);
            GameObject Obj = Instantiate(prefab, validGrids[randomIndex].transform);
            Obj.name = prefab.name;
        }
        else
        {
            Debug.Log("Không có GameObject nào có ít hơn 2 con.");
        }
    }

    public void setStar()
    {
        intaddItem++;
        if (intaddItem == TrueAdd)
        {
            intaddItem = 0;
            int intRandom = Random.Range(1, 4);
            if (intRandom == 1)
            {
                PlayerPrefs.SetInt("RotateCount", PlayerPrefs.GetInt("RotateCount", 0) + 1);
            }
            if (intRandom == 2)
            {
                PlayerPrefs.SetInt("DestroyCount", PlayerPrefs.GetInt("DestroyCount", 0) + 1);
            }
            if (intRandom == 3)
            {
                PlayerPrefs.SetInt("BoomCount", PlayerPrefs.GetInt("BoomCount", 0) + 1);
            }
        }
        PlayerPrefs.SetInt("IntAddItem", intaddItem);
        setPosition();
    }
    void setPosition()
    {
        // panelStart.rect.width = (panelAdd.GetComponent<RectTransform>().rect.width/TrueAdd)*intaddItem;
        float progress = (float)intaddItem / (float)TrueAdd;
        panelStart.gameObject.GetComponent<Image>().fillAmount = Mathf.Clamp01(progress);

        RectTransform rt = panelStart.transform.GetChild(1).GetComponent<RectTransform>();
        //thay đổi điểm neo
        Vector2 anchorMin = rt.anchorMin;
        Vector2 anchorMax = rt.anchorMax;
        anchorMin.x = progress;
        anchorMax.x = progress;
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        //đặt lại vị trí về điểm neo
        Vector3 pos = rt.anchoredPosition;
        pos.x = 0f; // Giá trị X bạn muốn đặt
        rt.anchoredPosition = pos;

    }
}
 