using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public abstract class SO_MenuItem : ScriptableObject
{
    public string Name;
    public int price;
    public float time;
    public Sprite ItemImage;
    public GameObject prefab;
}
