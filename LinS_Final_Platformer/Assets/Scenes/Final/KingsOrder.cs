using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KingsOrder : MonoBehaviour
{
    public static KingsOrder Instance;

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
    //the current order its on
    public int activeOrderIndex;

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

    public GameObject finalTestPanel;
    public GameObject continueArrow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Load current room from PlayerPrefs
        currentRoom = PlayerPrefs.GetInt("CurrentRoom", 0);

        // Ensure it’s within bounds
        if (currentRoom < 0)
            currentRoom = 0;

        // Hide everything at start
        HideBlackPanel();
        HideKingsOrder();
        HideHint();

        // Randomize only if we're not past last room
        if (currentRoom < roomLimits.Length)
            RandomizeCurrentRoomTexts();
    }

    void RandomizeCurrentRoomTexts()
    {
        if (currentRoom >= roomLimits.Length) return;

        int startIndex = 0;
        for (int r = 0; r < currentRoom; r++)
            startIndex += roomLimits[r];

        int count = roomLimits[currentRoom];

        // Shuffle only the texts for the current room
        List<int> indices = new List<int>();
        for (int i = 0; i < count; i++)
            indices.Add(startIndex + i);

        for (int i = 0; i < indices.Count; i++)
        {
            int rand = Random.Range(i, indices.Count);
            int temp = indices[i];
            indices[i] = indices[rand];
            indices[rand] = temp;
        }

        for (int i = 0; i < count; i++)
        {
            nameOrders[startIndex + i].text = nameOrders[indices[i]].text;
            nameOrders[startIndex + i].gameObject.SetActive(true);
        }

        // hide texts not in this room
        for (int i = 0; i < nameOrders.Length; i++)
        {
            if (i < startIndex || i >= startIndex + count)
                nameOrders[i].gameObject.SetActive(false);
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
        if (currentRoom >= roomLimits.Length)
        {
            // We're past the last room, just activate final test
            if (finalTestPanel != null)
                finalTestPanel.SetActive(true);

            HideKingsOrder();
            HideBlackPanel();
            return;
        }

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

    public void OnRoomEnd(bool conditionMet)
    {
        HideKingsOrder();

        // Show hint immediately if the condition was met
        if (conditionMet)
            ShowHint();          // show the hint first
            ShowKingsOrder();    // then show the King's Order

        // Then proceed to the next room
        GoToNextRoom();
    }

    public void GoToNextRoom()
    {
        currentRoom++;

        // If we exceed the last room, activate final test
        if (currentRoom >= roomLimits.Length)
        {
            Debug.Log("All rooms completed! Activating final test...");

            if (continueArrow != null)
            {
                continueArrow.SetActive(false);
            }

            if (finalTestPanel != null)
                finalTestPanel.SetActive(true);

            HideKingsOrder();
            HideBlackPanel();

            // Stop further room logic
            return;
        }

        // Otherwise, randomize texts for the new room
        RandomizeCurrentRoomTexts();

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
