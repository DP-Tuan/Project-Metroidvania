using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI healthBarValueText;
    public BossHealth bossHealth;
    public GameObject healthBarCanvas;
    void Awake()
    {
        this.bossHealth = FindAnyObjectByType<BossHealth>();
        this.healthBarCanvas.SetActive(false);
    }

    public virtual void Update()
    {
        slider.value = bossHealth.CurrentHealth / bossHealth.maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            healthBarCanvas.SetActive(true);

            BossAI bossAI = FindAnyObjectByType<BossAI>();
            bossAI.ActivateBoss();
        }
    }
}
