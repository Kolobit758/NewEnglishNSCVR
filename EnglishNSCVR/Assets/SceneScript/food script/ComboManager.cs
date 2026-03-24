using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public static ComboManager instance;

    public int combo = 0;
    public TextMeshProUGUI comboText;

    void Awake()
    {
        instance = this;
    }

    public void AddCombo()
    {
        combo++;
        UpdateUI();
    }

    public void ResetCombo()
    {
        combo = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (comboText != null)
            comboText.text = "Combo x" + combo;
    }
}