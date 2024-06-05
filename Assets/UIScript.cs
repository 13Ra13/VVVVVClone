using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    private int scID;

    public void StartGame()
    {
        scID = PlayerPrefs.GetInt("PreviousSceneIndex", 0);
        if (scID >6 || scID <=0 )
        {
            scID = 1; 
        }
        SceneManager.LoadScene(scID);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
