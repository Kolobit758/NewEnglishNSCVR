using UnityEngine;
using TMPro;
public class ChangeQuestionText : MonoBehaviour
{
    public TMP_Text showText;
    public void ChangeText(string text)
    {
        showText.text = text;
    }
}
