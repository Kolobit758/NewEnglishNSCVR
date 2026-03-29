using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IgnoredWord" , menuName = "WordColecrtor/IgnoredWord")]
public class IgnoreWordList : ScriptableObject
{
    public List<string> ignoredWords = new List<string>();
}
