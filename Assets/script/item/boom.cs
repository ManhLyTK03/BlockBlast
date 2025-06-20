using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;


[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class boom : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject vienBoom;
    public GameObject panelBox;
    public GameObject panelGrid;
    public Transform canvasFall;
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Vector2 dragOffset;
    private Vector3 defaultScale;
    private Canvas canvas;
    private Coroutine holdCoroutine;
    public bool isGameOver = true;
    Color colorDefault = new Color(1f, 1f, 1f, 1f);
    private float rotationZ;

    private Dictionary<Transform, GameObject> nearestItemsPerChild = new Dictionary<Transform, GameObject>();// Danh sách lưu trữ gridItem gần nhất cho mỗi phần tử con

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        rotationZ = transform.rotation.eulerAngles.z;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        vienBoom.SetActive(true);
        originalPosition = rectTransform.anchoredPosition;
        ExecuteHoldAction(eventData);
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        resetChildren();
    }
    private void ExecuteHoldAction(PointerEventData eventData)
    {
        setChildren();

        // Đẩy object lên trên một chút
        rectTransform.anchoredPosition = rectTransform.anchoredPosition + new Vector2(0f, 500f);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPointerPosition
        );

        dragOffset = rectTransform.anchoredPosition - localPointerPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPointerPosition))
        {
            rectTransform.anchoredPosition = localPointerPosition + dragOffset;
        }
        FindNearestGridItems();
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        DropBox();
    }
    void setChildren()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        defaultScale = rectTransform.localScale;
        rectTransform.localScale = new Vector3(GridResizer.cellWidth / transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta.x, GridResizer.cellHeight / transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta.y, rectTransform.localScale.z);
    }
    void resetChildren()
    {
        rectTransform.localScale = defaultScale;
        rectTransform.anchoredPosition = originalPosition;
    }
    void FindNearestGridItems()
    {
        nearestItemsPerChild.Clear(); // Xóa dữ liệu cũ mỗi frame

        GameObject[] tickerItems = GameObject.FindGameObjectsWithTag("ticker");
        GameObject[] gridItems = GameObject.FindGameObjectsWithTag("gridItem");

        // Gộp 2 mảng lại thành 1 mảng mới
        GameObject[] allItems = new GameObject[tickerItems.Length + gridItems.Length];
        tickerItems.CopyTo(allItems, 0);
        gridItems.CopyTo(allItems, tickerItems.Length);

        RectTransform rectTransform = transform.GetChild(0).gameObject.GetComponent<RectTransform>();

        Vector3 imageCenter = rectTransform.position;
        float minEdgeDistance = rectTransform.rect.width * rectTransform.lossyScale.x;
        GameObject nearest = null;
        float nearestDistance = float.MaxValue;
        
        foreach (GameObject item in allItems)
        {
            float distance = Vector3.Distance(imageCenter, item.transform.position);
            if (distance <= nearestDistance && distance <= minEdgeDistance)
            {
                nearest = item;
                nearestDistance = distance;
            }
        }

        if (nearest != null)
        {
            nearestItemsPerChild[transform.GetChild(0)] = nearest;
            vienBoom.GetComponent<MatchBorderSize>().tamBoom = nearest.GetComponent<RectTransform>();
            foreach (Transform child in transform)
            {
                foreach (GameObject item in allItems)
                {
                    if (((transform.GetChild(0).position - child.position) == (nearest.transform.position - item.transform.position)) && !nearestItemsPerChild.ContainsValue(item))
                    {
                        nearestItemsPerChild[child] = item;
                    }
                }
            }
        }
        foreach (GameObject item in tickerItems)
        {
            if (nearestItemsPerChild.ContainsValue(item))
            {
                item.transform.GetChild(0).gameObject.GetComponent<Image>().color = colorDefault;
                item.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                item.transform.GetChild(0).gameObject.GetComponent<Image>().color = colorDefault*.8f;
            }
        }
    }
    void DropBox()
    {
        vienBoom.SetActive(false);
        int tickerCount = 0;

        foreach (var item in nearestItemsPerChild)
        {
            GameObject ticker = item.Value;
            if (ticker != null && ticker.CompareTag("ticker"))
            {
                tickerCount++;
            }
        }

        GameObject[] tickerItems = GameObject.FindGameObjectsWithTag("ticker");
        if (tickerCount > 0)
        {
            item.boomCount--;
            PlayerPrefs.SetInt("BoomCount", item.boomCount);
            panelBox.GetComponent<item>().itemBoom();
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(10 * tickerCount);
            }
            foreach (GameObject item in tickerItems)
            {
                if (nearestItemsPerChild.ContainsValue(item))
                {
                    // item.tag = "gridItem";
                    // item.transform.GetChild(0).gameObject.SetActive(false);
                    DuplicateImage(item.transform.GetChild(0).GetComponent<Image>());
                    item.tag = "gridItem";
                    item.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                    if (item.transform.childCount > 1)
                    {
                        Destroy(item.transform.GetChild(1).gameObject);
                        panelGrid.GetComponent<addItem>().setStar();
                    }
                }
                else
                {
                    item.transform.GetChild(0).gameObject.GetComponent<Image>().color = colorDefault;
                }
            }
        }
        foreach (GameObject item in tickerItems)
        {
            if (!nearestItemsPerChild.ContainsValue(item))
            {
                item.transform.GetChild(0).gameObject.GetComponent<Image>().color = colorDefault;
            }
        }
    }
    public void DuplicateImage(Image originalImage)
    {
        // Clone GameObject của image gốc, đặt parent và giữ local transform
        GameObject newGO = Instantiate(
            originalImage.gameObject,
            originalImage.transform.parent,
            false   // worldPositionStays = false để giữ nguyên localPosition, anchor, pivots, sizeDelta...
        );

        newGO.transform.SetParent(originalImage.transform);

        // Nếu cần thay đổi Sprite hoặc tweaking thêm:
        Image newImage = newGO.GetComponent<Image>();
        newImage.sprite = originalImage.sprite;

        //thêm vật lý 
        newGO.AddComponent<BoxCollider2D>();
        var rb2d = newGO.AddComponent<Rigidbody2D>();


        // 1. Sinh biên độ lực
        float forceMag = Random.Range(10f, 15f);

        // 2. Sinh góc ngẫu nhiên
        float result;
        if (Random.value < 0.5f)
        {
            // Khoảng âm: từ -30 đến -20
            result = Random.Range(-50f, -40f);
        }
        else
        {
            // Khoảng dương: từ 20 đến 30
            result = Random.Range(40f, 50f);
        }
        float angle = result;

        // 3. Tạo véc-tơ hướng lên rồi xoay theo góc
        Vector2 dir = Quaternion.Euler(0f, 0f, angle) * Vector2.up;

        // 4. Áp lực
        rb2d.AddForce(dir * forceMag, ForceMode2D.Impulse);
        // **Thêm xung lực xoắn để vật thể quay**
        float torqueMag = Random.Range(-0.01f, 0.01f);
        rb2d.AddTorque(torqueMag, ForceMode2D.Impulse);


        newGO.transform.SetParent(canvasFall);

        Destroy(newGO, 3f);
        
    }
}
