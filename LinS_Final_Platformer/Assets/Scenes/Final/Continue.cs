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
    public KingsOrder kingsOrder;
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
        //dialogue lines in order
        dialogueText.text = dialogueLines[index];

        // Hide phone and panels at start
        if (phoneSprite != null) phoneSprite.SetActive(false);
        if (kingsOrder != null)
        {
            kingsOrder.HideBlackPanel();
            kingsOrder.HideKingsOrder();
            kingsOrder.HideHint();
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
        else
        {
            //or else finish dialogue is true
            dialogueFinished = true;
            //show panel immediately
            ShowPanel();
            
        }
    }

    void ShowPanel()
    {
        //activate phone sprite
        if (phoneSprite != null) phoneSprite.SetActive(true);

        //activate King’s Order panel with black panel
        if (kingsOrder != null)
        {
            //show black panel and kings order
            kingsOrder.ShowKingsOrder();
            Debug.Log("kings order called");
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
