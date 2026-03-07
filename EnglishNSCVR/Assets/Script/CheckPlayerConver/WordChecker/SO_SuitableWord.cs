using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WordChecker/SuitableWord",fileName ="SuitableWord")]
public class SO_SuitableWord : ScriptableObject
{
    public string situation;
    public List<VocabularyScore> words = new List<VocabularyScore>();
}
