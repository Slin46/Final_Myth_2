using TMPro;
using UnityEngine;

public class StoryScene : MonoBehaviour
{
    public TMP_Text dialogueText;  // assign your TMP_Text in Inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string storyType = SceneData.storyType;

        if (dialogueText != null)
        {
            switch (storyType)
            {
                case "Success":
                    dialogueText.text = "The King's order failed. No one died.";
                    break;
                case "Fail":
                    dialogueText.text = "The King's order has been fulfilled. Someone died.";
                    break;
                default:
                    dialogueText.text = "..."; // optional: clear default dialogue
                    break;
            }
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
