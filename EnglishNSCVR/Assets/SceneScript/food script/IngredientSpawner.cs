using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    public GameObject[] ingredients;

    public float spawnInterval = 0.8f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnIngredient), 1f, spawnInterval);
    }

    void SpawnIngredient()
    {
        if (FoodGameManager.instance != null && FoodGameManager.instance.isGameOver)
            return;

        int spawnCount = Random.Range(2, 5); // 🔥 2-4 ชิ้นต่อรอบ

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnOne();
        }
    }

    void SpawnOne()
    {
        Camera cam = Camera.main;

        int direction = Random.Range(0, 3); // 0=ล่าง 1=ซ้าย 2=ขวา

        Vector3 spawnPos = Vector3.zero;
        Vector3 velocity = Vector3.zero;

        float depth = 10f;

        // 🔼 จากล่าง
        if (direction == 0)
        {
            float x = cam.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), 0, depth)).x;
            float y = cam.ViewportToWorldPoint(new Vector3(0, 0, depth)).y - 1f;

            spawnPos = new Vector3(x, y, 0);
            velocity = new Vector3(Random.Range(-3f, 3f), Random.Range(10f, 12f), 0);
        }
        // ▶ จากซ้าย
        else if (direction == 1)
        {
            float x = cam.ViewportToWorldPoint(new Vector3(0, 0, depth)).x - 1f;
            float y = cam.ViewportToWorldPoint(new Vector3(0, Random.Range(0f, 1f), depth)).y;

            spawnPos = new Vector3(x, y, 0);
            velocity = new Vector3(Random.Range(8f, 12f), Random.Range(2f, 5f), 0);
        }
        // ◀ จากขวา
        else
        {
            float x = cam.ViewportToWorldPoint(new Vector3(1, 0, depth)).x + 1f;
            float y = cam.ViewportToWorldPoint(new Vector3(0, Random.Range(0f, 1f), depth)).y;

            spawnPos = new Vector3(x, y, 0);
            velocity = new Vector3(Random.Range(-12f, -8f), Random.Range(2f, 5f), 0);
        }

        int index = Random.Range(0, ingredients.Length);
        GameObject obj = Instantiate(ingredients[index], spawnPos, Quaternion.identity);

        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;

            rb.velocity = velocity;

            // 🔥 หมุนตอนลอย
            rb.angularVelocity = Random.insideUnitSphere * 5f;
        }

        // 🎲 สุ่มขนาด
        float scale = Random.Range(0.3f, 0.6f);
        obj.transform.localScale = new Vector3(scale, scale, scale);
    }
}