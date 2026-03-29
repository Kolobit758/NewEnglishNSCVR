using System.Collections.Generic;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    public IgnoreWordList ignoreWordList;
    public WordDatabase wordDatabase;
    public List<string> SystemdebugWords;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    public void CheckIsNewWord(string response)
    {
        string[] words = response.Split(' ');

        foreach (string word in words)
        {
            string cleanWord = word.ToLower().Trim('.', ',', '/', '?', '!');

            if (IsIgnoredWord(cleanWord)) continue;
            if(cleanWord.Length <= 2) continue;
            if(CheckIsSystemWord(cleanWord)) continue;
            if (!WordExists(cleanWord))
            {
                AddWordToDb(cleanWord);
            }
        }
    }

    bool IsIgnoredWord(string word)
    {
        bool isIgnored = false;
        foreach (string ignoredWord in ignoreWordList.ignoredWords)
        {
            if (word == ignoredWord.ToLower())
            {
                isIgnored = true;
            }

        }

        return isIgnored;
    }
    bool WordExists(string word)
    {
        bool isFound = false;
        foreach (WordEntry existsWord in wordDatabase.wordEntries)
        {
            if (word == existsWord.word.ToLower())
            {
                isFound = true;
            }

        }

        return isFound;
    }
    bool CheckIsSystemWord(string word)
    {
        bool isFound = false;
        foreach(string SystemWord in SystemdebugWords)
        {
            if(word == SystemWord.ToLower())
            {
                isFound = true;
            }
        }
        return isFound;
    }

    void AddWordToDb(string word)
    {
        if (word != null)
        {
            WordEntry newWord = new WordEntry();
            newWord.word = word;
            newWord.englishMeaning = "";
            newWord.thaiMeaning = "";

            wordDatabase.wordEntries.Add(newWord);
        }
    }
}
