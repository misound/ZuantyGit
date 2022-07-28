using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text pointText;
    public void Setup(int score)
    {
        gameObject.SetActive(true);
        pointText.text = score.ToString() + "point";
    }

    public void RestartBtn()
    {
        SceneManager.LoadScene("Game");
    }

    public void MenuBtn()
    {
        SceneManager.LoadScene("Menu");
    }
}
