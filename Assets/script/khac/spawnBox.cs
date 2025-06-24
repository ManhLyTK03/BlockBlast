using UnityEngine;
using UnityEngine.UI;

public class BoxSpawner : MonoBehaviour
{
    public GameObject[] imagePrefab1;  // Prefab thap
    public GameObject[] imagePrefab2;  // Prefab cao
    public int numberOfImages = 3;  // Số lượng Image muốn tạo
    public HorizontalLayoutGroup layoutGroup;
    void Start()
    {
        // Set FPS
        QualitySettings.vSyncCount = 0;
        // Lấy từ refreshRateRatio để ra đúng fps thực của màn hình
        var rr = Screen.currentResolution.refreshRateRatio;
        int fpsCap = Mathf.RoundToInt((float)rr.numerator / rr.denominator);
        Application.targetFrameRate = fpsCap;

        layoutGroup = GetComponent<HorizontalLayoutGroup>();
    }
    void Update()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (transform.childCount == 0)
        {
            layoutGroup.enabled = true;

            for (int i = 0; i < numberOfImages; i++)
            {
                int randomCheck = Random.Range(0, 100);
                GameObject newImage;
                if (randomCheck > 95)
                {
                    int randomIndex = Random.Range(0, imagePrefab1.Length);
                    newImage = Instantiate(imagePrefab1[randomIndex]);
                    newImage.name = imagePrefab1[randomIndex].name;
                }
                else
                {
                    int randomIndex = Random.Range(0, imagePrefab2.Length);
                    newImage = Instantiate(imagePrefab2[randomIndex]);
                    newImage.name = imagePrefab2[randomIndex].name;
                }

                newImage.transform.SetParent(transform, false);

                // Random góc xoay: 0, 90, 180 hoặc 270 độ
                int randomRotation = 90 * Random.Range(0, 4);

                newImage.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
                foreach (Transform child in newImage.transform)
                {
                    child.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            Invoke("offLayoutGroup", .1f);
        }
    }
    void offLayoutGroup()
    {
        layoutGroup.enabled = false;
    }
}
