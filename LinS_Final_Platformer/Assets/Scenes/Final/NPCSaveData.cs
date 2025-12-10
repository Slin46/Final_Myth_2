using System.Collections.Generic;
using UnityEngine;

public class NPCSaveData : MonoBehaviour
{
    public static NPCSaveData Instance;

    // Tracks dead NPCs in this playthrough (memory only)
    private Dictionary<int, bool> npcDeadStatus = new Dictionary<int, bool>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // survive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsNPCDead(int orderID)
    {
        return npcDeadStatus.ContainsKey(orderID) && npcDeadStatus[orderID];
    }

    public void KillNPC(int orderID)
    {
        npcDeadStatus[orderID] = true;
    }

}
