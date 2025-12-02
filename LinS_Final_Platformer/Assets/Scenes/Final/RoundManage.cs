using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManage : MonoBehaviour
{
    public static RoundManage Instance;

    //room completed set to flase at first
    public bool room1Done = false;
    public bool room2Done = false; 
    public bool room3Done = false;
    public bool room4Done = false;

    //drag final test UI in inspector
    public GameObject finaltestUI;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RoomComplete(int roomNumber)
    {
        if (room1Done && room2Done && room3Done && room4Done)
        {
            ActivateFinalTest();
            
        }
    }

    private void ActivateFinalTest()
    {
       if (finaltestUI !=null)
       {
            finaltestUI.SetActive(true);
            Debug.Log("final test activated");
       }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
