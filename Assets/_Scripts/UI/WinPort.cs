using UnityEngine;

public class WinPort : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && BossAI.Instance.gameObject == null)
        {
            UIManager.Instance.Win();
        }
    }
}
