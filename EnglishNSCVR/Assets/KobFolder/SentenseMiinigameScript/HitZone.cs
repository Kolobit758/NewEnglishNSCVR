using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    // ใช้เก็บกล่องที่กำลังอยู่ในโซน (อาจจะมีมากกว่า 1 ถ้าลอยมาติดๆ กัน)
    public List<AwnserBoxInfo> boxesInZone = new List<AwnserBoxInfo>();
    public QuestionManager questionManager;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        if (other.gameObject.tag == "AwsBox")
        {
            boxesInZone.Add(other.gameObject.GetComponent<AwnserBoxInfo>());
        }


    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("out");
        if (other.gameObject.tag == "AwsBox")
        {
            boxesInZone.Remove(other.gameObject.GetComponent<AwnserBoxInfo>());
        }
    }

    // ฟังก์ชันนี้จะถูกเรียกเมื่อผู้เล่นกดปุ่ม
    public Question_Sound GetCurrentChoiceFirst()
    {
        if (boxesInZone.Count > 0)
        {
            // คืนค่าตัวที่อยู่ใกล้จุดศูนย์กลางที่สุด หรือตัวแรกที่เข้ามา

            return boxesInZone[0].choiceData;
        }
        return null; // กดแป้ก (ไม่มีกล่องอยู่ในโซน)
    }
    public string GetCurrentChoice()
    {
        if (boxesInZone.Count > 0)
        {
            return boxesInZone[0].data; // แค่ return data
        }
        return null;
    }

}
