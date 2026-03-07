using TMPro;
using UnityEngine;

public class NPCscript : MonoBehaviour
{
    public TMP_Text npcMsssage;

    
    public void updaeteUI(string msg)
    {
        npcMsssage.text = msg;
    }

    public void PlayHappyFace()
    {
        //happy face
        Debug.Log("NPC face : Happy face");
    }
    public void PlayAngryFace()
    {
        //angry fave
        Debug.Log("NPC face : Angry face");
    }
    public void PlayNormalFace()
    {
        //normal face
        Debug.Log("NPC face : Normal face");
    }
}
