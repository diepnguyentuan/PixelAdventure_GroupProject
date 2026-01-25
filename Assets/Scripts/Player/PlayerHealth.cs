using UnityEngine;
using UnityEngine.UI; // B?t bu?c ?? ch?nh s?a UI
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settting Health")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("UI Interface")]
    public Image[] hearts; // M?ng ch?a 3 c�i ?nh Heart tr�n Canvas
    public Sprite fullHeart; // ?nh tim ??y
    public Sprite emptyHeart; // ?nh tim r?ng (ho?c null n?u mu?n t?t ?i)

    private bool isInvincible = false; // Tr?ng th�i b?t t? t?m th?i sau khi b? ?�nh
    private Animator anim;
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // B?t ??u game v?i ??y m�u
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    // H�m nh?n s�t th??ng
    public void TakeDamage(int damage)
    {
        // N?u ?ang b?t t? ho?c ?� ch?t th� kh�ng tr? m�u n?a
        if (isInvincible || currentHealth <= 0) return;

        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth > 0)
        {
            // B? ?au nh?ng ch?a ch?t
            StartCoroutine(BecomeInvincible());

            // Hi?u ?ng b? ??y l�i nh? (Knockback) cho c?m gi�c va ch?m
            rb.linearVelocity = new Vector2(0, 5f); // Nh?y l�n m?t ch�t
            anim.SetTrigger("hurt"); // N?u c� animation b? th??ng (Optional)
        }
        else
        {
            // H?t m�u -> Ch?t
            Die();
        }
    }

    void UpdateHealthUI()
    {
        // Duy?t qua 3 c�i tim
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                // N?u ch? s? i nh? h?n m�u hi?n t?i -> Hi?n th? tim ??y
                hearts[i].sprite = fullHeart;
                hearts[i].enabled = true;
            }
            else
            {
                // Ng??c l?i -> Hi?n th? tim r?ng (ho?c ?n ?i)
                if (emptyHeart != null)
                    hearts[i].sprite = emptyHeart;
                else
                    hearts[i].enabled = false; // N?u kh�ng c� ?nh tim r?ng th� ?n lu�n
            }
        }
    }

    // C? ch? b?t t? t?m th?i (I-frames) trong 1 gi�y ?? kh�ng b? tr? 3 m�u li�n t?c
    IEnumerator BecomeInvincible()
    {
        isInvincible = true;

        // L�m nh�n v?t nh?p nh�y (Gi?m ?? trong su?t)
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);

        yield return new WaitForSeconds(1f); // B?t t? 1 gi�y

        // Tr? l?i b�nh th??ng
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        isInvincible = false;
    }

    void Die()
    {
        // G?i l?i logic ch?t gi?ng b�i tr??c
        anim.SetTrigger("death");
        rb.bodyType = RigidbodyType2D.Static;
        Invoke("RestartLevel", 1.5f);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}