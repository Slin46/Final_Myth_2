using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string storySceneName = "StoryScene";
    //spawn after story ends
    public string spawnPointName;
    //dialogue and story variation
    public string storyType;

    public bool testMode = true;
    //only transition through door when condition is met
    public bool conditionMet = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && (conditionMet || testMode))
        {
            Debug.Log("Player collided with door! Transitioning...");

            //store info for the story scene
            SceneData.spawnPoint = spawnPointName;
            SceneData.storyType = storyType;

            // Load story scene
            SceneManager.LoadScene(storySceneName);
        }
        else
        {
            Debug.Log("Door is locked, condition not met.");
        }
    }
}
