using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string spawnPointName; // where player will appear after story
    public string storyType;      // which dialogue to show
    public string storySceneName = "StoryScene";
    
    public bool conditionMet = false;

    public enum DoorType { Spawn, Transition }
    public DoorType doorType = DoorType.Transition; // default to Transition


    void Start()
    {
     
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Only Transition doors trigger a scene load
        if (doorType == DoorType.Transition)
        {
            // Check if condition is met from RoundManage or test mode
            bool canOpen = (RoundManage.Instance != null && RoundManage.Instance.conditionMet);

            if (canOpen)
            {
                Debug.Log("Door unlocked! Player can proceed to StoryScene.");

                // Mark the condition met in RoundManage (for message display)
                if (RoundManage.Instance != null)
                    RoundManage.Instance.PlayerMetCondition();

                // Increment current room in PlayerPrefs for this play session
                int nextRoom = PlayerPrefs.GetInt("CurrentRoom", 0) + 1;
                PlayerPrefs.SetInt("CurrentRoom", nextRoom);
                PlayerPrefs.Save();

                // Pass spawn info and story type
                SceneData.spawnPoint = spawnPointName;
                SceneData.storyType = "Success";

                // Load story scene
                SceneManager.LoadScene(storySceneName);
            }
            else
            {
                Debug.Log("Door is locked. Condition not met yet.");
            }
        }
        else
        {
            Debug.Log("This door does not transition (spawn door).");
        }

    }
}
