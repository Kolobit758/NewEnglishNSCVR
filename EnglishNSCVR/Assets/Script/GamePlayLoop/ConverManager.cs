using UnityEngine;
using UnityEngine.UI;

public class ConverManager : MonoBehaviour
{
    public NPC_Controller nPC_Controller;
 
    public Gamesubmary gamesubmary;
    conversationState currentConverState;
    public GameManager gameManager;
    public CheckSuitableWord checkSuitableWord;

    [SerializeField] string jsonSelect;



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
            case conversationState.NPC_Wait:
                ConverSationStart();
                break;
            case conversationState.Player_StartConver:
                // open Microphone player start talk and sent it to server

                break;
            case conversationState.NPC_Response:
                // NPCResponse();
                break;
            case conversationState.Finished:
                FinishedConver();
                break;

        }
    }

    public void ConverSationStart()
    {

        SetState(conversationState.Player_StartConver);
    }
    public void PlayerTalkFinished()
    {
        // get player sound and parse to text
        //sent to gpt api

        SetState(conversationState.NPC_Response);
    }
    public void NPCResponse()
    {
        //NPC get gpt text from api and change text to sound play to player 
        TextAsset orderResponse = Resources.Load<TextAsset>(jsonSelect);
        NPCResponse msg = JsonUtility.FromJson<NPCResponse>(orderResponse.text);
        // if response type is order_food => invoke OnNPCOrdered Evente
        if (msg.intent == "order")
        {
            nPC_Controller.showMessageFromJson(msg);
            GameEvent.OnNPCOrdered?.Invoke();
            Debug.Log("order : " + msg.intent + " " + msg.ai_response);
        }
        else
        {
            nPC_Controller.showMessageFromJson(msg);
            Debug.Log("normalTalk : " + msg.intent + " " + msg.ai_response);
        }
        SetState(conversationState.Finished);
    }
    public void FinishedConver()
    {
        SetState(conversationState.NPC_Wait);
    }

    // public void TestReadJson()
    // {

    //     TextAsset orderResponse = Resources.Load<TextAsset>("npcOrderResponse");
    //     NPCResponse msg = JsonUtility.FromJson<NPCResponse>(orderResponse.text);
    //     // if response type is order_food => invoke OnNPCOrdered Evente
    //     if (msg.intent == "order")
    //     {
    //         GameEvent.OnNPCOrdered?.Invoke();
    //         Debug.Log("order" + msg.intent + " " + msg.ai_response);
    //     }
    // }


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

        GetComponent<PlayerMsgToServer>().SendMessageToServer(text, (response) =>
        {
            HandleNPCResponse(response);
        });

    }
    public void TestEndconver()
    {
        string text = "here you are";

        checkSuitableWord.CheckWord(text);
        Debug.Log("=========Check Word===========");
        GetComponent<PlayerMsgToServer>().SendMessageToServer(text, (response) =>
        {
            HandleNPCResponse(response);
        });

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
                    Debug.Log("Game Finished! Rank: " + msg.evaluation.rank);
                    gamesubmary.addItem(msg.evaluation.rank, msg.evaluation.strengths, msg.evaluation.improvements, msg.evaluation.next_rank_tip);
                }
                break;

            default:
                Debug.Log("Normal conversation.");
                break;
        }

        SetState(conversationState.Finished);
    }


    string TextToJson(PlayerMessage playerMessage)
    {
        string jsonText = JsonUtility.ToJson(playerMessage, true);
        Debug.Log(jsonText);

        return jsonText;
    }

    public void SetState(conversationState newState)
    {
        currentConverState = newState;
        Debug.Log("ConversationState -> " + newState);
        Debug.Log("========================================");
        ConverloopCase();
    }
}
