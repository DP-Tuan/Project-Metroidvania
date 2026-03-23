using UnityEngine;

public class ZoneDeath : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerDamageReceiver.instance.PlayerDie();
        }
    }
}
