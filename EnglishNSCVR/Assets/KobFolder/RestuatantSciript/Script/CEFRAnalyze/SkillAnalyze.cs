using System.Collections.Generic;
using UnityEngine;


public class SkillAnalyze : MonoBehaviour
{
    public SO_SkillProfile profile;

    public SkillFeature Analyze(string text)
    {
        text = CleanText(text);

        SkillFeature f = new SkillFeature();

        string[] words = text.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

        foreach (var word in words)
        {
            if (profile.basicWords.Contains(word))
                f.hasMeaning = true;

            if (profile.detailWords.Contains(word))
                f.hasDetail = true;

            if (profile.connectorWords.Contains(word))
                f.hasConnector = true;

            if (profile.advancedWords.Contains(word))
                f.hasAdvanced = true;
        }

        // 🔥 opinion ต้องใช้ Contains (เพราะเป็น phrase เช่น "i think")
        foreach (var phrase in profile.opinionWords)
        {
            if (text.Contains(phrase))
                f.hasOpinion = true;
        }

        f.wordCount = words.Length;

        return f;
    }
    public int CalculateLevel(SkillFeature f, SkillSession s)
    {
        int score = 0;

        if (f.hasMeaning) score += 1;
        if (f.hasDetail) score += 1;
        if (f.hasConnector) score += 1;
        if (f.hasOpinion) score += 1;
        if (f.hasAdvanced) score += 1;

        // 🔥 ความยาวช่วยเพิ่มคะแนน
        if (f.wordCount >= 6) score += 1;

        // 🔥 ความต่อเนื่องของบทสนทนา
        if (s.meaningfulTurnCount >= 3)
            return 5;

        if (score >= 5) return 4;
        if (score >= 3) return 3;
        if (score >= 2) return 2;
        return 1;
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