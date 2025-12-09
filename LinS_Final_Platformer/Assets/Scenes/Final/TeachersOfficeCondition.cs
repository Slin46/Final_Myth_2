using UnityEngine;

public class TeachersOfficeCondition : MonoBehaviour
{
    public static TeachersOfficeCondition Instance;

    [Header("Element 12 objects")]
    public GameObject coffee1;
    public GameObject paper;
    public GameObject telephone3;

    [Header("Element 13 objects")]
    public GameObject umbrella;

    [Header("Element 14 objects")]
    public GameObject redbook;
    public GameObject greenbook;
    public GameObject bluebook;
    public GameObject purplebook;


    [Header("Door")]
    public Door TeachersOfficeDoor;

    private Interactable interactableScript;
    private bool completed = false;

    // Map each element to its corresponding KingsOrder index
    public int[] TeachersOfficeOrderIndices = { 12, 13, 14 };

    public bool coffee1PickedUp = false;
    public bool noStress = false;

    public string[] correctOrder = { "redbook", "greenbook", "bluebook", "purplebook" };

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


        Condition.Instance.roomDoor = TeachersOfficeDoor;
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
            case 12: // JasmineOrder (stress)
                {
                    // STEP 1 — Pickup with Q
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        Collectible col = coffee1.GetComponent<Collectible>();

                        if (col != null && col.isHeld)
                        {
                            coffee1PickedUp = true;
                            Debug.Log("coffee1 picked up!");
                        }
                    }

                    // STEP 2 — interact with paper and telephone
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Player must have already picked the knife…
                        if (coffee1PickedUp)
                        {
                            // Must match exactly the two required objects
                            if (Condition.Instance.SelectedExactly(paper, telephone3))
                            {
                                noStress = true;
                                Debug.Log("coffe no stress");

                                Condition.Instance.OnElementCompleted(activeOrder);
                            }

                        }
                    }
                }
                break;

            case 13: // EaganOrder (umbrella)
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (Condition.Instance.SelectedExactly(umbrella))
                        Condition.Instance.OnElementCompleted(activeOrder);

                    Debug.Log("Checking EaganOrder objects.");
                }
                break;

            case 14: // Ivan (E books in rainbow order)
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Must match exactly the 4 required objects
                    if (Condition.Instance.SelectedExactly(redbook, greenbook, bluebook, purplebook))
                        Condition.Instance.OnElementCompleted(activeOrder);

                    Debug.Log("Checking ivanorder objects.");
                }
                break;
        }
        }
    }
