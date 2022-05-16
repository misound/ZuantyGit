using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    int maxPlatform = 0;
    public GameOverScreen GameOverScreen;
    public void GameOver()
    {
        
        GameOverScreen.Setup(maxPlatform);
    }

    
}


