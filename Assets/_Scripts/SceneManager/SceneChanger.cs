using UnityEngine;
using UnityEngine.SceneManagement;
public static class SceneTransitionData
{
    public static string lastGateID; public static string previousScene; public static bool spawnFacingRight = true; public static float enterTime;
}

public class SceneChanger : MonoBehaviour
{
    [Header("Scene Settings")]
    public string targetSceneName = "Level_02";
    [Header("Gate ID")]
    public string thisGateID;
    [Header("Spawn Face Direction")]
    public bool spawnFacingRight = true;

    [Header("Player Tag")]
    public string playerTag = "Player"; bool isLoading = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isLoading) return;
        if (Time.time - SceneTransitionData.enterTime < 0.5f) return;
        if (other.CompareTag(playerTag))
        {

            Debug.Log("Ngý?i chõi ði vào c?ng! Chuy?n Scene...");

            LoadNewScene();
        }
    }

    void LoadNewScene()
    {
        try
        {
            isLoading = true;
            SceneTransitionData.enterTime = Time.time;

            SceneTransitionData.previousScene = SceneManager.GetActiveScene().name;
            SceneTransitionData.lastGateID = thisGateID;
            SceneTransitionData.spawnFacingRight = spawnFacingRight;

            SceneManager.LoadScene(targetSceneName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("L?i khi t?i Scene: " + targetSceneName);
            Debug.LogError("Vui l?ng ki?m tra xem tên Scene ð? chính xác và ð? ðý?c thêm vào Build Settings chýa.");
            Debug.LogError(e);
        }
    }
}