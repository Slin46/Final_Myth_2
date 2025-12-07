using UnityEngine;

public class Collectible : MonoBehaviour
{
    public Transform player;           // assign your player in Inspector
    public bool isCollected = false;
    public Vector3 offset = new Vector3(0.5f, 0.5f, 0f); // adjust how item follows

    // Keep track of the currently held item (static so only one at a time)
    private static Collectible currentHeldItem = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCollected && player != null)
        {
            // Smoothly follow player
            transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * 10f);
        }
    }
    public void PickUp(Transform playerTransform)
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;

        // Drop previous item if there is one
        if (currentHeldItem != null && currentHeldItem != this)
        {
            currentHeldItem.Drop();
        }

        isCollected = true;
        currentHeldItem = this;

        // Disable collider and Rigidbody for smooth follow
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Debug.Log(gameObject.name + " picked up!");

    }
    public void Drop()
    {
        if (!isCollected) return;

        isCollected = false;
        currentHeldItem = null;

        // Re-enable physics/collider so it stays at current position
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;

        Debug.Log(gameObject.name + " dropped!");
    }
}
