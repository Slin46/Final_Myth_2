using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Continue : MonoBehaviour
{
    [Header("Dialogue Setup")]
    public TMP_Text dialogueText;
    public string[] dialogueLines;
    private int index = 0;

    [Header("Next Scene")]
    public GameObject nextPanel;
    public string nextSceneName = "GameScene";

    [Header("Scene Data")]
    //assign spawn point name so that i can come back to story scene but transition to a different room after story ends
    public string returnSpawnPointName;
    public string storyType;

    private bool dialogueFinished = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //dialogue lines in order
        dialogueText.text = dialogueLines[index];
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Next()
    {
        if (!dialogueFinished)
        {
            ContinueDialogue();
        }
        else
        {
            ContinueToNext();
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
        }
    }

    void ContinueToNext()
    {
        //continue to panel if theres one
        if (nextPanel != null)
        {
            nextPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        //save spawn point & story type for Game Scene
        if (!string.IsNullOrEmpty(returnSpawnPointName))
        {
            SceneData.spawnPoint = returnSpawnPointName;
        }
        if (!string.IsNullOrEmpty(storyType))
        {
            SceneData.storyType = storyType;
        }

        //continue to next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
