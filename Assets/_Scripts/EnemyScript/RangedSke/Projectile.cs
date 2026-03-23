using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string poolTag = "Arrow"; public float lifeTime = 5f;
    public LayerMask playerLayer; public LayerMask groundLayer;
    public float damage;


    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    private void ReturnToPool()
    {
        CancelInvoke();
        ObjectPooler.Instance.ReturnToPool(poolTag, this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        int layer = collision.gameObject.layer;

        if ((playerLayer.value & (1 << layer)) > 0)
        {
            PlayerDamageReceiver.instance.TakeDamage(damage);
            Debug.Log("Projectile hit Player!");

            ReturnToPool();
            return;
        }

        if ((groundLayer.value & (1 << layer)) > 0)
        {
            Debug.Log("Projectile hit wall/ground.");
            ReturnToPool();
            return;
        }

    }

}