using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    [Header("Coin UI")]
    public TextMeshProUGUI coinText_TMP;
    private int totalCoins = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        UpdateCoinText();
    }

    public void AddCoin(int amount)
    {
        totalCoins += amount;
        UpdateCoinText();
    }

    void UpdateCoinText()
    {
        if (coinText_TMP != null)
        {
            coinText_TMP.text = totalCoins.ToString();
        }
    }

    public int GetTotalCoins()
    {
        return totalCoins;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MenuGame")
        {
            instance = null;
            Destroy(gameObject);
        }
    }
}
