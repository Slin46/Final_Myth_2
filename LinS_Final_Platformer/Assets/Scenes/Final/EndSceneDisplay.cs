using TMPro;
using UnityEngine;

public class EndSceneDisplay : MonoBehaviour
{
    public TMP_Text resultText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (resultText != null)
        {
            string result = PlayerPrefs.GetString("EndResult", "No result found.");
            resultText.text = result;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
