using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject modeSelectedUI;
    void Start()
    {
        modeSelectedUI.SetActive(false);
    }
    public void Play()
    {
        modeSelectedUI.SetActive(true);
    }
    public void Setting()
    {
        
    }
    public void Exit()
    {
        
    }
}
