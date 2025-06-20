using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;


[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Vector2 dragOffset;
    private Vector3 defaultScale;
    private Canvas canvas;
    private Coroutine holdCoroutine;
    private bool isReset = false;
    public bool isGameOver = true;
    Color colorDefault = new Color(1f, 1f, 1f, 1f);
    public float rotationZ;

    private Dictionary<Transform, GameObject> nearestItemsPerChild = new Dictionary<Transform, GameObject>();// Danh sách lưu trữ gridItem gần nhất cho mỗi phần tử con

    IEnumerator Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        rotationZ = transform.rotation.eulerAngles.z;

        yield return new WaitForEndOfFrame();
        transform.parent.GetComponent<lostGame>().CheckLost(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item.isXoay)
        {
            foreach (Transform child in transform)
            {
                child.transform.rotation = Quaternion.Euler(0, 0, child.transform.eulerAngles.z - 90);
            }
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + 90);
            
            transform.parent.GetComponent<lostGame>().CheckLost(false);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        if (item.isXoay)
        {
            // Bắt đầu đếm thời gian giữ
            holdCoroutine = StartCoroutine(HoldCheck(eventData));
        }
        else
        {
            ExecuteHoldAction(eventData);
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        // Dừng coroutine nếu người dùng thả tay trước 1 giây
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }
        if (isReset)
        {
            resetChildren();
        }
    }
    private IEnumerator HoldCheck(PointerEventData eventData)
    {
        // Đợi 1 giây
        yield return new WaitForSeconds(.5f);

        // Sau 1 giây giữ, thực hiện hành động
        ExecuteHoldAction(eventData);
    }
    private void ExecuteHoldAction(PointerEventData eventData)
    {
        setChildren();

        // Đẩy object lên trên một chút
        rectTransform.anchoredPosition = eventData.position + new Vector2(0f, -100f);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPointerPosition
        );

        dragOffset = rectTransform.anchoredPosition - localPointerPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item.isXoay)
        {
            if (holdCoroutine != null)
            {
                StopCoroutine(holdCoroutine);
                holdCoroutine = null;
            }
            ExecuteHoldAction(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isReset)
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
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        DropBox();
    }
    void setChildren()
    {
        isReset = true;
        RectTransform rectTransform = GetComponent<RectTransform>();
        defaultScale = rectTransform.localScale;
        rectTransform.localScale = new Vector3(GridResizer.cellWidth / transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta.x, GridResizer.cellHeight / transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta.y, rectTransform.localScale.z);
    }
    void resetChildren()
    {
        isReset = false;
        rectTransform.localScale = defaultScale;
        rectTransform.anchoredPosition = originalPosition;
    }
    void FindNearestGridItems()
    {
        nearestItemsPerChild.Clear(); // Xóa dữ liệu cũ mỗi frame

        GameObject[] gridItems = GameObject.FindGameObjectsWithTag("gridItem");

        RectTransform rectTransform = transform.GetChild(0).gameObject.GetComponent<RectTransform>();

        Vector3 imageCenter = rectTransform.position;
        float minEdgeDistance = rectTransform.rect.width * rectTransform.lossyScale.x;
        GameObject nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject item in gridItems)
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
            foreach (Transform child in transform)
            {
                foreach (GameObject item in gridItems)
                {
                    if (((transform.GetChild(0).position - child.position) == (nearest.transform.position - item.transform.position)) && !nearestItemsPerChild.ContainsValue(item))
                    {
                        nearestItemsPerChild[child] = item;
                    }
                }
            }
        }
        GameObject obj = GameObject.Find("Grid");
        if (obj != null)
        {
            GridSpawner grid = obj.GetComponent<GridSpawner>();
            if (grid != null)
            {
                grid.checkWin(transform.GetChild(0).gameObject.GetComponent<Image>().sprite);
            }
        }
        foreach (GameObject item in gridItems)
        {
            if (nearestItemsPerChild.ContainsValue(item) && nearestItemsPerChild.Count >= transform.childCount)
            {
                item.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = transform.GetChild(0).gameObject.GetComponent<Image>().sprite;
                item.transform.GetChild(0).gameObject.GetComponent<Image>().color = colorDefault * .8f;
                item.transform.GetChild(0).gameObject.SetActive(true);
                item.transform.GetChild(0).gameObject.tag = "boolTicker";
            }
            else
            {
                item.transform.GetChild(0).gameObject.SetActive(false);
                item.transform.GetChild(0).gameObject.tag = "Untagged";
            }
        }
    }
    public bool boolCheckLost()
    {
        isGameOver = true;
        check(0);
        foreach (Transform child in transform)
        {
            if (isGameOver)
            {
                child.gameObject.GetComponent<Image>().color = colorDefault * .8f;
            }
            else
            {
                child.gameObject.GetComponent<Image>().color = colorDefault;
            }
        }
        return isGameOver;
    }
    void check(int i)
    {
        nearestItemsPerChild.Clear(); // Xóa dữ liệu cũ mỗi frame

        GameObject[] gridItems = GameObject.FindGameObjectsWithTag("gridItem");

        if (i >= gridItems.Length) { return; }

        nearestItemsPerChild[transform.GetChild(0)] = gridItems[i];

        // float tile = GridResizer.cellWidth / transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta.x;
        float tile = GridResizer.cellWidth / (transform.GetChild(0).gameObject.GetComponent<RectTransform>().rect.width*transform.localScale.x);
        for (int y = 1; y < transform.childCount; y++)
        {
            Transform child = transform.GetChild(y);
            foreach (GameObject item in gridItems)
            {

                if ((transform.GetChild(0).position - child.position) * tile == (gridItems[i].transform.position - item.transform.position))
                {
                    nearestItemsPerChild[child.transform] = item;
                }
            }
        }
        if (nearestItemsPerChild.Count >= transform.childCount)
        {
            isGameOver = false;
            // foreach (GameObject item in gridItems)
            // {
            //     if (nearestItemsPerChild.ContainsValue(item) && nearestItemsPerChild.Count >= transform.childCount)
            //     {
            //         item.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = transform.GetChild(0).gameObject.GetComponent<Image>().sprite;
            //         item.transform.GetChild(0).gameObject.GetComponent<Image>().color = transform.GetChild(0).gameObject.GetComponent<Image>().color * .8f;
            //         item.transform.GetChild(0).gameObject.SetActive(true);
            //     }
            // }
        }
        else
        {
            check(i + 1);
        }
    }
    void DropBox()
    {
        if (nearestItemsPerChild.Count >= transform.childCount)
        {
            if (transform.rotation.eulerAngles.z != rotationZ)
            {
                item.rotateCount--;
                PlayerPrefs.SetInt("RotateCount", item.rotateCount);
            }
            if (item.isXoay)
            {
                transform.parent.gameObject.GetComponent<item>().itemXoay();
            }
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(10 * transform.childCount);
            }
            foreach (var item in nearestItemsPerChild.Values)
            {
                item.transform.GetChild(0).gameObject.GetComponent<Image>().color = colorDefault;
                item.transform.GetChild(0).gameObject.SetActive(true);
                item.tag = "ticker";
            }
            transform.parent.GetComponent<lostGame>().CheckLost(true);
            GameObject obj = GameObject.Find("Grid");
            if (obj != null)
            {
                GridSpawner grid = obj.GetComponent<GridSpawner>();
                if (grid != null)
                {
                    grid.CheckWinCondition();
                }

                addItem obstacle = obj.GetComponent<addItem>();
                if (obstacle != null)
                {
                    obstacle.checkObstacle();
                }
            }
            Destroy(gameObject);
        }
    }
}
