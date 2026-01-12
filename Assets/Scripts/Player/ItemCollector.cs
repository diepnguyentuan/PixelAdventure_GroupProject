using UnityEngine;

public class ItemCollec : MonoBehaviour
{
    private int fruitCount = 0;

    [Header("Effects")]
    [SerializeField] private GameObject collectionEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fruit"))
        {
            if (collectionEffect != null)
            {
                Instantiate(collectionEffect, collision.transform.position, Quaternion.identity);
            }
            fruitCount++;
            Destroy(collision.gameObject);
            Debug.Log("Fruits collected: " + fruitCount);
        }
    }
}
