using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lostGame : MonoBehaviour
{
    public GameObject panelGameOver;
    public Text scoreTextGameOver;
    public bool isLost = true;
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
        int score = PlayerPrefs.GetInt("Score", 0);
        scoreTextGameOver.text = "" + score;
        panelGameOver.SetActive(true);//hien thi thua
    }
}
