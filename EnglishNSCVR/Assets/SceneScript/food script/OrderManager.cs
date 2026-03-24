using TMPro;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;

    public string[] ingredientNames;
    public string currentTarget;

    public TextMeshProUGUI targetText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InvokeRepeating(nameof(NewOrder), 2f, 5f);
    }

    void NewOrder()
    {
        int i = Random.Range(0, ingredientNames.Length);
        currentTarget = ingredientNames[i];

        if (targetText != null)
            targetText.text = "Cut: " + currentTarget;
    }
}