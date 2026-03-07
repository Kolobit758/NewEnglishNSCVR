using System;
using UnityEngine;

public class Button3D : MonoBehaviour
{
    public Action onClick;

    public void Init(Action callback)
    {
        onClick = callback;
    }

    void OnMouseDown()
    {
        onClick?.Invoke();
    }
}
