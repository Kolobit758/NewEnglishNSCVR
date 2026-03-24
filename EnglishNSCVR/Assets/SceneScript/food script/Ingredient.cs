using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public string ingredientName;

    private Vector3 rotationSpeed;

    void Start()
    {
        // 🎲 สุ่มความเร็วหมุน
        rotationSpeed = new Vector3(
            Random.Range(100f, 300f),
            Random.Range(100f, 300f),
            Random.Range(100f, 300f)
        );
    }

    void Update()
    {
        // 🔄 หมุนตลอดเวลา
        transform.Rotate(rotationSpeed * Time.deltaTime);

        // 🧹 ลบเมื่อหลุดจอ
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    public void Cut()
    {
        Destroy(gameObject);
    }
}