using System;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int currentRound = 0;
    public gamestate currentGameState;
    bool isNPCSpawn = false;
    public static int maxRounds = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {

        GameEvent.OnNPCComming += NPC_ReachCounter;
        GameEvent.OnNPCOrdered += NPC_Ordered;
        GameEvent.OnPlayerConfirmedOrder += Player_Ordered;
        GameEvent.OnPlayerServed += Player_Served;
        GameEvent.OnNPCLeave += NPC_out;
        GameEvent.OnNPCLeaveFinished += setNPCSpawn;

    }


    void OnDisable()
    {

    }
    void Start()
    {
        isNPCSpawn = false;
        currentGameState = gamestate.NPC_comming;
        gameloopCase();
    }
    void Update()
    {

    }
    #region GameLoop function
    // delegate event to chage this gameloopcase
    void gameloopCase()
    {
        switch (currentGameState)
        {
            case gamestate.NPC_comming:
                if (!isNPCSpawn)
                {
                    isNPCSpawn = true;
                    Debug.Log("Call spawn");
                    SpawnNPC();
                }
                break;
            case gamestate.NPC_Order:

                break;
            case gamestate.Player_Order:

                break;
            case gamestate.Wait_Cooking:

                break;
            case gamestate.Player_Serve:

                break;
            case gamestate.NPC_Paid:
                GameEvent.OnNPCPaid?.Invoke();
                NPC_Paid();
                break;
            case gamestate.NPC_out:
                
                break;
        }
    }

    private void SpawnNPC()
    {
        Debug.Log("call spwan NPC");
        GameEvent.OnNPCSpawn?.Invoke();

    }

    public void NPC_ReachCounter()
    {
        //NPC ถึงโต้ะแล้ว 
        Debug.Log("reach counter");

        SetState(gamestate.NPC_Order);
    }
    public void NPC_Ordered()
    {

        //get json from gpt and order food to player [NPC Sound]
        // and change currentstate to player order
        Debug.Log("NPC ordered and Player order");
        SetState(gamestate.Player_Order);

    }
    public void Player_Ordered()
    {
        //when click order confirm all recipe sent to cooker and chage state to wait time
        SetState(gamestate.Wait_Cooking);

    }
    public void Player_Served()
    {
        // serve dishes
        SetState(gamestate.NPC_Paid);
    }
    public void NPC_Paid()
    {
        SetState(gamestate.NPC_out);

    }
    public void NPC_out()
    {
        //NPC go out from shop

        SetState(gamestate.NPC_comming);
        currentRound += 1;
        if(currentRound == maxRounds)
        {
            GameEvent.OnGameEnding?.Invoke();
        }

    }
    public void setNPCSpawn()
    {
        if(currentRound == maxRounds) return;
        isNPCSpawn = false;
        if (!isNPCSpawn)
        {
            isNPCSpawn = true;
            Debug.Log("Call spawn");
            SpawnNPC();
        }
    }
    #endregion

    public void SetState(gamestate newState)
    {
        currentGameState = newState;
        Debug.Log("GameState -> " + newState);
        Debug.Log("========================================");
        gameloopCase();
    }

}
