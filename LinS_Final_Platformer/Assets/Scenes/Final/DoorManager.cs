using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public Transform Spawn_A;
    public Transform Spawn_B;
    public Transform Spawn_C;
    public Transform Spawn_D;

    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");

        switch (SceneData.spawnPoint)
        {
            case "Spawn_A":
                player.transform.position = Spawn_A.position;
                break;
            case "Spawn_B":
                player.transform.position = Spawn_B.position;
                break;
            case "Spawn_C":
                player.transform.position = Spawn_C.position;
                break;
            case "Spawn_D":
                player.transform.position = Spawn_D.position;
                break;
            default:
                Debug.LogWarning("No spawn point set, using default position.");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
