using UnityEngine;
using TMPro;

public class SelectedBox : MonoBehaviour
{
    public TMP_Text text;
    public string wordValue; // เก็บค่าคำศัพท์ไว้เช็ค
    
    public void Setup(string word)
    {
        wordValue = word;
        if (text != null) text.text = word;
    }
}
