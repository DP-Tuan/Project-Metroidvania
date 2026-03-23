using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneFromIntro : MonoBehaviour
{
    [SerializeField] private float delay = 15f;
    [SerializeField] private float timer;

    void Start()
    {
        Time.timeScale = 1f;
        timer = delay;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
