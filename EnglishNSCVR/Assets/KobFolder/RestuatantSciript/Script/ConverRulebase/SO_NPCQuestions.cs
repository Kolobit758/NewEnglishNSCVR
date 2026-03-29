using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QandANPC
{
    [Header("Question")]
    public string question;
    public string awnser;
    [Header("optional")]
    public List<string> keywords = new List<string>();
    [Header("Score")]
    public int baseScore = 5;     // ได้ตอบ = ได้คะแนนพื้นฐาน
    public int bonusScore = 5;    // ถ้ามี keyword เพิ่ม    
    public List<QandANPC> nextQuestions;
}
[CreateAssetMenu(menuName = "ConverRulebase/NPCQuestion_NEW", fileName = "NPCQuestion")]
public class SO_NPCQuestions : ScriptableObject
{

    public List<QandANPC> QandANPCs = new List<QandANPC>();


}