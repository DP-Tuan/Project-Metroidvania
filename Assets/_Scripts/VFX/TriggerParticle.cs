using UnityEngine;

public class TriggerParticle : MonoBehaviour
{
    public static TriggerParticle Instance { get; private set; }

    [Header("Particle Prefabs")]
    public GameObject explosionVFX;
    public GameObject hitVFX;
    public GameObject deathVFX;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Explosion(Vector3 pos)
    {
        if (explosionVFX == null) return;

        GameObject fx = Instantiate(explosionVFX, pos, Quaternion.identity);
        var ps = fx.GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();
    }

    public void SpawnDeath(Vector3 pos)
    {
        if (deathVFX != null)
            Instantiate(deathVFX, pos, Quaternion.identity);
    }

    public void Hit(Vector3 pos)
    {
        if (hitVFX == null) return;

        GameObject fx = Instantiate(hitVFX, pos, Quaternion.identity);
        var ps = fx.GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();
    }
}
