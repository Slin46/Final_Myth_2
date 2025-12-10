using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject envelope;

    [Header("Precondition Objects")]
    public GameObject hanger;
    public GameObject clock;
    public GameObject mountain;

    [Header("Precondition Targets")]
    public Transform hangerTarget; // empty GameObject in scene where hanger should go
    public Transform clockTarget;  // top of hanger
    public Transform mountainTarget; // top of clock

    private int finalconditionStep = 0; // tracks current step in the sequence
    public bool finalconditionCompleted = false;

    public TMP_Text finalPhaseMessage;
    
    [Header("Door")]
    public Door TeachersOfficeDoor;

    private Interactable interactableScript;
    private bool completed = false;

    // Map each element to its corresponding KingsOrder index
    public int[] TeachersOfficeOrderIndices = { 12, 13, 14 };

    public bool coffee1PickedUp = false;
    public bool noStress = false;

    private bool activeOrderCompleted = false; // track if player did the element itself

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

        //set final phase message to false
        if (finalPhaseMessage != null)
            finalPhaseMessage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckCurrentElement();
        CheckFinalcondition();
        CheckFinalCompletion();
    }

    //do the current active orders first
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

                                CompleteActiveOrder(activeOrder);
                            }

                        }
                    }
                }
                break;

            case 13: // EaganOrder (umbrella)
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (Condition.Instance.SelectedExactly(umbrella))

                        CompleteActiveOrder(activeOrder);
                    Debug.Log("Checking EaganOrder objects.");
                }
                break;

            case 14: // Ivan (E books in rainbow order)
                {
                    List<GameObject> selectedOrder = interactableScript.selectedObjectsInOrder;
                    GameObject[] requiredOrder = new GameObject[] { redbook, greenbook, bluebook, purplebook };

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (selectedOrder.Count == requiredOrder.Length)
                        {
                            bool correctOrder = true;
                            for (int i = 0; i < requiredOrder.Length; i++)
                            {
                                if (selectedOrder[i] != requiredOrder[i])
                                {
                                    correctOrder = false;
                                    break;
                                }
                            }

                            if (correctOrder)
                            {
                                CompleteActiveOrder(activeOrder);
                             
                                // Activate envelope
                                if (envelope != null)
                                    envelope.SetActive(true);

                                Debug.Log("Ivan order completed!");
                            }
                            else
                            {
                                Debug.Log("Selected objects are not in the correct order.");
                            }
                        }
                        else
                        {
                            Debug.Log("Incorrect number of objects selected.");
                        }
                    }

                    else
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            // Make sure the envelope is selected
                            if (interactableScript.selectedObjects.Contains(envelope))
                            {
                                Debug.Log("Envelope interacted! Precondition can now be done.");
                                // Here you could start the final precondition steps
                                activeOrderCompleted = true; // or trigger precondition logic
                                interactableScript.selectedObjects.Clear(); // clear selection after interaction
                            }
                            else
                            {
                                Debug.Log("Select the envelope to interact with it.");
                            }
                        }
                    }

                    break;
                }

        }

    }
    //once active order is completed start coroutine to show final phase message
    private void CompleteActiveOrder(int activeOrder)
    {
        if (!activeOrderCompleted)
        {
            activeOrderCompleted = true;
            StartCoroutine(ShowFinalPhaseMessageCoroutine());
            Debug.Log($"Active element {activeOrder} done. Final phase unlocked!");
        }
    }

    private IEnumerator ShowFinalPhaseMessageCoroutine()
    {
        if (finalPhaseMessage != null)
        {
            finalPhaseMessage.gameObject.SetActive(true);
            finalPhaseMessage.text = "Final Phase Activated! Find the King!";
        }

        yield return new WaitForSeconds(3f); // show for 3 seconds

        if (finalPhaseMessage != null)
            finalPhaseMessage.gameObject.SetActive(false);
    }

    //now player has to do precondition
    private void CheckFinalcondition()
    {
        if (!activeOrderCompleted || finalconditionCompleted) return;

        float snapDistance = 0.5f; // how close the object has to be

        // Step 0: Hanger
        if (finalconditionStep == 0)
        {
            if (Vector3.Distance(hanger.transform.position, hangerTarget.position) < snapDistance)
            {
                hanger.transform.position = hangerTarget.position; // snap to exact
                finalconditionStep++;
                Debug.Log("Hanger placed correctly!");
            }
        }
        // Step 1: Clock on top of hanger
        else if (finalconditionStep == 1)
        {
            if (Vector3.Distance(clock.transform.position, clockTarget.position) < snapDistance)
            {
                clock.transform.position = clockTarget.position; // snap to exact
                finalconditionStep++;
                Debug.Log("Clock placed correctly!");
            }
        }
        // Step 2: Mountain on top of clock
        else if (finalconditionStep == 2)
        {
            if (Vector3.Distance(mountain.transform.position, mountainTarget.position) < snapDistance)
            {
                mountain.transform.position = mountainTarget.position; // snap to exact
                finalconditionStep++;
                finalconditionCompleted = true;
                Debug.Log("Mountain placed correctly! Precondition completed!");
            }
        }
    }
    //when bot conditions are met does it call onelement completed and the door is unlocked
    private void CheckFinalCompletion()
    {
        int activeOrder = PlayerPrefs.GetInt("ActiveOrderIndex", -1);
        if (completed) return;

        if (activeOrderCompleted && finalconditionCompleted)
        {
            completed = true;
            Condition.Instance.OnElementCompleted(activeOrder);
            Debug.Log($"Element {activeOrder} completed! Door unlocked.");
        }
    }
}




