using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SitData
{
    public Transform sitPos;
    public bool isSited;
}
public class SitPosScript : MonoBehaviour
{
    public List<SitData> sitPos = new List<SitData>();
    
    public Transform GetSitPOS()
    {
        int ranNum = Random.Range(0, sitPos.Count);
        while (sitPos[ranNum].isSited)
        {
            ranNum = Random.Range(0, sitPos.Count);
            
        }

        sitPos[ranNum].isSited = true;
        return sitPos[ranNum].sitPos;
    }
}
