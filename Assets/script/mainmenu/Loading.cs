using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingController : MonoBehaviour
{
    [Header("UI References")]
    // Kéo-thả Slider (ProgressBar) và Text (ProgressText) vào Inspector
    public Image imageBack;
    public Image imagePercent;
    public Text progressText;

    public float widthBack;

    // Tên scene cần load (Gameplay)
    private string sceneToLoad = "Play";

    void Start()
    {
        widthBack = imageBack.rectTransform.rect.width;
        // Bắt đầu coroutine để load scene đích
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync()
    {
        // Bắt đầu load bất đồng bộ, nhưng không cho activate ngay lập tức
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        // Loop cho đến khi tiến độ >= 0.9f (Unity dùng 0.9 để chuẩn bị chuyển đổi)
        while (!operation.isDone)
        {
            // operation.progress trả về giá trị từ 0 → 0.9
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (imagePercent != null)
                imagePercent.GetComponent<RectTransform>().sizeDelta = new Vector2(progress * widthBack, imagePercent.GetComponent<RectTransform>().sizeDelta.y);
            if (progressText != null)
                    progressText.text = (progress * 100f).ToString("F0") + "%";

            // Khi đã load xong (progress >= 0.9f), ta có thể kích hoạt scene mới
            if (operation.progress >= 0.9f)
            {
                // Hiển thị 100% nếu muốn
                if (imagePercent != null)
                    imagePercent.GetComponent<RectTransform>().sizeDelta = new Vector2(widthBack, imagePercent.GetComponent<RectTransform>().sizeDelta.y);
                if (progressText != null) 
                    progressText.text = "100%";

                // Có thể chờ thêm 0.5s để người chơi nhìn thấy 100%, hoặc ngay lập tức:
                yield return new WaitForSeconds(0.5f);

                // Cho phép chuyển sang scene Gameplay
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
