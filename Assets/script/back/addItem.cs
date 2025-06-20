using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class addItem : MonoBehaviour
{
    public GameObject prefabObstacle;
    public GameObject panelAdd;
    public GameObject starObject;
    private int intCheck;
    public int activeObstacle = 5;
    public int TrueAdd = 8;
    private int intaddItem = 0;
    private GameObject[] gridObjects;
    private List<GameObject> validGrids = new List<GameObject>();
    IEnumerator Start()
    {
        intCheck = activeObstacle;
        for (int i = 0; i < TrueAdd; i++)
        {
            GameObject imgObj = new GameObject("Image_" + (i + 1));
            imgObj.transform.SetParent(panelAdd.transform, false); // false: giữ local scale
            Image img = imgObj.AddComponent<Image>();
            imgObj.GetComponent<RectTransform>().pivot = new Vector2(0f, imgObj.GetComponent<RectTransform>().pivot.y);
        }
        intaddItem = PlayerPrefs.GetInt("IntAddItem", 0);

        yield return new WaitForEndOfFrame();
        setPosition();
    }
    public void checkObstacle()
    {
        intCheck--;
        if (intCheck == 0)
        {
            intCheck = activeObstacle;
            addObstacle();
        }
    }

    void addObstacle()
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
            Instantiate(prefabObstacle, validGrids[randomIndex].transform);
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
        starObject.GetComponent<RectTransform>().position = panelAdd.transform.GetChild(intaddItem).GetComponent<RectTransform>().position;

        for (int i = 0; i < panelAdd.transform.childCount; i++)
        {
            Transform child = panelAdd.transform.GetChild(i);
            if (i < intaddItem)
            {
                child.gameObject.GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                child.gameObject.GetComponent<Image>().color = Color.white;
            }
        }
    }
}
