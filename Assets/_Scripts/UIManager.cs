using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject winUI;
    public GameObject loseUI;
    public GameObject pauseUI;
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
        winUI = GameObject.Find("Win");
        loseUI = GameObject.Find("GameOver");
        pauseUI = GameObject.Find("Paused");
        winUI.SetActive(false);
        loseUI.SetActive(false);
        pauseUI.SetActive(false);
    }
    public void Win()
    {
        winUI.SetActive(true);
    }

    public void Lose()
    {
        loseUI.SetActive(true);
    }
    public void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        loseUI.SetActive(false);
    }
    public void Continue()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void Menu()
    {
        SceneManager.LoadScene("MenuGame");
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }

}
