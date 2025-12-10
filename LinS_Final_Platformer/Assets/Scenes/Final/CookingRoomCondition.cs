using UnityEditor.PackageManager.UI;
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
    public GameObject reddrink;
    public GameObject redtrash;

    [Header("Element 10 objects")]
    public GameObject key2;
    public GameObject lock1;

    [Header("Element 11 objects")]
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

    public bool redpotPickedUp = false;
    public bool redpotThrown = false;
    public bool reddrinkPickedUp = false;
    public bool reddrinkThrown = false;

    public bool key2PickedUp = false;
    public bool key2Unlock = false;

    public bool potPickedUp = false;
    public bool potThrown = false;



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
                            if (Condition.Instance.SelectedExactly(trash))
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

            case 9: //FanOrder (poision)
                {
                    // STEP 1 — Pickup with Q
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        Collectible col = redpot.GetComponent<Collectible>();

                        if (col != null && col.isHeld)
                        {
                            redpotPickedUp = true;
                            Debug.Log("redpot picked up!");
                        }

                    }
                    // Reddink pickup (can only pick once)
                    else if (!reddrinkPickedUp)
                    {
                        Collectible drinkCol = reddrink.GetComponent<Collectible>();
                        if (drinkCol != null && drinkCol.isHeld)
                        {
                            reddrinkPickedUp = true;
                            Debug.Log("reddrink picked up!");
                        }
                    }

                    // STEP 2 — Throw into trash with E
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Redpot throw (only if picked up)
                        if (redpotPickedUp && !redpotThrown && Condition.Instance.SelectedExactly(redtrash))
                        {
                            redpotThrown = true;
                            Debug.Log("redpot thrown into redtrash!");
                        }

                        // Reddrink throw (only if picked up)
                        if (reddrinkPickedUp && !reddrinkThrown && Condition.Instance.SelectedExactly(redtrash))
                        {
                            reddrinkThrown = true;
                            Debug.Log("reddrink thrown into redtrash!");

                            // Final completion trigger
                            if (redpotThrown) // ensure redpot sequence completed first
                            {
                                Condition.Instance.OnElementCompleted(activeOrder);
                            }
                        }
                    }
                }
                break;

            case 10: //MiliOrder (suffocate from smoke)
                {
                    // STEP 1 — Pickup with Q
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        Collectible col = key2.GetComponent<Collectible>();

                        if (col != null && col.isHeld)
                        {
                            key2PickedUp = true;
                            Debug.Log("key2 picked up!");
                        }
                    }

                    // STEP 2 — unlock lock with E
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Player must have already picked the key2…
                        if (key2PickedUp)
                        {
                            // …and must currently be targeting or selecting the lock
                            if (Condition.Instance.SelectedExactly(lock1))
                            {
                                key2Unlock = true;
                                Debug.Log("key2 unlock lock1!");

                                // Final completion trigger
                                Condition.Instance.OnElementCompleted(activeOrder);
                            }
                        }
                    }
                }
                break;

            case 11: // Rifa (severe heat)
                {
                    // STEP 1 — Pickup with Q
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        Collectible col = pot.GetComponent<Collectible>();

                        if (col != null && col.isHeld)
                        {
                            potPickedUp = true;
                            Debug.Log("pot picked up!");
                        }
                    }

                    // STEP 2 — Throw into trash with E
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Player must have already picked the pot…
                        if (potPickedUp)
                        {
                            // …and must currently be targeting or selecting the trash
                            if (Condition.Instance.SelectedExactly(trash2))
                            {
                                potThrown = true;
                                Debug.Log("pot thrown into trash2!");

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
