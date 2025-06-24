using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lostGame : MonoBehaviour
{
    public bool isLost = true;
    public GameObject panelGameOver;
    void OnEnable()
    {
        CheckLost(true);
    }
    public void CheckLost(bool isCheck)
    {
        StartCoroutine(startCheck(isCheck));
    }
    IEnumerator startCheck(bool isCheck)
    {
        isLost = true;
        yield return new WaitForSeconds(.01f);
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<DragDrop>().boolCheckLost();
            if (isLost)
            {
                isLost = child.gameObject.GetComponent<DragDrop>().boolCheckLost();
            }
        }
        if (isLost && isCheck)
        {
            GameOver();
        }
    }
    void GameOver()
    {
        //reset
        PlayerPrefs.SetInt("Score", 0);//diem
        PlayerPrefs.SetInt("IntAddItem", 0);//sao
        PlayerPrefs.SetInt("RotateCount", 0);//itemXoay
        PlayerPrefs.SetInt("DestroyCount", 0);//itemXoa
        PlayerPrefs.SetInt("BoomCount", 0);//itembom
        
        gameObject.GetComponent<SaveOnQuit>().ResetSaveData();
        panelGameOver.SetActive(true);
    }
}
