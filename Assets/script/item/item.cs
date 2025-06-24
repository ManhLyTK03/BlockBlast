using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class item : MonoBehaviour
{
    public Text textXoay;
    public Text textDestroy;
    public Text textBoom;
    public GameObject panelXoay;
    public GameObject panelBlock;
    public GameObject objectBoom;
    public static bool isXoay = false;
    public static int rotateCount = 0;
    public static int destroyCount = 0;
    public static int boomCount = 0;
    private bool isBoom = false;
    void Start()
    {
        rotateCount = PlayerPrefs.GetInt("RotateCount", 0);
        destroyCount = PlayerPrefs.GetInt("DestroyCount", 0);
        boomCount = PlayerPrefs.GetInt("BoomCount", 0);
    }
    void Update()
    {
        if (textXoay.text != PlayerPrefs.GetInt("RotateCount", 0).ToString())
        {
            rotateCount = PlayerPrefs.GetInt("RotateCount", 0);
            textXoay.text = rotateCount.ToString();
        }
        if (textDestroy.text != PlayerPrefs.GetInt("DestroyCount", 0).ToString())
        {
            destroyCount = PlayerPrefs.GetInt("DestroyCount", 0);
            textDestroy.text = destroyCount.ToString();
        }
        if (textBoom.text != PlayerPrefs.GetInt("BoomCount", 0).ToString())
        {
            boomCount = PlayerPrefs.GetInt("BoomCount", 0);
            textBoom.text = boomCount.ToString();
        }
    }
    public void itemXoay()
    {
        if (isXoay)
        {
            isXoay = false;
            foreach (Transform child in transform)
            {
                if (child.transform.localEulerAngles.z != child.GetComponent<DragDrop>().rotationZ)
                {
                    child.transform.rotation = Quaternion.Euler(0, 0, child.GetComponent<DragDrop>().rotationZ);
                    foreach (Transform child1 in child.transform)
                    {
                        child1.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                }
            }
            for (int i = 0; i < panelBlock.transform.childCount; i++)
            {
                panelXoay.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            if (rotateCount > 0)
            {
                isXoay = true;
                for (int i = 0; i < panelBlock.transform.childCount; i++)
                {
                    panelXoay.transform.GetChild(i).gameObject.SetActive(true);
                    panelXoay.transform.GetChild(i).gameObject.GetComponent<AutoRotate2D>().MatchToImageLocal(panelBlock.transform.GetChild(i).gameObject);
                }
            }
        }
    }
    public void itemDestroy()
    {
        if (destroyCount > 0)
        {
            destroyCount--;
            PlayerPrefs.SetInt("DestroyCount", destroyCount);
            foreach (Transform child in panelBlock.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
    public void itemBoom()
    {
        if (!isBoom && boomCount > 0)
        {
            isBoom = true;
            panelBlock.SetActive(false);
            objectBoom.SetActive(true);
        }
        else
        {
            isBoom = false;
            panelBlock.SetActive(true);
            objectBoom.SetActive(false);
        }
    }
    public void addRotate()
    {
        PlayerPrefs.SetInt("RotateCount", PlayerPrefs.GetInt("RotateCount", 0) + 1);
    }
    public void addDestroy()
    {
        PlayerPrefs.SetInt("DestroyCount", PlayerPrefs.GetInt("DestroyCount", 0) + 1);
    }
    public void addBoom()
    {
        PlayerPrefs.SetInt("BoomCount", PlayerPrefs.GetInt("BoomCount", 0) + 1);
    }
    public void hiddenRotation(int intBlock,bool hidden)
    {
        panelXoay.transform.GetChild(intBlock).gameObject.SetActive(hidden);
    }

}
