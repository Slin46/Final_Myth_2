using System.Linq;
using UnityEngine;

public class Condition : MonoBehaviour
{
    public static Condition Instance;

    public Door roomDoor;                 // Door to unlock when conditions are complete
    public Interactable interactableScript;

    public bool completed = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // just in case you accidentally have multiple instances
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (interactableScript == null)
            interactableScript = FindFirstObjectByType<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    // Returns true if only the required objects are selected (no extras)
   public bool SelectedExactly(params GameObject[] requiredObjects)
    {
        if (interactableScript == null || interactableScript.selectedObjects == null)
            return false;

        var selected = interactableScript.selectedObjects;

        // Check that counts match and all required objects are present
        return selected.Count == requiredObjects.Length &&
               requiredObjects.All(o => selected.Contains(o));
    }

    public void OnElementCompleted(int activeOrder)
    {
        Debug.Log($"MusicRoomCondition: activeOrder {activeOrder} satisfied.");
        if (RoundManage.Instance != null)
        {
            RoundManage.Instance.PlayerMetCondition();
        }
        else Debug.LogWarning("RoundManage.Instance null when completing element.");

        // Clear selected objects so the player can continue interacting
        if (interactableScript != null && interactableScript.selectedObjects != null)
            interactableScript.selectedObjects.Clear();

        // Persist that the active order has been completed (optional)
        PlayerPrefs.SetInt("ActiveOrderCompleted", 1);
        PlayerPrefs.Save();

        // inform door or other things if needed
        if (roomDoor!= null) Debug.Log("MusicRoomCondition: door should now be unlocked.");

        completed = true;
    }

    //storytype success when player collides with door 
    public void PlayerCollidedWithDoor()
    {
        if (RoundManage.Instance != null) RoundManage.Instance.PlayerMetCondition();
    }
}
