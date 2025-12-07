using UnityEngine;

public class BookInteractible : MonoBehaviour
{
    public GameObject keyObject; // assign in Inspector
    public bool isCollected = false;

    public void Interact()
    {
        if (isCollected) return;

        // Destroy the book
        Destroy(gameObject);

        if (keyObject != null)
        {
            // Unparent so it behaves independently
            keyObject.transform.parent = null;

            // Activate key so it can be picked up
            keyObject.SetActive(true);

            Debug.Log("Key revealed!");
        }
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
