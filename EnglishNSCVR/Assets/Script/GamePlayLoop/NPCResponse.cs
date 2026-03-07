using System;

[System.Serializable]
public class NPCResponse
{
    public string ai_response;
    public string intent; // "order_food", "small_talk"
    public bool is_end_game;   // คำพูดที่ NPC จะพูด
    public EvaluationData evaluation;
}

[System.Serializable]
public class NPCOrderRequest
{
    public string npcId;
    public string intent; // "order_food"
    public OrderData order;
}


[System.Serializable]
public class OrderData
{
    public string food;
    public string beverage;
    public string topping;
    public taste taste;
    public bool isSpecial; 

    public OrderData(string food, string beverage,string topping,taste taste,bool isSpecial)
    {
        this.food = food;
        this.beverage = beverage;
        this.topping = topping;
        this.taste = taste;
        this.isSpecial = isSpecial;
    }
}

[Serializable]
public class EvaluationData {
    public string rank;
    public string strengths;
    public string improvements;
    public string next_rank_tip;

    public EvaluationData(string rank, string strengths,string improvements,string next_rank_tip)
    {
        this.rank = rank;
        this.strengths = strengths;
        this.improvements = improvements;
        this.next_rank_tip = next_rank_tip;
 
    }
}
