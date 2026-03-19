using System.Collections;
using UnityEngine;
using TMPro; // อย่าลืมใส่ถ้าใช้ TextMeshPro

public class AwnserBoxInfo : MonoBehaviour
{
    [Header("Data")]
    public Question_Sound choiceData;
    public QuestionManager questionManager;
    public string data;
    public TMP_Text textDisplay; // หรือ TextMeshProUGUI ถ้าเป็น UI
    [HideInInspector] public HitZone hitZone; // เพิ่มตัวนี้เพื่อไว้อ้างอิง

    [Header("Movement Settings")]
    public Transform targetTransform;
    public Transform startPosition;
    public float moveSpeed = 3f;
    public bool isSelected;


    [Header("Beat Effect")]
    [SerializeField] private Vector3 minSize;
    private Vector3 maxSize;
    [SerializeField] private float returnSpeed = 5f; // ความเร็วในการย่อกลับ

    private bool isMoving = false; // ✅ เริ่มหยุดก่อน

    [Header("Grid Movement")]
    public float stepDistance = 2f; // ระยะทาง 1 ช่อง (ต้องเท่ากับที่ใช้ใน SpawnProcess)
    public float smoothSpeed = 15f; // ความเร็วในการสไลด์ไปยังช่องถัดไป
    private Vector3 targetPos;
    [SerializeField] GameObject selectedBoxPrefab;
    [SerializeField] Transform selectedBoxSpawnPoint;
    // [SerializeField] Transform selectedBoxSpawnPos;
    public GameObject selecedtBox;

    [SerializeField] private Renderer boxRenderer; // ถ้าเป็น 3D
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color beatColor = Color.yellow;
    [SerializeField] private Color actionColor = Color.green;

    void Start()
    {
        if (boxRenderer == null)
            boxRenderer = GetComponent<Renderer>();

        boxRenderer.material.color = normalColor;
        // เซต targetPos ให้เท่ากับตำแหน่งที่มันเกิดมา ณ ตอนนั้น
        questionManager = FindAnyObjectByType<QuestionManager>();
        selectedBoxSpawnPoint = GameObject.Find("selectedBoxSpawnPoint").transform;
        targetPos = transform.position;
        startPosition = GameObject.Find("AwsBoxSpawnPoint").transform;
        // ตั้งค่าขนาดให้เรียบร้อยถ้าลืมเซตมาจาก Inspector
        if (minSize == Vector3.zero) minSize = transform.localScale;
        setMaxSize();

        GameObject endBox = GameObject.Find("AwsBoxEndPoint");
        hitZone = FindAnyObjectByType<HitZone>();
        if (endBox != null) targetTransform = endBox.transform;
    }

    void OnEnable()
    {
        BeatManager.OnBeat += SizeBeat; // เต้นตามจังหวะ (1 และ 0)
        BeatManager.OnActionBeat += MoveStep; // เดินเฉพาะจังหวะที่เป็น 1
    }

    void OnDisable()
    {
        BeatManager.OnBeat -= SizeBeat;
        BeatManager.OnActionBeat -= MoveStep;
    }

    void MoveStep()
    {
        targetPos += Vector3.left * stepDistance;

        // สีเขียวตอน "จังหวะสำคัญ"
        if (boxRenderer != null)
            boxRenderer.material.color = actionColor;
    }

    void Update()
    {
        if (!isSelected)
        {
            // สไลด์กล่องให้ไปถึง targetPos อย่างนุ่มนวลแต่รวดเร็ว
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);

            // เอฟเฟกต์ย่อตัว (Beat Visual)
            transform.localScale = Vector3.Lerp(transform.localScale, minSize, Time.deltaTime * returnSpeed);

            // เช็คว่าถึงจุดหมายหรือยัง
            if (targetTransform != null && transform.position.x <= targetTransform.position.x)
            {
                // ทำลายทิ้งหรือส่งคืน Pool (แนะนำให้ Destroy หรือเอาออกจาก List ใน HitZone ด้วย)
                ResetToStart();
            }
        }

        if (boxRenderer != null)
        {
            boxRenderer.material.color = Color.Lerp(
                boxRenderer.material.color,
                normalColor,
                Time.deltaTime * 5f
            );
        }

    }
    public void SetupFirstStep(Question_Sound data)
    {
        choiceData = data;
        this.data = data.question;
        if (textDisplay) textDisplay.text = this.data;
    }

    public void SetupOtherStep(string data)
    {
        this.data = data;
        if (textDisplay) textDisplay.text = data;
    }

    // เมื่อถึงจังหวะ ให้ขยายตัวทันที
    public void SizeBeat()
    {
        transform.localScale = maxSize;

        // กระพริบสี
        if (boxRenderer != null)
            boxRenderer.material.color = beatColor;

    }

    public void setMaxSize()
    {
        // ตั้งให้ขนาดตอนเต้นใหญ่กว่าปกติ 20-50% (ปรับตัวคูณตามความเหมาะสม)
        maxSize = minSize * 1.2f;
    }
    void ResetToStart()
    {
        if (startPosition != null)
        {
            // ✅ วาร์ปตัวถังกลับไปที่จุดเกิด
            transform.position = startPosition.position;

            // ✅ รีเซ็ตเป้าหมายการเดินให้เท่ากับจุดเริ่มต้น ไม่งั้นมันจะพยายามวิ่งไปข้างหน้าต่อทันที
            targetPos = startPosition.position;

            // (Option) ถ้าอยากให้พอกลับไปแล้วหยุดรอก่อนค่อยวิ่งตามบีตถัดไป
            // Logic ของเราตอนนี้มันจะเดินต่อใน OnActionBeat ถัดไปโดยอัตโนมัติอยู่แล้ว
        }
    }

    // ใน AwnserBoxInfo
    void OnDestroy()
    {
        // ✅ ป้องกัน Error: ถ้ากล่องโดนทำลาย ให้ลบตัวเองออกจาก List ใน HitZone ด้วย
        if (hitZone != null && hitZone.boxesInZone.Contains(this))
        {
            hitZone.boxesInZone.Remove(this);
        }
    }

    public void SelectBox()
    {
        isSelected = true;
        // selecedtBox = Instantiate(selectedBoxPrefab,selectedBoxSpawnPos);
        selecedtBox = Instantiate(selectedBoxPrefab, selectedBoxSpawnPoint);
        selecedtBox.GetComponent<SelectedBox>().Setup(data);
        questionManager.fillBoxInList(selecedtBox);

        Destroy(gameObject);
    }

    // public void Shake()
    // {
    //     StartCoroutine(ShakeRoutine());
    // }

    // IEnumerator ShakeRoutine()
    // {
    //     Vector3 original = transform.position;

    //     for (int i = 0; i < 5; i++)
    //     {
    //         transform.position = original + Random.insideUnitSphere * 0.1f;
    //         yield return new WaitForSeconds(0.02f);
    //     }

    //     transform.position = original;
    // }

}