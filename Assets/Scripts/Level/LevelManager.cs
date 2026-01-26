using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private int totalFruits;
    private int fruitsCollected = 0;

    [Header("Cài đặt Cửa & Đích")]
    public GameObject levelGate; // MỚI: Kéo cái Tilemap LevelGate vào đây
    public GameObject finishPoint; // Cái cúp (nếu vẫn dùng)

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        totalFruits = GameObject.FindGameObjectsWithTag("Fruit").Length;
        Debug.Log("GAME START: LevelManager đếm được tổng cộng " + totalFruits + " quả trong map.");
        // Đảm bảo lúc đầu cửa phải ĐÓNG (Hiện lên)
        if (levelGate != null) levelGate.SetActive(true);
    }

    public void AddFruit()
    {
        fruitsCollected++;
        CheckLevelCompleted(); // Kiểm tra mỗi khi ăn
    }

    void CheckLevelCompleted()
    {
        if (fruitsCollected >= totalFruits)
        {
            OpenGate();
        }
    }

    void OpenGate()
    {
        // 1. Tắt bức tường đi -> Đường mở ra
        if (levelGate != null)
        {
            levelGate.SetActive(false);
            // Gợi ý: Sau này có thể thêm hiệu ứng nổ bùm bùm ở đây cho đẹp
        }

        // 2. Hiện cái đích đến (nếu cần)
        if (finishPoint != null) finishPoint.SetActive(true);

        Debug.Log("Cửa đã mở! Mời qua màn 2.");
    }
}