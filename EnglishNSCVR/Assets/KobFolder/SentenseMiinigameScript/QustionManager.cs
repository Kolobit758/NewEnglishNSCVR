using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class QuestionManager : MonoBehaviour
{
    public SO_QandA sO_QandA;
    public BeatManager beatManager;
    public int step;
    public int currentScore;
    private bool isProcessingChoice = false; // เพิ่ม field นี้
    [SerializeField] int decreseScore;
    [SerializeField] int increseScore;

    [Header("thrid Data")]

    public string correctSentence;
    public string[] correctSequence;


    public List<string> thirdAwsSelect = new List<string>();
    [Header("Current Data")]
    private int nextWordIndex = 0;
    private List<string> currentChoices = new List<string>();
    public List<GameObject> selectedBoxes = new List<GameObject>();
    public List<Question_Sound> questionHistory = new List<Question_Sound>();


    [Header("Settings")]

    public GameObject AwserBoxPrefab;
    public ChangeQuestionText ChangeQuestionText;

    public Transform spawPoint;

    public HitZone hitZone;

    private bool isCorrect;

    public GameObject QuestionUI;

    [Header("QuestionAndAwnser")]

    public Question_Sound firstQuestion;
    public string secondQuestion;
    public string thirdQuestion;
    public int QuestionCount = 0;


    private List<GameObject> activeBoxes = new List<GameObject>();

    private int spawnQueue = 0;
    [Header("Layout Settings")]
    public float gap = 1.5f; // ระยะห่างระหว่างกล่อง
    public Transform resultCenterPoint; // จุดกึ่งกลางจอที่อยากให้กล่องไปเรียง



    void OnEnable()
    {
        BeatManager.OnActionBeat += GameLoopLogic;
    }

    void OnDisable()
    {
        BeatManager.OnActionBeat -= GameLoopLogic;
    }



    void GameLoopLogic()
    {
        Debug.Log("Step = " + step + " | spawnQueue = " + spawnQueue);

        if (step == 0)
        {
            StartStep1();
            return;
        }

        if (spawnQueue > 0)
        {
            Debug.Log(">>> SPAWNING STEP " + step);
            SpawnProcess();
            spawnQueue--;
        }
    }



    void StartStep1()
    {
        step = 1;

        QuestionUI.SetActive(false);




        if (QuestionCount < sO_QandA.questions.Count)
        {
            int ranNum;
            Question_Sound randomQuestion;

            do
            {
                ranNum = Random.Range(0, sO_QandA.questions.Count);
                randomQuestion = sO_QandA.questions[ranNum];

            } while (questionHistory.Contains(randomQuestion)); // 🔥 ถ้าซ้ำ → สุ่มใหม่

            firstQuestion = randomQuestion;
            questionHistory.Add(randomQuestion); // 🔥 ต้อง add ไม่งั้นซ้ำแน่
            QuestionCount++;
        }
        else
        {
            beatManager.OutOfQustion();
        }



        Debug.Log("Let find : " + firstQuestion.question);

        currentChoices.Clear();

        currentChoices.Add(firstQuestion.question);

        int safetyNet = 0;

        while (currentChoices.Count < 3 && safetyNet < 100)
        {
            string randomWrong =
                sO_QandA.questions[Random.Range(0, sO_QandA.questions.Count)].question;

            if (!currentChoices.Contains(randomWrong))
                currentChoices.Add(randomWrong);

            safetyNet++;
        }

        // Shuffle
        SwapChoice();

        spawnQueue = currentChoices.Count;
    }





    void SpawnProcess()
    {
        float offset = activeBoxes.Count * 2f;

        Vector3 spawnPosWithOffset =
            spawPoint.position + (Vector3.right * offset);

        GameObject newBox =
            Instantiate(AwserBoxPrefab, spawnPosWithOffset, Quaternion.identity);

        activeBoxes.Add(newBox);

        AwnserBoxInfo info = newBox.GetComponent<AwnserBoxInfo>();
        info.hitZone = this.hitZone;
        info.setMaxSize();

        int choiceIndex = activeBoxes.Count - 1; // ✅ ใช้ตัวเดียวทุก step

        if (choiceIndex >= currentChoices.Count) return;

        if (step == 1)
        {
            string selectedSentence = currentChoices[choiceIndex];

            Question_Sound dataToSet =
                sO_QandA.questions.Find(q => q.question == selectedSentence);

            if (dataToSet != null)
                info.SetupFirstStep(dataToSet);
        }
        else if (step == 2 || step == 3)
        {
            info.SetupOtherStep(currentChoices[choiceIndex]);
        }
    }



    public void ChooseChoice()
    {

        if (step == 1)
        {
            Question_Sound selected =
                hitZone.GetCurrentChoiceFirst();

            isCorrect = false; // ✅ reset ก่อน
            if (selected != null && selected.question == firstQuestion.question)
            {
                isCorrect = true;
                Debug.Log("Correct Step 1!");

                ChangeQuestionText.ChangeText(firstQuestion.question);

                ClearOldBoxes();

                StartStep2();
            }

            else if (selected != null && selected.question != firstQuestion.question)
            {
                isCorrect = false;
            }

            ChangeScore();
        }



        else if (step == 2)
        {
            string selected = hitZone.GetCurrentChoice();
            isCorrect = false; // ✅ reset ก่อน
            if (selected == correctSentence)
            {
                isCorrect = true;

                ChangeQuestionText.ChangeText(secondQuestion);

                Debug.Log("Correct Step 2!");

                ClearOldBoxes();

                StartStep3();
                Debug.Log("start step 3");
            }
            else
            {
                isCorrect = false;
            }

            ChangeScore();
        }



        else if (step == 3)
        {

            if (isProcessingChoice) return; // ✅ ย้ายมาบนสุดก่อนทุกอย่าง
            isProcessingChoice = true;

            Debug.Log($"boxesInZone count = {hitZone.boxesInZone.Count}");
            for (int i = 0; i < hitZone.boxesInZone.Count; i++)
                Debug.Log($"  [{i}] = {hitZone.boxesInZone[i]?.data}");
            Debug.Log($"nextWordIndex = {nextWordIndex}, expecting = {correctSequence[nextWordIndex]}");

            string selected = hitZone.GetCurrentChoice();
            if (string.IsNullOrEmpty(selected))
            {
                isProcessingChoice = false; // ✅ อย่าลืม reset ถ้า return กลางทาง
                return;
            }

            isCorrect = false;

            if (selected == correctSequence[nextWordIndex])
            {
                hitZone.boxesInZone[0].SelectBox(); // ✅ เรียกตรงนี้แทน
                thirdAwsSelect.Add(selected);
                nextWordIndex++;

                if (nextWordIndex >= correctSequence.Length)
                    CheckSentence();
            }

            else
            {
                Debug.Log("not correct");
                isCorrect = false;
                ChangeScore();

                ClearOldBoxes();

                foreach (GameObject box in selectedBoxes)
                {
                    if (box != null)
                        Destroy(box);
                }

                selectedBoxes.Clear();

                step = 0;
                beatManager.ChangeRound(currentScore);
            }


            StartCoroutine(ResetChoiceFlag()); // ✅ รอ 1 frame แล้วค่อย unlock
        }

    }
    IEnumerator ResetChoiceFlag()
    {
        yield return null; // รอ 1 frame
        isProcessingChoice = false;
    }


    void StartStep2()
    {
        step = 2;

        QuestionUI.SetActive(true);

        secondQuestion = firstQuestion.awnserThisQuestion;

        correctSentence = secondQuestion;

        currentChoices.Clear();

        currentChoices.Add(secondQuestion);

        int safetyNet = 0;

        while (currentChoices.Count < 3 && safetyNet < 100)
        {
            string randomWrong =
                sO_QandA.answers[Random.Range(0, sO_QandA.answers.Count)];

            if (!currentChoices.Contains(randomWrong))
                currentChoices.Add(randomWrong);

            safetyNet++;
        }

        SwapChoice();

        spawnQueue = currentChoices.Count;
    }



    void StartStep3()
    {
        ClearOldBoxes();
        step = 3;

        Debug.Log("step 3 begin");
        hitZone.boxesInZone.Clear(); // 👈 เพิ่มฟังก์ชันนี้

        correctSentence = secondQuestion;

        ChangeQuestionText.ChangeText(correctSentence);

        correctSequence = correctSentence.Trim().Split(' ');

        nextWordIndex = 0;

        thirdAwsSelect.Clear();

        currentChoices.Clear();

        foreach (string word in correctSequence)
        {
            currentChoices.Add(word);
        }

        SwapChoice();

        spawnQueue = correctSequence.Length;
        beatManager.ForceNextActionBeat(); // 👈 เพิ่มอันนี้
    }



    void ClearOldBoxes()
    {
        StopAllCoroutines(); // ✅ หยุด SmoothMove ทั้งหมดก่อน

        foreach (GameObject box in activeBoxes)
        {
            if (box != null)
                Destroy(box);
        }

        activeBoxes.Clear();
    }



    void CheckSentence()
    {
        isCorrect = true;

        for (int i = 0; i < thirdAwsSelect.Count; i++)
        {
            if (thirdAwsSelect[i] != correctSequence[i])
            {
                isCorrect = false;
                break;
            }
        }

        ChangeScore(); // ✅ ต้องเรียกตรงนี้ก่อน
        ClearOldBoxes(); // ✅ สำคัญ
        foreach (GameObject box in selectedBoxes)
        {
            Destroy(box);
        }
        selectedBoxes.Clear(); // ✅ 
        step = 0;
        beatManager.ChangeRound(currentScore); // ✅ ส่งแค่ score
    }
    void ChangeScore()
    {
        if (isCorrect)
        {
            Debug.Log("plus score");
            currentScore += increseScore;
            beatManager.ScoreUpdateUI(increseScore);


        }

        else
        {
            Debug.Log("decrease score");
            currentScore -= decreseScore;
            beatManager.ScoreUpdateUI(decreseScore);

        }
    }

    void SwapChoice()
    {
        for (int i = 0; i < currentChoices.Count; i++)
        {
            string temp = currentChoices[i];
            int randomIndex = Random.Range(i, currentChoices.Count);

            currentChoices[i] = currentChoices[randomIndex];
            currentChoices[randomIndex] = temp;
        }
    }
    public void fillBoxInList(GameObject box)
    {
        selectedBoxes.Add(box);
        RearrangeSelectedBoxes(); // ทุกครั้งที่เพิ่ม ให้จัดเรียงใหม่
    }
    void RearrangeSelectedBoxes()
    {
        int count = selectedBoxes.Count;
        // คำนวณหาจุดเริ่มต้น (ซ้ายสุด) เพื่อให้กึ่งกลางกลุ่มอยู่ตรงกลางจุดที่กำหนด
        // สูตร: จุดศูนย์กลาง - (ครึ่งหนึ่งของความกว้างทั้งหมด)
        float totalWidth = (count - 1) * gap;
        Vector3 startPos = resultCenterPoint.position + (Vector3.left * (totalWidth / 2f));

        for (int i = 0; i < count; i++)
        {
            if (selectedBoxes[i] == null) continue; // ✅ กัน null

            Vector3 targetPos = startPos + (Vector3.right * (i * gap));
            StartCoroutine(SmoothMove(selectedBoxes[i], targetPos));
        }
    }

    IEnumerator SmoothMove(GameObject obj, Vector3 target)
    {
        if (obj == null) yield break; // ✅ กันตั้งแต่เริ่ม

        float elapsed = 0;
        Vector3 start = obj.transform.position;

        while (elapsed < 0.2f)
        {
            if (obj == null) yield break; // ✅ กันตอนโดน destroy ระหว่างทาง

            obj.transform.position = Vector3.Lerp(start, target, elapsed / 0.2f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (obj != null)
            obj.transform.position = target;
    }
}