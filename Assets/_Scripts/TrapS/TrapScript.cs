using UnityEngine;

public class TrapScript : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    public PlayerController player;
    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            player.GetComponentInChildren<PlayerDamageReceiver>().TakeDamage(8);
        }
    }
}
