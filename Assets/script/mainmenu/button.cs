using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Tên scene Loading
    private string sceneLoad = "Loading";
    private string sceneHome = "Home";

    // Hàm này gắn vào OnClick của Button Play
    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene(sceneLoad);
    }
    public void OnHomeButtonPressed()
    {
        SceneManager.LoadScene(sceneHome);
    }
    public void OnOffButtonPressed(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
    public void ReStart(GameObject objLost)
    {
        //reset
        CoinManager.Instance.AddCoins(PlayerPrefs.GetInt("Score", 0)/2000);
        PlayerPrefs.SetInt("Score", 0);//diem
        PlayerPrefs.SetInt("IntAddItem", 0);//sao
        PlayerPrefs.SetInt("RotateCount", 0);//itemXoay
        PlayerPrefs.SetInt("DestroyCount", 0);//itemXoa
        PlayerPrefs.SetInt("BoomCount", 0);//itembom
        PlayerPrefs.SetInt("intCheckObstacle", 0);// tạo báu vật
        PlayerPrefs.SetInt("intCheckIce", 0);//tạo băng
        objLost.GetComponent<SaveOnQuit>().ResetSaveData();//khac
    }
    public void watchLostDown(GameObject obj)
    {
        obj.SetActive(false);
    }
    public void watchLostUp(GameObject obj)
    {
        obj.SetActive(true);   
    }
}
