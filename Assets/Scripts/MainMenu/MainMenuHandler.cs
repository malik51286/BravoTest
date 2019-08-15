using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetString("PreviousScene", SceneList.MenuScene);
        PlayerPrefs.Save();
    }
    public void OnStartGame()
    {
        SceneManager.LoadScene(SceneList.GameScene);
    }
    public void OnViewLeaderBoard()
    {
        SceneManager.LoadScene(SceneList.LeaderBoardScene);
    }
    public void OnQuit()
    {
        Application.Quit();
    }
}
