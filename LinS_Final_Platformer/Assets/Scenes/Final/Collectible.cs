using UnityEngine;

public class Collectible : MonoBehaviour
{
    public bool isHeld = false;
    private Transform followTarget;
    public float followSpeed = 8f;

    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        // Get Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();

        // Get Collider2D
        col = GetComponent<Collider2D>();
        if (col == null)
            col = gameObject.AddComponent<BoxCollider2D>();
    }
    void Update()
    {
        if (isHeld && followTarget != null)
        {
            transform.position = Vector2.Lerp(
                transform.position,
                followTarget.position,
                followSpeed * Time.deltaTime
            );
        }
    }

    public void PickUp(Transform playerAttachPoint)
    {
        if (playerAttachPoint == null) return;

        // Always unparent to avoid hierarchy issues
        transform.parent = null;

        followTarget = playerAttachPoint;
        isHeld = true;

        // Disable physics
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Disable collisions while held
        col.enabled = false;

        Debug.Log("Picked up " + gameObject.name);
    }

    public void Drop()
    {
        isHeld = false;
        followTarget = null;

        // Re-enable physics
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Re-enable collider
        col.enabled = true;

        Debug.Log("Dropped " + gameObject.name);
    }
}
