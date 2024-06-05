using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public Button continueButton;
    public Button quitButton;
    private int previousSceneIndex;

    void Start()
    { 
        previousSceneIndex = PlayerPrefs.GetInt("PreviousSceneIndex", 0);

        continueButton.onClick.AddListener(ContinueGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(previousSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
