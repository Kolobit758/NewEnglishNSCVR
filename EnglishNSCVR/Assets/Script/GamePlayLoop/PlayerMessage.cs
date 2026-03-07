[System.Serializable]
public class PlayerMessage
{
    // public string npcId;        // คุยกับ NPC ตัวไหน
    public string player_name;   // สิ่งที่ player พูด
    public string player_message;      // state ตอนนั้น (order / chat)

    public PlayerMessage(string name, string message)
    {
        player_name = name;
        player_message = message;
    }
}
