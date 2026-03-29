
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ConverRulebase_Manager_New : MonoBehaviour

{
    public List<SO_WordIntent> wordIntents = new List<SO_WordIntent>();
    [SerializeField] private intetnts textIntent;
    [SerializeField] private NPCResponse nPCResponse;
    [SerializeField] private SO_WordIntent thisWordIntent;
    [SerializeField] private OrderData currentOrder;
    [SerializeField] private int total_cost;
    string orderTaste;

    public NPCResponse NPC_Rulebase_Response(string text)
    {
        textIntent = intetnts.Unidentified;

        text = text.ToLower();


        foreach (SO_WordIntent intent in wordIntents)
        {
            foreach (string word in intent.keywords)
            {
                // เช็คว่าในประโยคมี Keyword ของ Intent นั้นๆ ไหม
                if (text.Contains(word.ToLower()))
                {
                    // --- กรณีพิเศษ: ถ้าเป็น Intent จ่ายเงิน ---
                    if (intent.intetnts == intetnts.PaidMoney)
                    {
                        // เช็คว่าในประโยคมี "ตัวเลข" หรือมี "ราคาที่ถูกต้อง" หรือเปล่า
                        if (IsPaymentValid(text))
                        {
                            thisWordIntent = intent;
                            textIntent = intent.intetnts;
                            goto found;
                        }
                        else
                        {
                            // ถ้ามีคำว่า "bath" หรือ "pay" แต่ไม่มีเลขราคาที่ถูกต้อง 
                            // ให้ข้ามไปดู Intent อื่น (เผื่อเขาแค่พูดถึงเงินแต่ยังไม่ได้จ่าย)
                            continue;
                        }
                    }


                    // --- กรณีปกติ: Intent อื่นๆ (Order, Greeting, etc.) ---
                    thisWordIntent = intent;
                    textIntent = intent.intetnts;
                    goto found;
                }
            }
        }

    found:

        switch (textIntent)
        {
            case intetnts.Order:
                nPCResponse = Response_Order(thisWordIntent);
                break;

            case intetnts.PaidMoney:
                nPCResponse = Response_PaidMoney(thisWordIntent);
                break;
            case intetnts.Serve:
                nPCResponse = Response_Serve(thisWordIntent);
                break;
            case intetnts.Normal:
                nPCResponse = Response_Ask_Normal(thisWordIntent);
                break;
            case intetnts.Unidentified:
                nPCResponse = Response_Unidentified(thisWordIntent);
                break;

        }

        return nPCResponse;
    }


    private NPCResponse Response_Unidentified(SO_WordIntent wordIntent)
    {
        NPCResponse response = new NPCResponse("Sorry, Can you say again.", "Unidentified", false, null);
        return response;

    }

    private NPCResponse Response_Ask_Normal(SO_WordIntent wordIntent)
    {
        int ranFormatNum = Random.Range(0, wordIntent.dialogueFormat.Count);
        int ranResNum = Random.Range(0, wordIntent.resWords.Count);
        string resText = wordIntent.dialogueFormat[ranFormatNum].Replace(wordIntent.placeHolderWord, wordIntent.resWords[ranResNum]);

        NPCResponse response = new NPCResponse(resText, "normal", false, null);
        return response;
    }



    private NPCResponse Response_Serve(SO_WordIntent wordIntent)
    {
        int ranFormatNum = Random.Range(0, wordIntent.dialogueFormat.Count);
        int ranResNum = Random.Range(0, wordIntent.resWords.Count);
        string resText = wordIntent.dialogueFormat[ranFormatNum].Replace("{noun}", wordIntent.resWords[ranResNum]);

        NPCResponse response = new NPCResponse(resText, "delivery", true, null);
        return response;
    }

    private NPCResponse Response_PaidMoney(SO_WordIntent wordIntent)
    {
        int ranFormatNum = Random.Range(0, wordIntent.dialogueFormat.Count);
        string resText = wordIntent.dialogueFormat[ranFormatNum].Replace("{total_cost}", total_cost.ToString());

        NPCResponse response = new NPCResponse(resText, "pay", false, null);
        return response;
    }

    private NPCResponse Response_Order(SO_WordIntent wordIntent)
    {
        int ranFormatNum = Random.Range(0, wordIntent.dialogueFormat.Count);
        string isSpecialText;

        if (currentOrder.isSpecial == true)
        {
            isSpecialText = "special";
        }
        else
        {
            isSpecialText = "";
        }
        switch (currentOrder.taste)
        {
            case taste.Sweet:
                orderTaste = "sweet";
                break;
            case taste.Spicy:
                orderTaste = "spicy";
                break;
            case taste.Salty:
                orderTaste = "salty";
                break;
            case taste.Normal:
                orderTaste = "";
                break;


        }
        string resText = wordIntent.dialogueFormat[ranFormatNum].Replace("{food}", currentOrder.food).Replace("{beverage}", currentOrder.beverage).Replace("{topping}", currentOrder.topping).Replace("{taste}", orderTaste).Replace("{special}", isSpecialText);


        NPCResponse response = new NPCResponse(resText, "order", false, null);
        return response;
    }

    private bool IsPaymentValid(string text)
    {
        // ใช้ Regex ดึงตัวเลขทั้งหมดที่อยู่ในประโยคออกมา
        MatchCollection matches = Regex.Matches(text, @"\d+");

        foreach (Match match in matches)
        {
            if (int.TryParse(match.Value, out int amount))
            {
                // ถ้าตัวเลขที่พิมพ์มา ตรงกับราคาสินค้า (หรือมากกว่า)
                if (amount >= total_cost && total_cost > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void setCurentOrder(OrderData orderData)
    {
        currentOrder.food = orderData.food;
        currentOrder.beverage = orderData.beverage;
        currentOrder.taste = orderData.taste;
        currentOrder.topping = orderData.topping;
        currentOrder.isSpecial = orderData.isSpecial;
    }
    public void updateTotalCost(int cost)
    {
        total_cost = cost;
    }
    // 🔥 เพิ่มฟังก์ชันนี้ไว้ล่างๆ ของไฟล์
    public intetnts GetIntent(string text)
    {
        text = text.ToLower();

        foreach (SO_WordIntent intent in wordIntents)
        {
            foreach (string word in intent.keywords)
            {
                if (text.Contains(word.ToLower()))
                {
                    if (intent.intetnts == intetnts.PaidMoney)
                    {
                        if (IsPaymentValid(text))
                            return intent.intetnts;
                        else
                            continue;
                    }

                    return intent.intetnts;
                }
            }
        }

        return intetnts.Unidentified;
    }
}
