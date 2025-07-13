using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class obstacle : MonoBehaviour
{
    public int intObstancle;
    // Start is called before the first frame update
    void OnEnable()
    {
        intObstancle = Random.Range(3, 9);
        activeObstacle(intObstancle);
    }
    public void activeObstacle(int intBlock)
    {
        intObstancle = intBlock;
        transform.GetChild(0).gameObject.GetComponent<Text>().text = "" + intBlock;
    }

    // Update is called once per frame
    public void setObstancle()
    {
        intObstancle--;
        transform.GetChild(0).gameObject.GetComponent<Text>().text = "" + intObstancle;
        if (intObstancle == 0)
        {
            Destroy(gameObject);
        }
    }
}
