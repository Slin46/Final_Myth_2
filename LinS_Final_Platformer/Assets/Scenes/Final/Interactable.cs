using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Camera cam;
    //list of all objects 
    public List<GameObject> selectedObjects = new List<GameObject>();
    //keep in track of the order of selectedobjects
    public List<GameObject> selectedObjectsInOrder = new List<GameObject>();

    //assign player transform here
    public Transform playerHoldPoint;
    //currently held item
    private GameObject heldItem = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //call selection
        HandleSelection();

        FollowHeldItem(playerHoldPoint);
    }

    public void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);

            //get all colliders at the mouse position
            Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorldPos);

            GameObject objToSelect = null;

            //find the top-most valid object (Interactible or Collectible)
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Interactible") || hit.CompareTag("Collectible"))
                {
                    objToSelect = hit.gameObject;
                    break; // take the first valid one
                }
            }

            if (objToSelect == null) return; // no valid object under cursor

            //select/deselect obj
            if (selectedObjects.Contains(objToSelect))
            {
                //deselect to remove from both lists
                selectedObjects.Remove(objToSelect);
                selectedObjectsInOrder.Remove(objToSelect);
                Highlight(objToSelect, false);
                Debug.Log("Deselected: " + objToSelect.name);
            }
            else
            {
                //select to add to both lists
                selectedObjects.Add(objToSelect);
                selectedObjectsInOrder.Add(objToSelect); //this keeps the click order
                Highlight(objToSelect, true);
                Debug.Log("Selected: " + objToSelect.name);
            }
        }
    }
    void Highlight(GameObject obj, bool state)
    {
        if (obj == null) return;

        //find object named highlight
        Transform highlight = obj.transform.Find("Highlight");

        if (highlight != null)
        {
            //set it active
            highlight.gameObject.SetActive(state);
        }
        else
        {
            Debug.LogWarning("Highlight child not found on " + obj.name);
        }
    }
    public void HandleInteraction()
    {
        //no selections, nothing happens
        if (selectedObjects.Count == 0) return;

           foreach (var obj in selectedObjects)
           {
               //for tags named interactible only
               if (obj.CompareTag("Interactible"))
               {
                   IInteractible interact = obj.GetComponent<IInteractible>();
                   if (interact != null)
                       interact.Interact();

                   Debug.Log("Interacted:" + obj.name);
               }
           }
            
        
        selectedObjects.RemoveAll(obj => obj == null);

    }
    public void HandlePickup(Transform playerHoldPoint)
    {
        // Drop currently held item if there is one
        if (heldItem != null)
        {
            Collectible heldCol = heldItem.GetComponent<Collectible>();
            if (heldCol != null)
                heldCol.Drop();

            Debug.Log("Dropped: " + heldItem.name);
            heldItem = null;
        }

        // Now pick up the first selected collectible (if any)
        if (selectedObjects.Count == 0) return;

        foreach (GameObject obj in selectedObjects)
        {
            if (obj.CompareTag("Collectible"))
            {
                Collectible col = obj.GetComponent<Collectible>();
                if (col != null)
                {
                    heldItem = obj; // store currently held item
                    col.PickUp(playerHoldPoint);
                    Debug.Log("Picked Up: " + heldItem.name);

                    // Deselect it
                    selectedObjects.Remove(obj);
                    Highlight(obj, false);
                    break; // only pick up one item at a time
                }
            }
        }
    }
    // Call this every frame to make held item follow the player
    public void FollowHeldItem(Transform playerHoldPoint)
    {
        if (heldItem != null && playerHoldPoint != null)
        {
            heldItem.transform.position = playerHoldPoint.position;
        }
    }



}
