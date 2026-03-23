using UnityEngine;

public class TrampolineFunc : MonoBehaviour
{
    [SerializeField] private float bounce = 20f;
    [SerializeField] private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            animator.SetTrigger("push");


            player.PlayerMovementHandler.ApplyBounce(bounce);
        }
    }
}
