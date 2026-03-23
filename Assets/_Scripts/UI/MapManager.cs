using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    public GameObject mapGame;

    private void Awake()
    {
        instance = this;
        mapGame = GameObject.Find("MapGame");
        mapGame.SetActive(false);
    }

    private void Start()
    {

    }

    public void OpenMap()
    {
        mapGame.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseMap()
    {
        mapGame.SetActive(false);
        Time.timeScale = 1;
    }
}
