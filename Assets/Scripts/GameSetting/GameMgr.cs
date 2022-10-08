using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameMgr : MonoBehaviour
{
    int maxPlatform = 0;
    public GameOverScreen GameOverScreen;

    UnityEngine.Rendering.VolumeProfile volumeProfile;
    public PlayerController playerController;
    public TakeEnemy takeEnemy;
    static bool pauseEnabled;
    static bool OpEnabled;


    public GameObject Panal;
    public GameObject OptionUI;
    public Image pauseImage;
    public Button con;
    public GameObject Pausefirstbtn;
    public Button option;
    public GameObject Optionfirstbtn;
    public Button btm;
    public Button OpBack;
    public Button volume;
    public Slider mainBGM;
    public GameObject mainBGMSli;
    public AudioSource MBGM;

    UnityEvent PauseEvent = new UnityEvent();

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PauseEvent.AddListener(PauseUI);
        con.onClick.AddListener(ContinueBtn);
        btm.onClick.AddListener(BackToTitle);
        option.onClick.AddListener(Option);
        OpBack.onClick.AddListener(Option);
        volume.onClick.AddListener(Volume);
        playerController = FindObjectOfType<PlayerController>();
        takeEnemy = FindObjectOfType<TakeEnemy>();
    }
    private void Update()
    {
        Pause();
        MBGM.volume = mainBGM.value;
    }
    private void FixedUpdate()
    {
        VisualEffect();
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
                //AudioListener.volume = 1;
                Panal.SetActive(false);
                OpEnabled = false;
            }

            //else if game isn't paused, then pause it
            else if (pauseEnabled == false)
            {
                pauseEnabled = true;
                //AudioListener.volume = 0;
                Time.timeScale = 0;
                PauseEvent.Invoke();
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(Pausefirstbtn);
            }
        }
    }
    public void PauseUI()
    {
        if(pauseEnabled)
            Panal.SetActive(true);
        if(!pauseEnabled)
            Panal.SetActive(false);
    }
    public void ContinueBtn()
    {
        Panal.SetActive(false);
        if (pauseEnabled == true)
        {
            //unpause the game
            pauseEnabled = false;
            Time.timeScale = 1;
            //AudioListener.volume = 1;
        }
    }

    public void Option()
    {
        if (OpEnabled == false)
        {
            OpEnabled = true;
            OptionUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(Optionfirstbtn);
        }
        else if(OpEnabled == true)
        {
            OpEnabled = false;
            OptionUI.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(Pausefirstbtn);
        }

    }
    private void Volume()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainBGMSli);
    }
    private void BackToTitle()
    {
        SceneManager.LoadScene(0);
    }
    private void VisualEffect()
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


