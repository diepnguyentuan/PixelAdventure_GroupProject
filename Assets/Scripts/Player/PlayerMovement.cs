using System;
using UnityEngine;
using UnityEngine.SceneManagement; // Cần thư viện này để reset màn chơi

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private float horizontalInput;
    private bool isGrounded;
    private bool isAlive = true; // Biến kiểm tra còn sống không

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isAlive) return; // Nếu chết rồi thì không nhận nút bấm nữa

        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        FlipSprite();
        UpdateAnimationUpdate();
    }

    void FixedUpdate()
    {
        if (!isAlive) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    void FlipSprite()
    {
        if (horizontalInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    // --- XỬ LÝ VA CHẠM (MỚI) ---

    // Khi va chạm với vật thể có Collider (như Quái, Bẫy)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu vật va chạm có Tag là "Trap" hoặc "Enemy"
        if (collision.gameObject.CompareTag("Trap") || collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    // Một số bẫy dùng Trigger (đi xuyên qua mới chết), dùng hàm này
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trap") || collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    private void UpdateAnimationUpdate()
    {
        // Gửi thông báo tốc độ chạy (lấy giá trị tuyệt đối vì đi lùi vẫn là chạy)
        anim.SetFloat("speed", Mathf.Abs(horizontalInput));

        // Gửi thông báo có chạm đất không
        anim.SetBool("isGrounded", isGrounded);

        // Gửi thông báo vận tốc trục Y (để biết nhảy lên hay rơi xuống)
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }
    void Die()
    {
        if (!isAlive) return;

        isAlive = false;
        rb.linearVelocity = Vector2.zero; // Dừng nhân vật lại
        Debug.Log("Player Died!");

        // Nhảy lên một chút kiểu Mario chết (Optional)
        rb.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
        GetComponent<Collider2D>().enabled = false; // Tắt va chạm để rơi xuyên đất

        // Reset lại màn chơi sau 1 giây
        Invoke("ReloadLevel", 1f);
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}