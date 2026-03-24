using UnityEngine;
using UnityEngine.InputSystem;

public class BladeController : MonoBehaviour
{
    public Camera cam;
    private TrailRenderer trail;

    void Start()
    {
        trail = GetComponent<TrailRenderer>();

        if (trail != null)
            trail.emitting = false;
    }

    void Update()
    {
        if (FoodGameManager.instance == null) return;
        if (FoodGameManager.instance.isGameOver) return;

        if (cam == null) return;

        // ✅ เช็ค input (รองรับทั้งเมาส์ + ทัช)
        bool isPressed = false;
        Vector2 inputPos = Vector2.zero;

        // 🖱️ Mouse
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            isPressed = true;
            inputPos = Mouse.current.position.ReadValue();
        }

        // 📱 Touch
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            isPressed = true;
            inputPos = Touchscreen.current.primaryTouch.position.ReadValue();
        }

        // 🔥 เปิด/ปิด trail
        if (isPressed)
        {
            if (trail != null)
                trail.emitting = true;

            Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(inputPos.x, inputPos.y, 10f));
            transform.position = worldPos;

            Ray ray = cam.ScreenPointToRay(inputPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Ingredient ing = hit.collider.GetComponent<Ingredient>();

                if (ing != null)
                {
                    ProcessCut(ing);
                }
            }
        }
        else
        {
            if (trail != null)
                trail.emitting = false;
        }
    }

    void ProcessCut(Ingredient ing)
    {
        if (OrderManager.instance == null) return;

        string target = OrderManager.instance.currentTarget;

        if (ing.ingredientName == target)
        {
            if (ScoreManager.instance != null)
                ScoreManager.instance.AddScore(10);

            if (ComboManager.instance != null)
                ComboManager.instance.AddCombo();
        }
        else
        {
            if (ComboManager.instance != null)
                ComboManager.instance.ResetCombo();

            if (CameraShake.instance != null)
                CameraShake.instance.Shake();
        }

        ing.Cut();
    }
}