using System.Collections;
using UnityEngine;

public class BookInteractible : MonoBehaviour, IInteractible
{
    public GameObject key; // assign the key in inspector
    public float deactivateDuration = 0.5f;

    private Collider2D col;
    private SpriteRenderer sr;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    public void Interact()
    {
        if (key != null)
        {
            // Unparent the key first
            key.transform.parent = null;

            // Enable key if it was inactive
            key.SetActive(true);

            // Enable physics
            Rigidbody2D rb = key.GetComponent<Rigidbody2D>();

            Debug.Log("key revealed");
        }
        
        // Temporarily deactivate the book
        StartCoroutine(TemporarilyDeactivate());
    }
    private IEnumerator TemporarilyDeactivate()
    {
        // Disable interaction and visibility
        if (col != null) col.enabled = false;
        if (sr != null) sr.enabled = false;

        yield return new WaitForSeconds(deactivateDuration);

        // Re-enable interaction and visibility
        if (col != null) col.enabled = true;
        if (sr != null) sr.enabled = true;

        Debug.Log("Book reactivated after delay");
    }
}
