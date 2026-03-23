using UnityEngine;
using UnityEngine.Playables;

public class DestroyGoScene : MonoBehaviour
{
    public PlayableDirector director;

    void Start()
    {
        if (director != null)
            director.timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;

        Time.timeScale = 0;

        if (director != null)
            director.stopped += OnTimelineEnd;
    }

    void OnTimelineEnd(PlayableDirector pd)
    {
        Debug.Log("Timeline ended!");
        Time.timeScale = 1f; Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (director != null)
            director.stopped -= OnTimelineEnd;
    }
}






