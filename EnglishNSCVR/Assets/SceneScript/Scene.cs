using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void PlayAgain(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
