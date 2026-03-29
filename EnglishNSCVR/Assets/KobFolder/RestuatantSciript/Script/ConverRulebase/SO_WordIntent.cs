using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ConverRulebase/Intent" , fileName = "WordIntent")]
public class SO_WordIntent : ScriptableObject
{
    public intetnts intetnts;
    public string subType;// ex Normal is Mun and the child subtypr is wheather .

    public List<string> keywords = new List<string>();
    public List<string> resWords = new List<string>();
    public List<string> dialogueFormat = new List<string>();
    public string placeHolderWord;
}
