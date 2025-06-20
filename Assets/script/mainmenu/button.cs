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
}
