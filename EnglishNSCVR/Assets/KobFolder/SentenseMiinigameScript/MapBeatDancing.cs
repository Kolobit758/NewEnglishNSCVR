using UnityEngine;

public class MapBeatDancing : MonoBehaviour
{
    public Vector3 maxSize;
    public Vector3 minSize;
    public Vector3 criSize;
    public float returnSpeed = 0.5f;
    public float criMultiply = 5f;
    public float maxMultiply = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (minSize == Vector3.zero) minSize = transform.localScale;
        setMaxSize();
        setCriSize();
    }
    void OnEnable()
    {
        BeatManager.OnBeat += SizeBeat; // เต้นตามจังหวะ (1 และ 0)
        BeatManager.OnActionBeat += CriBeat; // เดินเฉพาะจังหวะที่เป็น 1
    }

    void OnDisable()
    {
        BeatManager.OnBeat -= SizeBeat;
        BeatManager.OnActionBeat -= CriBeat;
    }
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, minSize, Time.deltaTime * returnSpeed);
    }
    // Update is called once per frame
    public void SizeBeat()
    {
        transform.localScale = maxSize;

    }
    public void CriBeat()
    {
        transform.localScale = maxSize;

    }

    public void setMaxSize()
    {
        // ตั้งให้ขนาดตอนเต้นใหญ่กว่าปกติ 20-50% (ปรับตัวคูณตามความเหมาะสม)
        maxSize = minSize * maxMultiply;
    }
    public void setCriSize()
    {
        // ตั้งให้ขนาดตอนเต้นใหญ่กว่าปกติ 20-50% (ปรับตัวคูณตามความเหมาะสม)
        criSize = minSize * criMultiply;
    }
}
