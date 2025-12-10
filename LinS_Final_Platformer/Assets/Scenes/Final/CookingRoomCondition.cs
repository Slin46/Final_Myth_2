using UnityEngine;
using UnityEngine.InputSystem;

public class CookingRoomCondition : MonoBehaviour
{
    public static CookingRoomCondition Instance;

    [Header("Element 8 objects")]
    public GameObject knife;
    public GameObject trash;

    [Header("Element 9 objects")]
    public GameObject redpot;
    public GameObject redtrash;

    [Header("Element 10 objects")]
    public GameObject key2;
    public GameObject lock1;

    [Header("Element 3 objects - speaker")]
    public GameObject pot;
    public GameObject trash2;

    [Header("Door")]
    public Door CookingRoomDoor;

    private Interactable interactableScript;
    private bool completed = false;

    // Map each element to its corresponding KingsOrder index
    public int[] CookingRoomOrderIndices = { 8, 9, 10, 11 };

    public bool knifePickedUp = false;
    public bool knifeThrown = false;


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


        Condition.Instance.roomDoor = CookingRoomDoor;
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
            case 8: // CielOrder (knife  trash)
                {
                    // STEP 1 — Pickup with Q
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        Collectible col = knife.GetComponent<Collectible>();

                        if (col != null && col.isHeld)
                        {
                            knifePickedUp = true;
                            Debug.Log("Knife picked up!");
                        }
                    }

                    // STEP 2 — Throw into trash with E
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Player must have already picked the knife…
                        if (knifePickedUp)
                        {
                            // …and must currently be targeting or selecting the trash
                            if (interactableScript.selectedObjects.Contains(trash))
                            {
                                knifeThrown = true;
                                Debug.Log("Knife thrown into trash!");

                                // Final completion trigger
                                Condition.Instance.OnElementCompleted(activeOrder);
                            }
                        }
                    }
                }
                break;
        }
    }
            
    
}
