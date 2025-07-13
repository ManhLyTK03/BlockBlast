using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ice : MonoBehaviour
{
    public int intObstancle;
    public Sprite[] imageIce;
    // Start is called before the first frame update
    void OnEnable()
    {
        intObstancle = Random.Range(1, imageIce.Length + 1);
        setIce(intObstancle);
    }
    public void setIce(int intIce)
    {
        intObstancle = intIce;
        gameObject.GetComponent<Image>().sprite = imageIce[intObstancle-1];
    }

    // Update is called once per frame
    public void setObstancle()
    {
        intObstancle--;
        if (intObstancle == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = imageIce[intObstancle - 1];
        }
    }
}
