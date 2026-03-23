using UnityEngine;

public class DropCoins : MonoBehaviour
{
    public GameObject coinPrefab;
    public int coinAmount = 1;

    private bool playerInRange = false;
    private bool rewarded = true;
    public Animator animator;
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && rewarded)
        {
            DropCoin();
        }
    }

    public void DropCoin()
    {
        if (coinPrefab == null) return;

        animator.SetTrigger("openChest");
        for (int i = 0; i < coinAmount; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);

            Rigidbody2D rbCoin = coin.GetComponent<Rigidbody2D>();
            if (rbCoin != null)
            {
                float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

                float speed = Random.Range(5f, 15f);

                Vector2 velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;

                rbCoin.linearVelocity = velocity;

                rbCoin.gravityScale = 1f;
            }
        }
        rewarded = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
