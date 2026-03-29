using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody; // ลากตัว Player (Capsule) มาใส่ตรงนี้

    float xRotation = 0f;

    void Start()
    {
        // ล็อคเมาส์ไว้กลางจอ และซ่อนตัวชี้เมาส์
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. รับค่าการขยับเมาส์
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 2. คำนวณการหมุนแนวตั้ง (ขึ้น-ลง)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ล็อคไม่ให้เงยหน้าจนตีลังกา

        // หมุนเฉพาะกล้องในแนวตั้ง
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 3. หมุนตัวผู้เล่นในแนวนอน (ซ้าย-ขวา)
        playerBody.Rotate(Vector3.up * mouseX);

        if (Input.GetKeyDown(KeyCode.X))
        {
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
