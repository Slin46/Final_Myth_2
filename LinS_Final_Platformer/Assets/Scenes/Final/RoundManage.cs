using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManage : MonoBehaviour
{
    public static RoundManage Instance;

    //1 minute countdown
    public float roomTime = 60f;
    private float currentTime;
    public TMP_Text timerText;
    private bool roomActive = false;
    //set condition met to false
    public bool conditionMet = false;

    //say door is unlocked when condition is met
    public TMP_Text messageText;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartRoom();
    }

    public void StartRoom()
    {
        currentTime = roomTime;
        roomActive = true;
        conditionMet = false;

        //deactivate the text
        if (messageText != null)
            messageText.text = "";
    }

    private void Update()
    {
        if (!roomActive) return;

        currentTime -= Time.deltaTime;

        if (currentTime < 0f)
            currentTime = 0f;

        // Update TMP_Text if assigned
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = string.Format("Time Left: {0:00}:{1:00}", minutes, seconds);
        }
      
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            roomActive = false;
            conditionMet = false;

            // Set story type for fail
            SceneData.storyType = "Fail";

            // Increment current room index for persistence
            int nextRoom = PlayerPrefs.GetInt("CurrentRoom", 0) + 1;
            PlayerPrefs.SetInt("CurrentRoom", nextRoom);
            PlayerPrefs.Save();

            // Update KingsOrder current room so the next order shows correctly
            if (KingsOrder.Instance != null)
            {
                KingsOrder.Instance.currentRoom = nextRoom;
                KingsOrder.Instance.OnRoomEnd(false); // will skip hint since condition not met
            }
            SceneData.spawnPoint = "Spawn_" + (char)('A' + nextRoom);

            // Now load the StoryScene
            SceneManager.LoadScene("StoryScene");

        }
    }


    // Call this when player meets the condition (before time runs out)
    public void PlayerMetCondition()
    {
        roomActive = false;      // still mark room as inactive after condition met
        conditionMet = true;     // ensures door knows it can open

        SceneData.storyType = "Success";

        if (messageText != null)
            messageText.text = "Door is unlocked";
        else
            Debug.Log("Door is unlocked");

        if (KingsOrder.Instance != null)
            KingsOrder.Instance.OnRoomEnd(conditionMet);
    }
    

}
