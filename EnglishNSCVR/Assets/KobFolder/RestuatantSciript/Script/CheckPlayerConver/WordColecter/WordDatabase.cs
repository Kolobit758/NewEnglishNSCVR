using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WordEntry
{
    public string word;
    public string thaiMeaning;
    public string englishMeaning;
    public bool isLearned;
}

[CreateAssetMenu(fileName = "WordDatabase",menuName = "WordColecrtor/WordDatabase")]
public class WordDatabase : ScriptableObject
{
    public List<WordEntry> wordEntries;
}
