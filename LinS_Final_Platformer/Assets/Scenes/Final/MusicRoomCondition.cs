using System.Linq;
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
    private bool completed = false;

    // Map each element to its corresponding KingsOrder index
    public int[] musicRoomOrderIndices = { 0, 1, 2, 3 };


    private void Awake()
    {
        Debug.Log("Awake called.");
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactableScript = FindFirstObjectByType<Interactable>();
        if (interactableScript == null) Debug.LogWarning("No Interactable found.");
        else
            Debug.Log("Interactable found successfully.");

      
        Condition.Instance.roomDoor = musicRoomDoor;
    }

    // Update is called once per frame
    void Update()
    {

        CheckCurrentElement();
    }

    private void CheckCurrentElement()
    {
        if (completed)
        {
            return;
        }

        // Read active order from PlayerPrefs
        int activeOrder = PlayerPrefs.GetInt("ActiveOrderIndex", -1);
        if (activeOrder < 0)
        {
            Debug.LogWarning("ActiveOrderIndex not set in PlayerPrefs!");
            return;
        }

        // Map directly: 0 => OliverOrder, 1 => WinnieOrder, 2 => JellyOrder, 3 => LayOrder
        switch (activeOrder)
        {
            case 0: // OliverOrder (bookshelf2 + shelf1)
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Must match exactly the two required objects
                    if (Condition.Instance.SelectedExactly(bookshelf2, shelf1))
                        Condition.Instance.OnElementCompleted(activeOrder);

                    Debug.Log("Checking OliverOrder objects.");
                }
                break;

            case 1: // WinnieOrder (rope + piano)
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (Condition.Instance.SelectedExactly(rope2, piano))
                        Condition.Instance.OnElementCompleted(activeOrder);

                    Debug.Log("Checking WinnieOrder objects.");
                }
                break;

            case 2: // JellyOrder (pick up key)
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    if (key != null)
                    {
                        Collectible keyCol = key.GetComponent<Collectible>();
                        if (keyCol != null && keyCol.isHeld)
                            Condition.Instance.OnElementCompleted(activeOrder);
                    }
                }
                break;

            case 3: // LayOrder (speaker green)
                Speaker speakerScript = speaker != null ? speaker.GetComponent<Speaker>() : null;
                if (speakerScript != null && speakerScript.isGreen)
                {
                    Debug.Log("Speaker is green - completing element");
                    Condition.Instance.OnElementCompleted(activeOrder);
                }
                break;
        }
    }

   
}
