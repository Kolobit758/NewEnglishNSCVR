using TMPro;
using UnityEngine;

public class FoodGameManager : MonoBehaviour
{
    public static FoodGameManager instance;

    public float gameTime = 180f;
    public bool isGameOver = false;

    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        gameTime -= Time.deltaTime;

        UpdateTimerUI();

        if (gameTime <= 0)
        {
            EndGame();
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(gameTime / 60);
            int seconds = Mathf.FloorToInt(gameTime % 60);

            timerText.text = "Time: " + minutes + ":" + seconds.ToString("00");
        }
    }

    void EndGame()
    {
        isGameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Debug.Log("Game Over");
    }
}