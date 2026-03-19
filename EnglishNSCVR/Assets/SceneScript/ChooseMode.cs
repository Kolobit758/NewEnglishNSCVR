using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseMode : MonoBehaviour
{
    [Serializable]
    public struct ModeScene
    {
        public string modeName;
        public string sceneName;
    }

    public List<ModeScene> rolePlaymode = new List<ModeScene>();
    public List<ModeScene> learningMode = new List<ModeScene>();
    public GameObject learnModeUI;
    public GameObject rolePlayModeUI;
    public bool isOpen = false;

    void OnEnable()
    {
        learnModeUI.SetActive(false);
        rolePlayModeUI.SetActive(false);
    }
    public void OpenCloseRPMode()
    {
        if (isOpen == false)
        {
            isOpen = true;
            rolePlayModeUI.SetActive(true);
        }
        else
        {
            isOpen = false;
            rolePlayModeUI.SetActive(false);
        }
    }
    public void OpenCloseLearnMode()
    {
        if (isOpen == false)
        {
            isOpen = true;
            learnModeUI.SetActive(true);
        }
        else
        {
            isOpen = false;
            learnModeUI.SetActive(false);
        }
    }
    // 🔹 Role Play
    public void RolePlayMode(string modeName)
    {
        foreach (var mode in rolePlaymode)
        {
            if (mode.modeName == modeName)
            {
                SceneManager.LoadScene(mode.sceneName);
                return;
            }
        }

        Debug.Log("ไม่เจอ mode: " + modeName);
    }

    // 🔹 Learning Mode
    public void LearningMode(string modeName)
    {
        foreach (var mode in learningMode)
        {
            if (mode.modeName == modeName)
            {
                SceneManager.LoadScene(mode.sceneName);
                return;
            }
        }

        Debug.Log("ไม่เจอ mode: " + modeName);
    }
}