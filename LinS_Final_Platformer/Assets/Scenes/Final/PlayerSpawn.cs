using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!string.IsNullOrEmpty(SceneData.spawnPoint))
        {
            //spawn at the spawnpoint assigned in the inspector
            GameObject spawn = GameObject.Find(SceneData.spawnPoint);
            if (spawn != null)
            {
                transform.position = spawn.transform.position;
            }
            else
            {
                Debug.LogWarning("Spawn point not found: " + SceneData.spawnPoint);
            }
            //reset after spawning
            SceneData.spawnPoint = null;
        }
    }

}
