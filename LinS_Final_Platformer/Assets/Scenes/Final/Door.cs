using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string spawnPointName; // where player will appear after story
    public string storyType;      // which dialogue to show
    public string storySceneName = "StoryScene";
    
    public bool testMode = true;
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
        if (doorType == DoorType.Transition && (conditionMet || testMode))
        {
            Debug.Log("Player collided with transition door! Moving to story scene...");

            SceneData.spawnPoint = spawnPointName;
            SceneData.storyType = storyType;

            SceneManager.LoadScene(storySceneName);
        }
        else
        {
            Debug.Log("This door does not transition (spawn door or condition not met).");
        }

    }
}
