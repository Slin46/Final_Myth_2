using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Continue : MonoBehaviour
{
    [Header("Dialogue Setup")]
    //dialogue lines inside inspector
    public TMP_Text dialogueText;
    public string[] dialogueLines;
    private int index = 0;

    [Header("Next Scene")]
    //phone sprite and next game scene
    public GameObject phoneSprite;
    public GameObject dialogueBox;
    public GameObject continueArrow;
    public KingsOrder kingsOrder;
    public bool Hint = false;
    public string nextSceneName = "GameScene";

    [Header("Scene Data")]
    //assign spawn point name so that i can come back to story scene but transition to a different room after story ends
    public string returnSpawnPointName;
    public string storyType;

    //dialogue and panel false on start
    private bool dialogueFinished = false;
    private bool panelShown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Replace dialogue if storyType from SceneData exists
        string sceneStoryType = SceneData.storyType;

        if (!string.IsNullOrEmpty(sceneStoryType))
        {
            switch (sceneStoryType)
            {
                case "Success":
                    dialogueLines = new string[] { "The King's order failed. No one died." };
                    break;
                case "Fail":
                    dialogueLines = new string[] { "The King's order has been fulfilled. Someone died." };
                    break;
                default:
                    break;
            }
        }

        // Set the first line of dialogue
        dialogueText.text = dialogueLines[index];

        // Hide phone and panels at start
        if (phoneSprite != null) phoneSprite.SetActive(false);
        if (kingsOrder != null)
        {
            kingsOrder.HideBlackPanel();
            kingsOrder.HideKingsOrder();
            kingsOrder.HideHint();
            
        }
        if (kingsOrder.currentRoom != 0)
        {
            dialogueFinished = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Next()
    {
        //if diaogue is not finished, continue dialogue
        if (!dialogueFinished)
        {
            ContinueDialogue();
        }

        else if (kingsOrder.currentRoom !=0 && RoundManage.Instance.conditionMet && !Hint)
        {
                Hint = true;
                ShowHintPanel();
        }
        else if (!panelShown)
        {
            //show panel after dialogue finishes
            ShowPanel();

        }
        else
        {
            //panel already visible, go to next scene
            LoadNextScene();
        }
    }

    void ContinueDialogue()
    {
        //if the index list is less than the dialogue length than minus one on click
        if (index < dialogueLines.Length - 1)
        {
            index++;
            dialogueText.text = dialogueLines[index];
        }
        if (index == dialogueLines.Length - 1)
        {
            //or else finish dialogue is true
            dialogueFinished = true;
            return;
            
        }
    }

    void ShowHintPanel()
    {
        //activate phone sprite
        if (phoneSprite != null) phoneSprite.SetActive(true);


        //activate King’s Order panel with black panel
        if (kingsOrder != null)
        {
            //show black panel and kings order
            kingsOrder.ShowHint();
            Debug.Log("Hint called");

            if (continueArrow != null && kingsOrder.finalTestPanel != null && kingsOrder.finalTestPanel.activeSelf)
                continueArrow.SetActive(false);
        }

        //hide dialogue box
        if (dialogueBox != null) dialogueBox.SetActive(false);
    }

    void ShowPanel()
    {
        kingsOrder.HideHint();

        //activate phone sprite
        if (phoneSprite != null) phoneSprite.SetActive(true);


        //activate King’s Order panel with black panel
        if (kingsOrder != null)
        {
            //show black panel and kings order
            kingsOrder.ShowKingsOrder();
            Debug.Log("kings order called");

            if (continueArrow != null && kingsOrder.finalTestPanel != null && kingsOrder.finalTestPanel.activeSelf)
                continueArrow.SetActive(false);
        }

        //hide dialogue box
        if (dialogueBox != null) dialogueBox.SetActive(false);

        panelShown = true;
    }

    void LoadNextScene()
    {
        //save spawn point & story type for Game Scene
        if (!string.IsNullOrEmpty(returnSpawnPointName))
        {
            SceneData.spawnPoint = returnSpawnPointName;
        }
        if (!string.IsNullOrEmpty(storyType))
        {
            SceneData.storyType = storyType;
        }

        //load the next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

}
