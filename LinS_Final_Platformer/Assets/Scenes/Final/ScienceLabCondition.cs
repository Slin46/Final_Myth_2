using UnityEngine;
using UnityEngine.InputSystem;

public class ScienceLabCondition : MonoBehaviour
{
    public static ScienceLabCondition Instance;

    [Header("Element 4 objects")]
    public GameObject sink;

    [Header("Element 5 objects")]
    public GameObject locker3;

    [Header("Element 6 objects")]
    public GameObject GreenBottle;

    [Header("Element 7 objects")]
    public GameObject window1;
    public GameObject window2;

    [Header("Door")]
    public Door scienceLabDoor;

    private Interactable interactableScript;
    private bool completed = false;

    // Map each element to its corresponding KingsOrder index
    public int[] scienceLabOrderIndices = { 4, 5, 6, 7 };

    private bool hasBeenHeld = false;
    private bool hasBeenDropped = false;


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


        Condition.Instance.roomDoor = scienceLabDoor;
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

        // Map directly: 
        switch (activeOrder)
        {
            case 4: // HaniOrder (E sink)
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Must match exactly the two required objects
                    if (Condition.Instance.SelectedExactly(sink))
                        Condition.Instance.OnElementCompleted(activeOrder);

                    Debug.Log("Checking HaniOrder objects.");
                }
                break;

            case 5: // ChanOrder (locker)
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (Condition.Instance.SelectedExactly(locker3))
                        Condition.Instance.OnElementCompleted(activeOrder);

                    Debug.Log("Checking ChanOrder objects.");
                }
                break;

            case 6: // BooOrder (green bottle)
                {
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        if (GreenBottle != null)
                        {
                            Collectible bottle = GreenBottle.GetComponent<Collectible>();

                            if (bottle == null)
                                break;

                            // FIRST Q object becomes held
                            if (bottle.isHeld && !hasBeenHeld)
                            {
                                hasBeenHeld = true;
                                Debug.Log("Bottle was picked up.");
                            }
                            // SECOND Q object is dropped
                            else if (!bottle.isHeld && hasBeenHeld && !hasBeenDropped)
                            {
                                hasBeenDropped = true;
                                Debug.Log("Bottle was dropped.");

                                // Now both states completed complete condition
                                Condition.Instance.OnElementCompleted(activeOrder);
                            }
                        }
                    }
                }
                break;

            case 7: // Noah (windows)
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (Condition.Instance.SelectedExactly(window1, window2))
                        Condition.Instance.OnElementCompleted(activeOrder);

                    Debug.Log("Checking NoahOrder objects.");
                }
                break;


        }

    }

           
}

