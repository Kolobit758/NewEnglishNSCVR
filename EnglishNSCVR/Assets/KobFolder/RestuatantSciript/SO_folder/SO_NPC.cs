using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCStoryNode
{
    public string topic; // เช่น "work", "family", "dream"

    [TextArea]
    public string npcResponse; // สิ่งที่ NPC จะพูด

    public List<string> requiredKeywords; // player ต้องถาม/พูดอะไรถึงจะ unlock

    public int requiredAffinity; // ต้องสนิทระดับไหน
    public List<int> nextNodeIndex;

    public bool isUnlocked; // runtime
}

[CreateAssetMenu(fileName = "npcBehavior", menuName = "NPC/new_npc")]
public class SO_NPC : ScriptableObject
{
    public string npcName;
    public int money;
    public personality personality;
    public GameObject prefab;

    [Header("Relationship")]
    public int affinity; // ความสนิท

    [Header("Story")]
    public List<NPCStoryNode> storyNodes;

}
