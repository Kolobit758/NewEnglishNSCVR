using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ConverRulebase/Intent" , fileName = "WordIntent")]
public class SO_WordIntent : ScriptableObject
{
    public intetnts intetnts;
    public List<string> words = new List<string>();
    public List<string> resWords = new List<string>();
    public List<string> dialogueFormat = new List<string>();
}
