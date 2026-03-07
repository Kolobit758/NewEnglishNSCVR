using UnityEngine;

[CreateAssetMenu(fileName = "npcBehavior", menuName = "NPC/new_npc")]
public class SO_NPC : ScriptableObject
{
    public string npcName;
    public int money;
    public personality personality;
    public GameObject prefab;
}
