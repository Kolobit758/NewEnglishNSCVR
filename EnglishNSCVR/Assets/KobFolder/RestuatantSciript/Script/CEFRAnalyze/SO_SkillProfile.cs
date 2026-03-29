using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillSystem/SkillProfile", fileName = "SkillProfile")]
public class SO_SkillProfile : ScriptableObject
{
    [Header("Level 1: Meaning")]
    public List<string> basicWords; // eat, want, water

    [Header("Level 2: Detail")]
    public List<string> detailWords; // very, really, more, little

    [Header("Level 3: Connector + Opinion")]
    public List<string> connectorWords; // and, but, because
    public List<string> opinionWords;   // I think, I like, I want

    [Header("Level 4: Advanced")]
    public List<string> advancedWords; // recommend, prefer, suggest
}