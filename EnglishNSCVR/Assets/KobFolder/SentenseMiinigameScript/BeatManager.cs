using System;
using UnityEngine;
using TMPro;
using System.Collections;

public class BeatManager : MonoBehaviour
{
    [Serializable]
    public struct RhythmPattern
    {
        public string name;
        public int[] steps; // เช่น {1, 0, 1, 1}
    }

    [Header("Settings")]
    public AudioSource audioSource;
    public float bpm = 120f;
    public RhythmPattern[] patternLibrary; // ใส่ได้หลายแบบใน Inspector

    [Header("Runtime")]
    public int maxRound;
    public int currentRound;
    public int currentPatternIndex = 0;
    private int stepIndex = 0;
    private double interval;      // ไม่ใช่ float interval
    private double nextBeatTime;  // ไม่ใช่ float nextBeatTime
    private double startDspTime;
    private int beatCount;

    public static Action OnBeat;        // ทุกบีต (สำหรับ Visual/Background)
    public static Action OnActionBeat;  // เฉพาะจังหวะที่ต้องกด (Spawn Note)
    [Header("About UI")]
    public GameObject submaryUI;
    public TMP_Text totalScoreText;
    public GameObject scoreUpadteUI;

    public bool hasQuestion = true;


    void Start()
    {
        submaryUI.SetActive(false);
        interval = 60.0 / bpm;
        currentRound = 1;
        // ✅ ดึง dspTime แล้วเผื่อเวลา 1 interval ไปข้างหน้า
        startDspTime = AudioSettings.dspTime - interval * 0;
        beatCount = 0;
        // แต่เพิ่ม offset ป้องกัน Start() กับ Update() frame แรก overlap
        startDspTime += 0.1; // หน่วง 100ms ก่อน beat แรก
    }

    void Update()
    {
        while (AudioSettings.dspTime >= startDspTime + beatCount * interval && currentRound <= maxRound)
        {

            HandleBeat();
            beatCount++; // คำนวณจาก origin เสมอ ไม่สะสม error
        }
    }

    void HandleBeat()
    {
        OnBeat?.Invoke();
        if (patternLibrary == null || patternLibrary.Length == 0) return;
        // ดึง Pattern ปัจจุบันมาใช้
        int[] currentSteps = patternLibrary[currentPatternIndex].steps;

        if (currentSteps[stepIndex] == 1)
        {
            OnActionBeat?.Invoke();

            Debug.Log($"<color=cyan>Hit!</color> ในชุด {patternLibrary[currentPatternIndex].name}");
        }

        // วนลูปใน Pattern
        stepIndex++;
        if (stepIndex >= currentSteps.Length)
        {
            stepIndex = 0;
            if (currentPatternIndex == 0)
            {
                ChangePattern(1);
            }
            else
            {
                ChangePattern(0);
            }

            // คุณอาจจะสั่งสุ่ม Pattern ใหม่ที่นี่เมื่อจบประโยคก็ได้
        }
    }

    // ฟังก์ชันสำหรับเปลี่ยน Pattern เมื่อเปลี่ยน Stage หรือสุ่มใหม่
    public void ChangePattern(int index)
    {
        currentPatternIndex = Mathf.Clamp(index, 0, patternLibrary.Length - 1);
        stepIndex = 0;
    }
    public void ChangeRound(int currentScore)
    {
        currentRound++;

        if (currentRound >= maxRound) // ✅ ใช้ > แทน ==
        {
            submaryUI.SetActive(true);
            totalScoreText.text = "Total Score : " + currentScore;
        }
        else if (!hasQuestion)
        {
            submaryUI.SetActive(true);
            totalScoreText.text = "Total Score : " + currentScore;
        }

        beatCount = 0;
        stepIndex = 0;
        startDspTime = AudioSettings.dspTime + 0.1; // หน่วงนิดนึง

        Debug.Log("New Round Start!");

    }

    public void ScoreUpdateUI(int scoreChange)
    {

        scoreUpadteUI.GetComponent<ScoreUpdateAnimation>().PlayScoreUpdate(scoreChange);

    }

    public void OutOfQustion()
    {
        hasQuestion = false;
    }
    public void ForceNextActionBeat()
    {
        stepIndex = 0; // บังคับให้เริ่มที่ index 0 (ซึ่งเป็น 1)
    }

}
