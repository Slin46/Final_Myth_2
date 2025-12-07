using System.Collections.Generic;
using UnityEngine;

public class MusicRoomCondition : MonoBehaviour
{
    public static MusicRoomCondition Instance;

    [Header("Element 0 objects")]
    public GameObject bookshelf2;
    public GameObject shelf1;

    [Header("Element 1 objects")]
    public GameObject rope2;
    public GameObject piano;

    [Header("Element 2 objects - key pickup")]
    public GameObject books;
    public GameObject key;

    [Header("Element 3 objects - speaker")]
    public GameObject speaker;

    [Header("Door")]
    public Door musicRoomDoor;

    private Interactable interactableScript;
    private int currentElement = 0;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactableScript = FindFirstObjectByType<Interactable>();
        if (interactableScript == null)
            Debug.LogError("No Interactable script found in the scene!");
    }

    // Update is called once per frame
    void Update()
    {
        //call
        CheckCurrentElement();
    }

    private void CheckCurrentElement()
    {
        if (RoundManage.Instance == null || KingsOrder.Instance == null) return;

        int currentOrderIndex = KingsOrder.Instance.activeOrderIndex;

        // Only check element if it matches the active order index
        if (currentElement != currentOrderIndex) return;

        switch (currentElement)
        {
            case 0:
                // Element 0: select bookshelf2 + shelf1 and press E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (interactableScript.selectedObjects.Contains(bookshelf2) &&
                        interactableScript.selectedObjects.Contains(shelf1))
                    {
                        ElementCompleted();
                    }
                }
                break;

            case 1:
                // Element 1: select rope + piano and press E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (interactableScript.selectedObjects.Contains(rope2) &&
                        interactableScript.selectedObjects.Contains(piano))
                    {
                        ElementCompleted();
                    }
                }
                break;

            case 2:
                // Element 2: pick up key after interacting with book
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Collectible keyCol = key.GetComponent<Collectible>();
                    if (keyCol != null && keyCol.isHeld)
                    {
                        ElementCompleted();
                    }
                }
                break;

            case 3:
                Speaker speakerScript = speaker.GetComponent<Speaker>();
                if (speakerScript != null && speakerScript.isGreen)
                {
                    Debug.Log("Speaker is green! Music Room condition is now met.");
                    ElementCompleted();
                }
                break;
        }
    }

    private void ElementCompleted()
    {
        Debug.Log("Music Room Element " + currentElement + " completed.");

        // Tell RoundManage that condition is met (shows door unlocked message)
        if (RoundManage.Instance != null)
            RoundManage.Instance.PlayerMetCondition();

        // Optional: show a debug message on the door
        if (musicRoomDoor != null)
            Debug.Log("Door is now unlocked!");

        // Reference the current active order in KingsOrder
        if (KingsOrder.Instance != null)
        {
            int currentOrderIndex = KingsOrder.Instance.activeOrderIndex;
            Debug.Log($"Music Room element {currentElement} corresponds to KingsOrder index {currentOrderIndex}");
            // You can use currentOrderIndex if needed for hints or logic
        }

        currentElement++;

        // Ensure the key does not follow player to next scene
        if (currentElement == 3 && key != null)
        {
            Collectible keyCol = key.GetComponent<Collectible>();
            if (keyCol != null)
                keyCol.isHeld = false;
        }
    }

    //storytype success when player collides with door 
    public void PlayerCollidedWithDoor()
    {
        if (currentElement > 0)
        {
            if (RoundManage.Instance != null)
                RoundManage.Instance.PlayerMetCondition();
        }
    }
}
