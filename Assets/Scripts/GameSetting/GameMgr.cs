using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameMgr : MonoBehaviour
{
    int maxPlatform = 0;
    public GameOverScreen GameOverScreen;

    UnityEngine.Rendering.VolumeProfile volumeProfile;
    public PlayerController playerController;
    public TakeEnemy takeEnemy;
    bool pauseEnabled;

    public GameObject Panal;
    public Image pauseImage;
    public Button con;
    public Button btm;

    UnityEvent PauseEvent = new UnityEvent();

    private void Start()
    {
        PauseEvent.AddListener(PauseUI);
        playerController = FindObjectOfType<PlayerController>();
        takeEnemy = FindObjectOfType<TakeEnemy>();
    }
    private void Update()
    {
        Pause();

    }
    private void FixedUpdate()
    {
        Volume();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GameOver()
    {

        GameOverScreen.Setup(maxPlatform);
    }

    public void Pause()
    {
        if (Input.GetKeyDown("escape"))
        {
            //check if game is already paused
            if (pauseEnabled == true)
            {
                //unpause the game
                pauseEnabled = false;
                Time.timeScale = 1;
                AudioListener.volume = 1;
                Cursor.visible = false;
            }

            //else if game isn't paused, then pause it
            else if (pauseEnabled == false)
            {
                pauseEnabled = true;
                AudioListener.volume = 0;
                Time.timeScale = 0;
                Cursor.visible = true;
                PauseEvent.Invoke();
            }
        }

    }
    public void PauseUI()
    {
        Panal.SetActive(true);
    }
    private void Volume()
    {
        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

        // You can leave this variable out of your function, so you can reuse it throughout your class.
        UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration;

        if (!volumeProfile.TryGet(out chromaticAberration)) throw new System.NullReferenceException(nameof(chromaticAberration));
        if (takeEnemy.slaind)
            chromaticAberration.intensity.Override(0.5f);
        else
            chromaticAberration.intensity.Override(0f);
    }

}


