using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    // Hủy object này sau 0.5 giây
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }
}