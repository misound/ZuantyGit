using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    int maxPlatform = 0;
    public GameOverScreen GameOverScreen;

    UnityEngine.Rendering.VolumeProfile volumeProfile;
    public PlayerController playerController;
    public TakeEnemy takeEnemy;

    private void Start()
    {

        playerController = FindObjectOfType<PlayerController>();
        takeEnemy = FindObjectOfType<TakeEnemy>();
    }

    private void FixedUpdate()
    {
        Volume();
    }
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
    private void Volume()
    {
        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

        // You can leave this variable out of your function, so you can reuse it throughout your class.
        UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration;

        if (!volumeProfile.TryGet(out chromaticAberration)) throw new System.NullReferenceException(nameof(chromaticAberration));
        if(takeEnemy.slaind)
        chromaticAberration.intensity.Override(0.5f);
        else
            chromaticAberration.intensity.Override(0f);
    }
    
}


