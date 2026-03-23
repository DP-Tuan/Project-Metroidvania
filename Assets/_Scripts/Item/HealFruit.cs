using UnityEngine;

public class HealFruit : MonoBehaviour
{
    public int healValue = 25;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Heal();
        }
    }

    void Heal()
    {
        PlayerDamageReceiver.instance.Health += 25;
        AudioManager.instance.PlaySFX(AudioManager.instance.eatHP);
        Destroy(gameObject);
    }
}
