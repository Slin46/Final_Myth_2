using UnityEngine;

public class Speaker : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    //red orange yellow green color in that order
    public Color red = Color.red;
    public Color orange = new Color(1f, 0.5f, 0f);
    public Color yellow = Color.yellow;
    public Color green = Color.green;

    //start at presscount -1
    private int pressCount = -1;
    //green is false
    public bool isGreen = false;

    private Color[] colors;

    private Interactable interactableScript;

    private bool hasBeenSelected = false; // Tracks if selected at least once

    private void Awake()
    {
        //grab sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Speaker requires a SpriteRenderer!");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize color array: red -> orange -> yellow -> green
        colors = new Color[] { red, orange, yellow, green };

        // Find the Interactable script in the scene
        interactableScript = FindFirstObjectByType<Interactable>();
        if (interactableScript == null)
            Debug.LogError("No Interactable script found in the scene!");
    }

    // Update is called once per frame
    void Update()
    {
        if (interactableScript == null) return;

        // Check if the speaker has been selected at least once
        bool isSelected = interactableScript.selectedObjects.Contains(gameObject);

        if (isSelected)
            hasBeenSelected = true;

        // Allow E presses if it has been selected at least once
        if ((isSelected || hasBeenSelected) && Input.GetKeyDown(KeyCode.E))
        {
            pressCount = Mathf.Min(pressCount + 1, colors.Length - 1);

            if (spriteRenderer != null)
                spriteRenderer.color = colors[pressCount];

            if (pressCount == colors.Length - 1)
            {
                isGreen = true;
                Debug.Log("Speaker is now green!");
            }
        }
    }
}
