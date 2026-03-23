using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    void CollectCoin()
    {
        if (CoinManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.takeCoin);
            CoinManager.instance.AddCoin(coinValue);
        }


        Destroy(gameObject);
    }
}
