using UnityEngine;
using Unity.Cinemachine; // QUAN TRỌNG: Dùng thư viện mới này

public class RoomCamSwitcher : MonoBehaviour
{
    // Ở bản cũ là CinemachineVirtualCamera, bản mới tên ngắn gọn là CinemachineCamera
    public CinemachineCamera roomCamera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            // Set độ ưu tiên cao lên để chiếm sóng
            roomCamera.Priority = 10;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            // Trả về mặc định
            roomCamera.Priority = 0;
        }
    }
}