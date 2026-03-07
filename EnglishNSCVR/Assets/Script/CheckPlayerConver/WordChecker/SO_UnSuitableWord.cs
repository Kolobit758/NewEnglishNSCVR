using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WordChecker/UnSuitableWord",fileName ="UnSuitableWord")]
public class SO_UnSuitableWord : ScriptableObject
{
    public string situation;
    public List<VocabularyScore> words = new List<VocabularyScore>();
}
