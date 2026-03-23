using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI healthBarValueText;

    void Start()
    {

    }

    public virtual void Update()
    {
        slider.value = PlayerDamageReceiver.instance.Health / PlayerDamageReceiver.instance.MaxHealth;
        healthBarValueText.text = PlayerDamageReceiver.instance.Health + "/" + PlayerDamageReceiver.instance.MaxHealth;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MenuGame")
        {
            Destroy(gameObject);
        }
    }
}
