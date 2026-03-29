using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrosshairUI : MonoBehaviour
{
    [Header("Crosshair")]
    public Image crosshairImage;
    public Color normalColor = Color.white;
    public Color interactColor = Color.yellow; // เปลี่ยนสีตอนเล็งโดน

    [Header("Prompt Text")]
    public TextMeshProUGUI promptText; // ข้อความ "คลิกเพื่อ..."

    public void SetInteractable(bool canInteract, string text)
    {
        crosshairImage.color = canInteract ? interactColor : normalColor;
        promptText.text = canInteract ? text : "";
        promptText.gameObject.SetActive(canInteract);
    }
}
