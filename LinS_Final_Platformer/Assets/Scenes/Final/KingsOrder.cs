using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        // Load current room from PlayerPrefs
        currentRoom = PlayerPrefs.GetInt("CurrentRoom", 0);

        // Ensure it’s within bounds
        if (currentRoom < 0)
            currentRoom = 0;

        // Hide everything at start
        HideBlackPanel();
        HideKingsOrder();
        HideHint();
        Debug.Log("hint hidden at start");

        // Randomize only if we're not past last room
        if (currentRoom < roomLimits.Length)
            RandomizeCurrentRoomTexts();
    }

    void RandomizeCurrentRoomTexts()
    {
        int startIndex = 0;
        for (int r = 0; r < currentRoom; r++)
            startIndex += roomLimits[r];

        int count = roomLimits[currentRoom];

        // Pick one active order index randomly for this room
        activeOrderIndex = startIndex + Random.Range(0, count);

        // Save it for the next scene
        PlayerPrefs.SetInt("ActiveOrderIndex", activeOrderIndex);
        PlayerPrefs.Save();

        Debug.Log($"KingsOrder randomized. Room {currentRoom} activeOrderIndex = {activeOrderIndex}");

        // Show only the selected text
        for (int i = 0; i < count; i++)
        {
            nameOrders[startIndex + i].gameObject.SetActive(i == (activeOrderIndex - startIndex));
        }
        // Hide all order texts initially
        foreach (var text in nameOrders)
            text.gameObject.SetActive(false);

        // Show only the active order text when ready
        nameOrders[activeOrderIndex].gameObject.SetActive(false); // initially hide
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

        //show the KingsOrder panel
        if (kingsOrderPanel != null) kingsOrderPanel.SetActive(true);

        foreach (var text in nameOrders)
            text.gameObject.SetActive(false);

        /// Show only the active order text
        nameOrders[activeOrderIndex].gameObject.SetActive(true);
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
        HideKingsOrder ();

        Debug.Log("show hint");
        ShowBlackPanel();

        if (currentRoom >= 0)
        {
            for (int i = 0; i < roomHints.Length; i++)
            {
                if (roomHints[i] != null)
                    roomHints[i].gameObject.SetActive(i == currentRoom);
            }
        }

        //show the hint panel parent
        if (hintPanel != null) hintPanel.SetActive(true);
        Debug.Log("hint panel set active");
    }

    //hide hint panel
    public void HideHint()
    {
        if (hintPanel != null) hintPanel.SetActive(false);
        Debug.Log("HideHint called");
    }
    // Called when the player clicks the continue arrow after seeing the hint
    public void OnContinueArrowClicked()
    {
        HideHint();
        if (continueArrow != null)
            continueArrow.SetActive(false);

        ShowKingsOrder();
    }

    public void OnRoomEnd(bool conditionMet)
    {
        Debug.Log("OnRoomEnd called");
        HideKingsOrder();

        if (conditionMet)
        {
            // Show hint panel and wait for continue arrow
            ShowHint();
            if (continueArrow != null)
                continueArrow.SetActive(true);
            Debug.Log("Hint shown");
        }
        else
        {
            // If condition not met, just show KingsOrder again
            ShowKingsOrder();
        }
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
