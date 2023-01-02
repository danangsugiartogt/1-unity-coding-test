using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static UIController instance; // The singleton instance

    public static UIController Instance
    {
        get
        {
            // If the instance is null, search for an object with the UIController component
            if (instance == null)
            {
                instance = FindObjectOfType<UIController>();
            }
            return instance;
        }
    }

    public GameObject obj_winText;
    public Text text_score;

    void Awake()
    {
        // If there is already an instance of UIController, destroy this object
        if (instance != null)
        {
            Destroy(gameObject);
        }
        // Otherwise, set this object as the singleton instance
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void DisplayWinText()
    {
        obj_winText.SetActive(true);
    }

    public void SetScore(int score)
    {
        text_score.text = "Score: " + score.ToString();
    }
}
