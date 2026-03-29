using UnityEngine;

public class NPCAnswerEvaluator : MonoBehaviour
{
    public bool CheckMeaning(string text, QandANPC question)
    {
        text = CleanText(text);

        int matchCount = 0;

        foreach (string keyword in question.keywords)
        {
            if (text.Contains(keyword.ToLower()))
            {
                matchCount++;
            }
        }

        // 🔥 flexible มากขึ้น
        float ratio = (float)matchCount / question.keywords.Count;

        if (ratio >= 0.5f)
            return true;

        return false;
    }

    string CleanText(string text)
    {
        text = text.ToLower();

        text = text.Replace(",", "");
        text = text.Replace(".", "");
        text = text.Replace("!", "");
        text = text.Replace("?", "");

        text = text.Replace("i'm", "i am");
        text = text.Replace("don't", "do not");

        return text;
    }
}