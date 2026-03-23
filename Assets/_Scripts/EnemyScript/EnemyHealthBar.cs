using UnityEngine.UI;

public class EnemyHealthBar : PlayerHealth
{
    public EnemyController controller;
    public Image fillImage;
    void Start()
    {
        this.controller = GetComponentInParent<EnemyController>();
    }

    public override void Update()
    {
        slider.value = controller.currentHealth / controller.maxHealth;
        if (slider.value <= 0.001f)
        {
            fillImage.enabled = false;
        }
        else
        {
            fillImage.enabled = true;
        }
    }

}
