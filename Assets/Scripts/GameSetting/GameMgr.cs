using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    int maxPlatform = 0;
    public GameOverScreen GameOverScreen;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GameOver()
    {
        
        GameOverScreen.Setup(maxPlatform);
    }

    
}


