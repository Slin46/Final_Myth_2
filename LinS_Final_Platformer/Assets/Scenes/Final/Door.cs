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

        if (doorType == DoorType.Transition)
        {
            // Instead of checking RoundManage.conditionMet, check doorType's own state
            if (RoundManage.Instance != null)
            {
                // Ensure PlayerMetCondition() has already been called
                if (!RoundManage.Instance.conditionMet)
                {
                    Debug.Log("Door is locked. Condition not met yet.");
                    return;
                }
            }

            Debug.Log("Door unlocked! Player can proceed to StoryScene.");

            // Increment current room in PlayerPrefs
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
            Debug.Log("This door does not transition (spawn door).");
        }

    }
}
