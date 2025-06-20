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
        if (isLost&&isCheck)
        {
            PlayerPrefs.SetInt("IntAddItem", 0);
            panelGameOver.SetActive(true);
        }
    }
}
