using UnityEngine;

public class StatueUpgrade : MonoBehaviour
{
    [Header("Upgrade Settings")]
    public int cost = 50; public int healthIncrease = 20; public int attackIncrease = 5;
    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryUpgrade();
        }
    }

    private void TryUpgrade()
    {
        if (CoinManager.instance == null) return;

        if (CoinManager.instance.GetTotalCoins() >= cost)
        {
            CoinManager.instance.AddCoin(-cost);

            PlayerDamageReceiver.instance.MaxHealth += healthIncrease;
            PlayerAttack.Instance.damage += attackIncrease;

            Debug.Log("Nâng c?p thành công! Máu + " + healthIncrease + ", Damage + " + attackIncrease);
        }
        else
        {
            Debug.Log("Không ð? ti?n ð? nâng c?p!");
        }
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
