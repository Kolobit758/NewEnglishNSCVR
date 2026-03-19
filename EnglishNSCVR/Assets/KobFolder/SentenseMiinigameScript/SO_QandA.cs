using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question_Sound
{
    public string question;
    public AudioClip sound;
    public string awnserThisQuestion;
}

[CreateAssetMenu(menuName = "SentenseMode/SO_QandA" )]
public class SO_QandA : ScriptableObject
{
    public List<Question_Sound> questions = new List<Question_Sound>();
    public List<string> answers = new List<string>();
    public List<string> words = new List<string>();
}
