using System.Collections.Generic;
using UnityEngine;

public class NPCDestroy : MonoBehaviour
{
    public GameObject redXPrefab; // the red X replacement
    [Header("The order ID this NPC belongs to (ex: 12, 13, 14)")]
    public int orderID;
    // match the NPC name to the active order

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If already dead in this playthrough, replace with X
        if (NPCSaveData.Instance.IsNPCDead(orderID))
        {
            ReplaceWithRedX();
        }
        // Otherwise, kill if story failed for this order
        else if (SceneData.storyType == "Fail" && orderID == SceneData.failedOrderID)
        {
            NPCSaveData.Instance.KillNPC(orderID);
            ReplaceWithRedX();
        }
    }

    void ReplaceWithRedX()
    {
        if (redXPrefab != null)
        {
            GameObject redX = Instantiate(redXPrefab, transform.position, transform.rotation);

            var sr = redX.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sortingOrder = 100; // in front
        }

        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
