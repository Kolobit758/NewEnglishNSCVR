using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class PlayerMsgToServer : MonoBehaviour
{

    string serverUrl = "http://localhost:5108/npc/chat";
    public bool isTest = false;
    public void SendMessageToServer(string text, System.Action<NPCResponse> onResponseReceived)
    {
        PlayerMessage msg = new PlayerMessage("kob", text);

        string json = JsonUtility.ToJson(msg);
        Debug.Log(json);
        StartCoroutine(Post(json, onResponseReceived));

    }

    IEnumerator Post(string json, System.Action<NPCResponse> onResponseReceived)
    {
        byte[] body = Encoding.UTF8.GetBytes(json);

        UnityWebRequest req = new UnityWebRequest(serverUrl, "POST");
        req.uploadHandler = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        if(isTest == false)
        {
            yield return req.SendWebRequest();
        }else
        {
            yield return null;
        }
        
        if (req.result == UnityWebRequest.Result.Success )
        {
            string responseText = req.downloadHandler.text;
            Debug.Log("Server Raw Response: " + responseText);

            // แปลง JSON ตรงนี้เลย
            NPCResponse res = JsonUtility.FromJson<NPCResponse>(responseText);
            onResponseReceived?.Invoke(res);
        }
        else
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("npcEndConverResponse");
            NPCResponse res = JsonUtility.FromJson<NPCResponse>(jsonFile.text);
            onResponseReceived?.Invoke(res);
            Debug.LogError("Error: " + req.error);
        }
    }
}
