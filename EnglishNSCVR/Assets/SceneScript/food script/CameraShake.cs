using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    public Transform cam;

    void Awake()
    {
        instance = this;
    }

    public void Shake()
    {
        StartCoroutine(DoShake());
    }

    IEnumerator DoShake()
    {
        Vector3 originalPos = cam.position;

        float duration = 0.2f;
        float magnitude = 0.2f;

        while (duration > 0)
        {
            cam.position = originalPos + Random.insideUnitSphere * magnitude;
            duration -= Time.deltaTime;
            yield return null;
        }

        cam.position = originalPos;
    }
}