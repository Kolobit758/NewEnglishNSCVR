using UnityEngine;
using UnityEngine.UI;


public class ConverManager : MonoBehaviour
{
    public NPC_Controller_New nPC_Controller;

    public Gamesubmary gamesubmary;
    playerAskNPCState currentConverState;
    public GameManager gameManager;
    public CheckSuitableWord checkSuitableWord;
    public ConverRulebase_Manager converRulebase_Manager;

    [SerializeField] bool currentMode = false;
    

    [SerializeField] string jsonSelect;
    [SerializeField]private string testText = "here you are";



    public string playerMsg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameEvent.OnNPCComming += ConverSationStart;

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region converLoop function
    // delegate event to chage this gameloopcase
    void ConverloopCase()
    {
        switch (currentConverState)
        {
            case playerAskNPCState.NPC_Wait:
                ConverSationStart();
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

    public void ConverSationStart()
    {

        SetState(playerAskNPCState.Player_StartConver);
    }
    public void PlayerTalkFinished()
    {
        // get player sound and parse to text
        //sent to gpt api

        SetState(playerAskNPCState.NPC_Response);
    }

    public void FinishedConver()
    {
        SetState(playerAskNPCState.NPC_Wait);
    }




    #endregion

    // public void MakePlayerMsg()
    // {
    //     PlayerMessage playerMessage = new PlayerMessage("miike", "what do you like to eat today");

    //     playerMsg = TextToJson(playerMessage);
    // }

    public void SentMessageToServer(string text)
    {
        if (text == null)
        {
            text = "what do you like to eat today";
        }

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
                    if(msg.evaluation == null) return; 
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

        SetState(playerAskNPCState.Finished);
    }


    string TextToJson(PlayerMessage playerMessage)
    {
        string jsonText = JsonUtility.ToJson(playerMessage, true);
        Debug.Log(jsonText);

        return jsonText;
    }

    public void SetState(playerAskNPCState newState)
    {
        currentConverState = newState;
        Debug.Log("ConversationState -> " + newState);
        Debug.Log("========================================");
        ConverloopCase();
    }
}
