using UnityEngine;

public class CheckSuitableWord : MonoBehaviour
{
    [SerializeField]SO_SuitableWord suitableWord;
    [SerializeField]SO_UnSuitableWord unSuitableWord;

    [SerializeField]Gamesubmary gamesubmary;
    public void CheckWord(string text)
    {
        text = text.ToLower();

        foreach(VocabularyScore word in suitableWord.words)
        {
            if (text.Contains(word.word.ToLower()))
            {
                gamesubmary.SuitableWordScore += word.score;
            }
            
        }
        foreach(VocabularyScore word in unSuitableWord.words)
        {
            if (text.Contains(word.word.ToLower()))
            {
                gamesubmary.SuitableWordScore += word.score;
            }
            
        }

        Debug.Log("score : " + gamesubmary.SuitableWordScore);
    }
}
