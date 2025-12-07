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
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
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
        followTarget = playerAttachPoint;
        isHeld = true;
        Debug.Log("Picked up " + gameObject.name);

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;  // disables physics while held
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (col != null)
            col.enabled = false; // disable collisions while held

    }

    public void Drop()
    {
        followTarget = null;
        isHeld = false;

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // re-enable physics
            rb.linearVelocity = Vector2.zero; // stop any leftover motion
            rb.angularVelocity = 0f;
        }

        if (col != null)
            col.enabled = true; // enable collisions again
    }
}
