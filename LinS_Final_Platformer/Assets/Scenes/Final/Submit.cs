using TMPro;
using UnityEngine;

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

    public void OnSubmit()
    {
        //remove extra spaces and lower case
        string playerText = inputField.text.Trim().ToLower();

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
