using System.Collections.Generic;
using UnityEngine;

public class MusicRoomCondition : MonoBehaviour
{
    public bool conditionMet = false;

    private Interactable interactableScript;

    [Header("Element Requirements")]
    public string[] element0Objects = { "bookshelf2", "shelf1" }; // element 0 
    public string[] element1Objects = { "rope2", "piano" };         // element 1


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
        if (interactableScript == null || conditionMet) return;
        if (KingsOrder.Instance == null) return;

        int currentElement = KingsOrder.Instance.activeOrderIndex;

        string[] requiredObjects = null;

        switch (currentElement)
        {
            case 0:
                requiredObjects = element0Objects;
                break;
            case 1:
                requiredObjects = element1Objects;
                break;
            default:
                return; // no condition for other elements yet
        }

        // Call the Interactable script's interaction handler
        interactableScript.HandleInteraction();

        // Check if all required objects are selected and interacted with (E pressed)
        List<GameObject> selectedObjects = interactableScript.selectedObjects;
        bool allInteracted = true;

        foreach (string objName in requiredObjects)
        {
            //if the objects are required and selected and interacted
            bool found = false;
            foreach (GameObject obj in selectedObjects)
            {
                if (obj.name == objName && obj.CompareTag("Interactible"))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                allInteracted = false;
                break;
            }
        }

        // Condition is met if all required objects are selected and interacted with
        if (!conditionMet && allInteracted)
        {
            conditionMet = true;
            Debug.Log($"Element {currentElement} condition met!");

            // Tell RoundManage
            if (RoundManage.Instance != null)
                RoundManage.Instance.PlayerMetCondition();

            // Set story type for success
            SceneData.storyType = "Success";
        }
    }
}
