using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class NPC_Controller : MonoBehaviour
{
    public bool isTest = false;
    public WordManager wordManager;
    public OrderController orderController;
    public ConverRulebase_Manager converRulebase_Manager;
    public List<SO_NPC> npcs = new List<SO_NPC>();
    public GameObject currrentNPC;
    public float speed = 5;
    public Transform outScenePoint;
    public Transform targetPoint;
    public Transform spawnPoint;
    private bool isMoving = false;
    public int moveState = 0;


    string serverUrl = "http://localhost:5108/npc/aiFoodjson";
    void OnEnable()
    {
        GameEvent.OnNPCSpawn += SpawnNPC;
        GameEvent.OnNPCPaid += SetMoveState;

    }
    void OnDisable()
    {
        GameEvent.OnNPCSpawn -= SpawnNPC;
        GameEvent.OnNPCPaid -= SetMoveState;
    }
    void Start()
    {

    }
    void Update()
    {
        if (moveState == 0)
        {
            Debug.Log("walk");
            WalkToPoint();
        }
        else
        {
            Debug.Log("out");
            NPCOutScene();
        }

    }

    public void SpawnNPC()
    {
        Debug.Log("=============spawn=========");
        int randomNum = Random.Range(0, npcs.Count);

        SO_NPC selectedNPC = npcs[randomNum];

        currrentNPC = Instantiate(selectedNPC.prefab, spawnPoint.position, Quaternion.identity);
        currrentNPC.GetComponent<NPCscript>().PlayNormalFace();
        isMoving = true;
        RandomMeal();
    }

    public void showMessageFromJson(NPCResponse npcResponse)
    {
        currrentNPC.GetComponent<NPCscript>().updaeteUI(npcResponse.ai_response);
        wordManager.CheckIsNewWord(npcResponse.ai_response);
    }
    void WalkToPoint()
    {

        if (currrentNPC == null) return;
        if (!isMoving) return;
        // Move the instantiated prefab towards the target
        float step = speed * Time.deltaTime;
        currrentNPC.transform.position = Vector3.MoveTowards(currrentNPC.transform.position, targetPoint.position, step);

        // Check if the NPC has reached the point
        if (Vector3.Distance(currrentNPC.transform.position, targetPoint.position) < 0.1f)
        {
            isMoving = false;
            GameEvent.OnNPCComming?.Invoke();
            Debug.Log("move state : " + moveState);
        }
    }
    void NPCOutScene()
    {

        if (currrentNPC == null) return;
        if (!isMoving) return;
        // Move the instantiated prefab towards the target
        float step = speed * Time.deltaTime;
        currrentNPC.transform.position = Vector3.MoveTowards(currrentNPC.transform.position, outScenePoint.position, step);

        // Check if the NPC has reached the point
        if (Vector3.Distance(currrentNPC.transform.position, outScenePoint.position) < 0.1f)
        {
            isMoving = false;
            GameEvent.OnNPCLeave?.Invoke();
            Destroy(currrentNPC);
            GameEvent.OnNPCLeaveFinished?.Invoke();
            moveState = 0;
            Debug.Log("move state : " + moveState);
        }
    }

    void SetMoveState()
    {

        moveState = 1;
        isMoving = true;
        Debug.Log("swicth to out state : " + moveState);
    }

    #region RandomMealToserver
    public void RandomMeal()
    {
        OrderData order = orderController.RandomOrder();

        SendMessageToServer(order.food,order.beverage,order.topping,order.taste,order.isSpecial);
    }

    public void SendMessageToServer(string food, string beverage, string topping, taste taste, bool isSpecial)
    {
        OrderData msg = new OrderData(food, beverage, topping, taste, isSpecial);

        string json = JsonUtility.ToJson(msg);
        Debug.Log(json);
        StartCoroutine(Post(json));

    }

    IEnumerator Post(string json)
    {
        byte[] body = Encoding.UTF8.GetBytes(json);

        UnityWebRequest req = new UnityWebRequest(serverUrl, "POST");
        req.uploadHandler = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        if (isTest == false)
        {
            yield return req.SendWebRequest();
            Debug.Log(req.downloadHandler.text);
        }


    }
    #endregion

    public void PlayHappyFace()
    {
        currrentNPC.GetComponent<NPCscript>().PlayHappyFace();
    }
    public void PlayAngryFace()
    {
        currrentNPC.GetComponent<NPCscript>().PlayAngryFace();
    }
}
