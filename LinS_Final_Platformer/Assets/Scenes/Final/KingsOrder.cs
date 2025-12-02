using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KingsOrder : MonoBehaviour
{
    [Header("Black Panel")]
    //black panel for the kings order and hint text to lay on
    public GameObject blackPanel;

    [Header("King's Order Panel")]
    //empty parent object for the kings order
    public GameObject kingsOrderPanel;
    //"kings order" text
    public TMP_Text kingsOrderTitle;
    //15 text orders for each npc
    public TMP_Text[] nameOrders;
    //orders per room
    public int[] roomLimits = { 4, 4, 4, 3 };

    [Header("Hint Panel")]
    //empty parent object for the hint panel
    public GameObject hintPanel;
    //"hint" text
    public TMP_Text hintTitle;
    //hint string for rooms
    public TMP_Text[] roomHints;

    [Header("Game Data")]
    //store the current room number data
    //0=1, 1=2, etc.
    public int currentRoom = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //hide everything at start
        HideBlackPanel();
        HideKingsOrder();
        HideHint();

        RandomizeNamesPerRoom();
    }

    void RandomizeNamesPerRoom()
    {
        int startIndex = 0;

        for (int room = 0; room < roomLimits.Length; room++)
        {
            int count = roomLimits[room];

            //only consider the texts for this room
            List<int> indices = new List<int>();
            for (int i = 0; i < count; i++)
                indices.Add(startIndex + i);

            //shuffle indices
            for (int i = 0; i < indices.Count; i++)
            {
                int rand = Random.Range(i, indices.Count);
                int temp = indices[i];
                indices[i] = indices[rand];
                indices[rand] = temp;
            }

            //assign randomized texts to UI elements for this room
            TMP_Text[] tempTexts = new TMP_Text[count];
            for (int i = 0; i < count; i++)
            {
                tempTexts[i] = nameOrders[indices[i]];
            }

            for (int i = 0; i < count; i++)
            {
                nameOrders[startIndex + i].text = tempTexts[i].text;
                // hide texts that are not in this room
                nameOrders[startIndex + i].gameObject.SetActive(room == currentRoom);
            }

            // hide all other texts outside this room
            for (int i = 0; i < nameOrders.Length; i++)
            {
                if (i < startIndex || i >= startIndex + count)
                    nameOrders[i].gameObject.SetActive(false);
            }

            startIndex += count;
        }
    }
    //show the black panel
    public void ShowBlackPanel()
    {
        if (blackPanel != null) blackPanel.SetActive(true);
    }

    //hide the black panel
    public void HideBlackPanel()
    {
        if (blackPanel != null) blackPanel.SetActive(false);
    }
    public void ShowKingsOrder()
    {
        Debug.Log("ShowKingsOrder called");
        //show black panel, hide hint, show kings order panel
        ShowBlackPanel();
        Debug.Log("Black panel activated");
        HideHint();

        //show the KingsOrder panel
        if (kingsOrderPanel != null) kingsOrderPanel.SetActive(true);

        // Hide all orders first
        foreach (var text in nameOrders)
            text.gameObject.SetActive(false);

        // Determine start and end indices for current room
        int startIndex = 0;
        for (int r = 0; r < currentRoom; r++)
            startIndex += roomLimits[r];

        int endIndex = startIndex + roomLimits[currentRoom];

        // Pick one random index within this room's range
        int randomIndex = Random.Range(startIndex, endIndex);

        // Activate only that random text
        nameOrders[randomIndex].gameObject.SetActive(true);

        Debug.Log($"Room {currentRoom + 1} order shown: {nameOrders[randomIndex].text}");
    }

    //hide King's Order panel
    public void HideKingsOrder()
    {
        Debug.Log("HideKingsOrder called");
        if (kingsOrderPanel != null) kingsOrderPanel.SetActive(false);
    }

    //show hint for the current room
    public void ShowHint()
    {
        //hide kings order, show black panel, show hint if condition met
        HideKingsOrder();
        ShowBlackPanel();

        // Only show the TMP_Text for the current room
        for (int i = 0; i < roomHints.Length; i++)
        {
            roomHints[i].gameObject.SetActive(i == currentRoom);
        }

        //show the hint panel parent
        if (hintPanel != null) hintPanel.SetActive(true);
    }

    //hide hint panel
    public void HideHint()
    {
        if (hintPanel != null) hintPanel.SetActive(false);
    }

    public void GoToNextRoom()
    {
        currentRoom++;
        if (currentRoom >= roomLimits.Length)
        {
            Debug.Log("All rooms completed");
            //cap to last room
            currentRoom = roomLimits.Length - 1;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
