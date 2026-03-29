using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ConverManager_New : MonoBehaviour
{
    public NPC_Controller_New nPC_Controller;

    public Gamesubmary gamesubmary;
    [Header("NPC AND PLAYER CONVER STATE")]
    public playerAskNPCState currentPlayerAskState;
    public npcAskPlayerState currentNpcAskState;
    public bool isPlayerAskstate = true; // true = playerAskNPCState false = npcAskPlayerState
    [Header("REFERENCE")]
    public GameManager gameManager;
    public CheckSuitableWord checkSuitableWord;
    public ConverRulebase_Manager converRulebase_Manager;

    [SerializeField] bool currentMode = false;
    [SerializeField] string jsonSelect;
    [SerializeField] private string testText = "here you are";
    [Header("NPC ASK SYSTEM")]
    public SO_NPCQuestions npcQuestions;
    private QandANPC currentQuestion;
    public NPCAnswerEvaluator answerEvaluator;
    public SkillAnalyze skillAnalyzer;
    private SkillSession skillSession = new SkillSession();
    public string playerMsg;
    public int meaningScoreIncrese = 2;
    [Header("STORY SYSTEM")]
    private SO_NPC currentNPC;
    private NPCStoryNode currentNode;
    public int currentNodeIndex = 0;




    void Start()
    {
        Debug.Log("Start Conver Sation");
        GameEvent.OnNPCComming += ConverSationStart;
        GameEvent.OnPlayerConfirmedOrder += ForceNPCAskPlayer;

    }

    private void ResetStoryNodes()
    {
        if (currentNPC == null) return;
        foreach (var node in currentNPC.storyNodes)
        {
            node.isUnlocked = false;
        }
        currentNPC.affinity = 0;
        currentNodeIndex = 0;
        Debug.Log("Story nodes reset!");
    }
    #region converLoop function
    // delegate event to chage this gameloopcase
    void ConverloopCase()
    {
        // ถ้า isPlayerAskstate = true จะให้ทำลูปของ player ที่ถาท NPC ถ้า false ให้ เล่นลูปที่ NPC ถาม player ก่อน
        if (isPlayerAskstate == true)
        {
            switch (currentPlayerAskState)
            {
                case playerAskNPCState.NPC_Wait:
                    ConverSationStart();
                    Debug.Log("Player ask NPC");
                    break;
                case playerAskNPCState.Player_StartConver:
                    // open Microphone player start talk and sent it to server

                    break;
                case playerAskNPCState.NPC_Response:
                    // NPCResponse();
                    break;
                case playerAskNPCState.Finished:
                    FinishedConver();
                    break;

            }
        }
        else
        {
            switch (currentNpcAskState)
            {
                case npcAskPlayerState.NPC_StartConver:
                    // open Microphone player start talk and sent it to server
                    Debug.Log("NPC ask Player");
                    break;
                case npcAskPlayerState.Player_Response:
                    // NPCResponse();
                    break;
                case npcAskPlayerState.Finished:
                    FinishedConver();
                    break;

            }
        }

    }







    #endregion

    #region PLAYER ASK NPC
    public void ConverSationStart()
    {
        currentNPC = nPC_Controller.currentNpcData;
        ResetStoryNodes();
        StartPlayerAskMode();
        SetPlayerAskState(playerAskNPCState.Player_StartConver);
    }
    public void PlayerTalkFinished()
    {
        // get player sound and parse to text
        //sent to gpt api

        SetPlayerAskState(playerAskNPCState.NPC_Response);
    }

    public void FinishedConver()
    {
        SetPlayerAskState(playerAskNPCState.NPC_Wait);
        SetNPCAskState(npcAskPlayerState.NPC_StartConver);
    }
    public void SentMessageToServer(string text)
    {
        if (text == null) text = "what do you like to eat today";

        checkSuitableWord.CheckWord(text);
        SkillFeature f = skillAnalyzer.Analyze(text);
        skillSession.AddTurn(f);
        int level = skillAnalyzer.CalculateLevel(f, skillSession);

        Debug.Log("player say : " + text + " | level : " + level);

        // ✅ ถ้า NPC กำลังถาม player → ส่งไปตอบคำถาม NPC
        if (!isPlayerAskstate)
        {
            HandlePlayerAnswer(text);
            return;
        }

        // ✅ player ถาม NPC → ลองเช็ค story node ก่อน
        bool storyProgressed = TryProgressStory(text);

        if (storyProgressed) return; // NPC ตอบตาม story แล้ว ไม่ต้องไป rulebase

        // ✅ ไม่มี keyword story → ไปตอบแบบ rulebase / AI ปกติ
        if (!PlayerData.isAiMode)
        {
            HandleNPCResponse(converRulebase_Manager.NPC_Rulebase_Response(text));
        }
        else
        {
            GetComponent<PlayerMsgToServer>().SendMessageToServer(text, (response) =>
            {
                HandleNPCResponse(response);
            });
        }
    }

    // ✅ ฟังก์ชันใหม่: เช็ค story node แล้วถ้า unlock ได้ → ให้ NPC ตอบตาม story
    private bool TryProgressStory(string text)
    {
        if (currentNPC == null || currentNPC.storyNodes.Count == 0) return false;

        // วนหา node ที่ unlock แล้ว และ keyword match
        for (int i = 0; i < currentNPC.storyNodes.Count; i++)
        {
            var node = currentNPC.storyNodes[i];

            // เช็คเฉพาะ node ที่ unlock แล้ว
            if (!node.isUnlocked) continue;

            foreach (var keyword in node.requiredKeywords)
            {
                if (text.ToLower().Contains(keyword.ToLower()))
                {
                    // match! → NPC ตอบด้วย response ของ node นี้
                    ShowNode(i);
                    Debug.Log($"Story matched: {node.topic}");

                    // unlock node ถัดไปตาม nextNodeIndex
                    foreach (int nextIndex in node.nextNodeIndex)
                    {
                        if (nextIndex >= currentNPC.storyNodes.Count) continue;

                        var nextNode = currentNPC.storyNodes[nextIndex];

                        // เช็ค affinity ก่อน unlock
                        if (currentNPC.affinity >= nextNode.requiredAffinity)
                        {
                            nextNode.isUnlocked = true;
                            Debug.Log($"Unlocked next node: {nextNode.topic}");
                        }
                    }

                    currentNPC.affinity += 1;
                    currentNodeIndex = i;
                    SetPlayerAskState(playerAskNPCState.Finished);
                    return true;
                }
            }
        }

        return false;
    }

    // ✅ ย้าย CanUnlock logic มาอยู่ใน ConverManager เพราะ NPC_Controller ไม่ควรรู้เรื่อง index
    private bool CanUnlockNode(NPCStoryNode node, string playerText)
    {
        playerText = playerText.ToLower();

        Debug.Log($"CanUnlock: affinity={currentNPC.affinity} required={node.requiredAffinity}");

        if (currentNPC.affinity < node.requiredAffinity)
        {
            Debug.Log("CanUnlock: FAIL affinity not enough");
            return false;
        }

        foreach (var keyword in node.requiredKeywords)
        {
            Debug.Log($"CanUnlock: checking keyword='{keyword.ToLower()}' in text='{playerText}'");
            if (playerText.Contains(keyword.ToLower()))
            {
                Debug.Log("CanUnlock: MATCH!");
                return true;
            }
        }

        Debug.Log("CanUnlock: FAIL no keyword match");
        return false;
    }
    public void TestEndconver()
    {
        string text = testText;

        checkSuitableWord.CheckWord(text);
        Debug.Log("=========Check Word===========");
        if (!PlayerData.isAiMode)
        {
            HandleNPCResponse(converRulebase_Manager.NPC_Rulebase_Response(text));
        }
        else
        {
            GetComponent<PlayerMsgToServer>().SendMessageToServer(text, (response) =>
            {
                HandleNPCResponse(response);
            });
        }

    }
    private void HandleNPCResponse(NPCResponse msg)
    {
        if (msg == null) return;

        nPC_Controller.showMessageFromJson(msg);


        switch (msg.intent)
        {
            case "server":
                Debug.Log("System Initialized: " + msg.ai_response);
                break;

            case "order":
                GameEvent.OnNPCOrdered?.Invoke();
                Debug.Log("NPC is ordering food.");
                break;
            case "normal":
                Debug.Log("normalTalk : " + msg.intent + " " + msg.ai_response);
                break;

            case "pay":
                Debug.Log("NPC wants to pay.");
                // GameEvent.OnNPCPaymentRequested?.Invoke();
                break;

            case "delivery":
                if (msg.is_end_game)
                {
                    if (msg.evaluation == null) return;
                    Debug.Log("Game Finished! Rank: " + msg.evaluation.rank);
                    gamesubmary.addItem(msg.evaluation.rank, msg.evaluation.strengths, msg.evaluation.improvements, msg.evaluation.next_rank_tip);
                }
                break;
            case "Unidentified":
                Debug.Log("Undentified text : can you say again");
                break;

            default:
                Debug.Log("Normal conversation.");
                break;
        }

        SetPlayerAskState(playerAskNPCState.Finished);
    }
    public void SetPlayerAskState(playerAskNPCState newState)
    {
        currentPlayerAskState = newState;
        Debug.Log("ConversationState -> " + newState);
        Debug.Log("========================================");
        ConverloopCase();
    }
    #endregion


    #region NPC ASK PLAYER
    public void SetNPCAskState(npcAskPlayerState newState)
    {
        currentNpcAskState = newState;
        Debug.Log("ConversationState -> " + newState);
        Debug.Log("========================================");
        ConverloopCase();
    }

    public void StartNPCAsk()
    {
        if (npcQuestions.QandANPCs.Count == 0)
        {
            Debug.LogWarning("No NPC Questions!");
            return;
        }

        isPlayerAskstate = false;

        int rand = Random.Range(0, npcQuestions.QandANPCs.Count);
        currentQuestion = npcQuestions.QandANPCs[rand];

        NPCResponse msg = new NPCResponse(currentQuestion.question, "npc_question", false, null);
        nPC_Controller.showMessageFromJson(msg);

        SetNPCAskState(npcAskPlayerState.Player_Response);
    }

    public void HandlePlayerAnswer(string text)
    {
        if (currentQuestion == null) return;

        bool hasKeyword = answerEvaluator.CheckMeaning(text, currentQuestion);

        if (hasKeyword)
            gamesubmary.currentScore += meaningScoreIncrese;

        string feedback = hasKeyword ? currentQuestion.awnser : "I see, thanks for sharing.";

        NPCResponse response = new NPCResponse(feedback, "npc_feedback", false, null);
        nPC_Controller.showMessageFromJson(response);

        // ✅ กลับ player ask mode
        isPlayerAskstate = true;

        // ✅ หลัง NPC ถาม-ตอบจบ ให้ story เดิน (ถ้า answer มี keyword ของ story)
      

        SetPlayerAskState(playerAskNPCState.Finished);
    }

    void ForceNPCAskPlayer()
    {
        isPlayerAskstate = false;
        StartNPCAsk();
    }

    void StartPlayerAskMode()
    {
        if (currentNPC == null || currentNPC.storyNodes.Count == 0)
        {
            Debug.LogWarning("No Story Node!");
            return;
        }

        currentNodeIndex = 0;

        currentNode = currentNPC.storyNodes[currentNodeIndex];
        currentNode.isUnlocked = true;

        // ShowNode(currentNodeIndex);
    }

    public void ShowNode(int index)
    {
        var node = currentNPC.storyNodes[index];

        NPCResponse msg = new NPCResponse(node.npcResponse, "story", false, null);
        nPC_Controller.showMessageFromJson(msg);

        Debug.Log("Show Node: " + node.topic);
    }

    #endregion






}
