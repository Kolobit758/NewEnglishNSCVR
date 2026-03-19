using UnityEngine;
using UnityEngine.InputSystem; // ต้องเพิ่ม namespace นี้
public class PlayerCallHitZone : MonoBehaviour
{
    [SerializeField]HitZone hitzone;
    public QuestionManager questionManager;

    void Start()
    {
        hitzone = gameObject.GetComponent<HitZone>();
    }

    void Update()
    {
        KeyboardInput();
    }

    void KeyboardInput()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            questionManager.ChooseChoice();
        }
    }
}
