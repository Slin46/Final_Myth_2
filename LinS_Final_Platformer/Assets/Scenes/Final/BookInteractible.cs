using UnityEngine;

public class BookInteractible : MonoBehaviour, IInteractible
{
    public GameObject key; // assign the key in inspector

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

        // Deactivate the book instead of destroying it
        gameObject.SetActive(false);
        Debug.Log("book deactivated");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
