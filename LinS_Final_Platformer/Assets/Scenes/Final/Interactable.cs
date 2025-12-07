using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Camera cam;
    //list of all objects 
    public List<GameObject> selectedObjects = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //call selection and interaction
        HandleSelection();
        HandleInteraction();
    }

    public void HandleSelection()
    {
        //when left clicking
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (!hit.collider) return;

            GameObject obj = hit.collider.gameObject;

            if (obj.CompareTag("Interactible") || obj.CompareTag("Collectible"))
            {
                if (selectedObjects.Contains(obj))
                {
                    //left click again to deselect
                    selectedObjects.Remove(obj);
                    Highlight(obj, false);
                    Debug.Log("Deselected: " + obj.name);

                }
                else
                {
                    //if its not selected and selectible then can select
                    selectedObjects.Add(obj);
                    //highlight for visual effects
                    Highlight(obj, true);
                    Debug.Log("Selected: " + obj.name);
                }
            }
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
                break;
               }
           }
            
        
        selectedObjects.RemoveAll(obj => obj == null);

    }
    public void HandlePickup()
    {
        if (selectedObjects.Count == 0) return;

        List<GameObject> pickedUp = new List<GameObject>();

        foreach (var obj in selectedObjects)
        {
            if (obj.CompareTag("Collectible"))
            {
                ICollectible collectible = obj.GetComponent<ICollectible>();
                if (collectible != null)
                {
                    collectible.PickUp();
                    pickedUp.Add(obj);
                    Debug.Log("Picked Up: " + obj.name);
                    break;
                }
            }
        }

        foreach (var obj in pickedUp)
        {
            selectedObjects.Remove(obj);
            Highlight(obj, false);
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


}
