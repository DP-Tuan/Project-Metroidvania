using System.Linq;
using UnityEngine;

public class SceneEntranceManager : MonoBehaviour
{
    void Start()
    {
        var spawnPoints = FindObjectsByType<SceneSpawnPoint>(FindObjectsSortMode.None);

        var targetSpawn = spawnPoints.FirstOrDefault(s => s.gateID == SceneTransitionData.lastGateID);

        if (targetSpawn != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = targetSpawn.transform.position;

            var sr = player.GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
                sr.flipX = !SceneTransitionData.spawnFacingRight;
        }
    }
}
