using System.Collections;
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

    // Map each element to its corresponding KingsOrder index
    public int[] musicRoomOrderIndices = { 0, 1, 2, 3 };


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

        // Load the saved index
        currentElement = 0;
        int savedIndex = PlayerPrefs.GetInt("ActiveOrderIndex", -1);

        if (savedIndex == -1)
            Debug.LogError("No active order index found!");

        Debug.Log("Loaded Active Order Index: " + savedIndex);

        // NOW use this number to decide which element to check
        UseActiveOrder(savedIndex);
    }
    private int activeOrderIndex;

    void UseActiveOrder(int index)
    {
        activeOrderIndex = index;
    }

    // Update is called once per frame
    void Update()
    {

        CheckCurrentElement();
    }

    private void CheckCurrentElement()
    {
        int activeOrder = activeOrderIndex;
        Debug.Log($"Active order index: {activeOrder}, Current Element: {currentElement}");

        switch (activeOrder)
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
                    Debug.Log("Selected objects: " + string.Join(", ", interactableScript.selectedObjects));
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
                    Debug.Log($"Speaker isGreen: {speakerScript.isGreen}");
                    if (speakerScript.isGreen)
                    {
                        Debug.Log("Speaker is green! Calling ElementCompleted");
                        ElementCompleted();
                    }
                }
                break;
        }
    }

    private void ElementCompleted()
    {
        Debug.Log("Music Room Element " + currentElement + " completed.");

        // Tell RoundManage that the condition is met
        if (RoundManage.Instance != null)
        {
            Debug.Log("Calling PlayerMetCondition");
            RoundManage.Instance.PlayerMetCondition();
        }
        else
        {
            Debug.LogWarning("RoundManage.Instance is null!");
        }

        // Optional debug message for the door
        if (musicRoomDoor != null)
            Debug.Log("Door is now unlocked!");

        currentElement++;

        // Stop key from following player after element 2
        if (currentElement > 2 && key != null)
        {
            Collectible keyCol = key.GetComponent<Collectible>();
            if (keyCol != null)
                keyCol.isHeld = false;
        }
    }

    //storytype success when player collides with door 
    public void PlayerCollidedWithDoor()
    {

        if (currentElement > 0 && RoundManage.Instance != null)
        {
            RoundManage.Instance.PlayerMetCondition();
        }
    }
}
