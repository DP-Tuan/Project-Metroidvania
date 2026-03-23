using UnityEngine;

public class HoldGlobalVolume : MonoBehaviour
{
    private static HoldGlobalVolume instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
