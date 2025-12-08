using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Submit : MonoBehaviour
{
    public TMP_InputField inputField;
    //list of acceptable answers
    private string[] correctAnswers = new string[]
    {
        "the king is philip",
        "philip is the king",
        "king is philip",
        "philip is king",
        "king philip"

    };

    public string endSceneName = "EndScene";
    public float delayBeforeScene = 2f; // seconds to wait before loading end scene
    private bool answered = false;
    public void OnSubmit()
    {
        // remove extra spaces and lowercase
        string playerText = inputField.text.Trim().ToLower();

        // If input is empty, just return and do nothing
        if (string.IsNullOrEmpty(playerText))
        {
            Debug.Log("Please type an answer before submitting!");
            return;
        }

        if (answered) return; // prevent multiple submissions
        answered = true;

        bool isCorrect = false;
        
        //if the answer is within the list then the answer is correct
        foreach(string answer in correctAnswers)
        {
            if(playerText == answer)
            {
                isCorrect = true;
                break;
            }
        }

        if(isCorrect) 
        {
            Debug.Log("correct");
        }
        else
        {
            Debug.Log("incorrect");
        }

        // Save result in PlayerPrefs so EndScene can read it
        if (isCorrect)
            PlayerPrefs.SetString("EndResult", "You survived. The king's game has been broken");
        else
            PlayerPrefs.SetString("EndResult", "You die. The king's game continues...");

        PlayerPrefs.Save();

        // Load EndScene
        SceneManager.LoadScene(endSceneName);
    }
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //player can also submit answer by pressing enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnSubmit();
        }
    }
}
