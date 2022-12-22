using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameMgr : MonoBehaviour
{
    public OldPlayerController playerController;
    public TakeEnemy takeEnemy;
    public TitleMgr titleMgr;
    private AudioMgr audioMgr;

    static bool pauseEnabled;
    static bool OpEnabled;

    [Header("PauseUI")]
    [SerializeField] public GameObject pauseUI;
    [SerializeField]public GameObject Panal;
    [SerializeField] public Button con;
    [SerializeField] public Button option;
    [SerializeField] public Button btt;
    [SerializeField] public GameObject Pausefirstbtn;

    [Header("OptionUI")]
    [SerializeField] public GameObject OptionUI;
    [SerializeField] public GameObject Optionfirstbtn;
    [SerializeField] public Button OpBack;

    [Header("VolumeUI")]
    [SerializeField] public GameObject VolumeUI;
    [SerializeField] public Button volume;
    [SerializeField] public Button VBack;
    [SerializeField] public Slider mainBGM;
    [SerializeField] public GameObject mainBGMSli;
    [SerializeField] public Slider mainSE;
    [SerializeField] public GameObject mainSESli;
    UnityEvent PauseEvent = new UnityEvent();

    public int pausestates;
    private void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        PauseEvent.AddListener(PauseUI);
        con.onClick.AddListener(ContinueBtn);
        btt.onClick.AddListener(BackToTitle);
        option.onClick.AddListener(Option);
        OpBack.onClick.AddListener(Option);
        volume.onClick.AddListener(Volume);
        VBack.onClick.AddListener(VolumeBack);
        playerController = FindObjectOfType<OldPlayerController>();
        takeEnemy = FindObjectOfType<TakeEnemy>();
        // audioMgr = FindObjectOfType<AudioMgr>();
        audioMgr = GameSetting.BGMAudio;

    }
    private void Update()
    {
        PauseStates();
        Pause();
        //GameSetting.BGMAudio.MBGM.volume = mainBGM.value;
        //M_BGM.volume = mainSE.value = titleMgr.TSESli.value;
    }
    private void FixedUpdate()
    {
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    #region 狀態管理
    public enum ePauseStates
    {
        Playing,
        Pause,
        Option,
        Volume
    }
    private void PauseStates()
    {
        switch (pausestates)
        {
            case (int)ePauseStates.Playing:
                OptionUI.SetActive(false);
                Panal.SetActive(false);
                pauseUI.SetActive(false);
                VolumeUI.SetActive(false);
                break;
            case (int)ePauseStates.Pause:
                //OptionUI.SetActive(false);
                //pauseUI.SetActive(true);
                //Panal.SetActive(true);
                //VolumeUI.SetActive(false);
                Time.timeScale = 0;
                break;
            case (int)ePauseStates.Option:
                OptionUI.SetActive(true);
                pauseUI.SetActive(false);
                Panal.SetActive(true);
                VolumeUI.SetActive(false);
                Time.timeScale = 0;
                break;
            case (int)ePauseStates.Volume:
                VolumeUI.SetActive(true);
                OptionUI.SetActive(false);
                pauseUI.SetActive(false);
                Panal.SetActive(true);
                Time.timeScale = 0;
                break;
        }
    }

    #endregion
    #region 暫停介面
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
                Panal.SetActive(false);
                OpEnabled = false;
                pausestates = 0;
            }

            //else if game isn't paused, then pause it
            else if (pauseEnabled == false)
            {
                pauseEnabled = true;
                Time.timeScale = 0;
                PauseEvent.Invoke();
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(Pausefirstbtn);
                pausestates = 1;
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
            pausestates = 0;
            //AudioListener.volume = 1;
        }
    }
    private void BackToTitle()
    {
        SceneManager.LoadScene(0);
    }
    #endregion
    #region 選項介面
    public void Option()
    {
        if (OpEnabled == false)
        {
            OpEnabled = true;
            OptionUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(Optionfirstbtn);
            pausestates = 2;
        }
        else if(OpEnabled == true)
        {
            OpEnabled = false;
            OptionUI.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(Pausefirstbtn);
            pausestates = 1;
        }
    }
    #endregion
    #region 音樂
    private void Volume()
    {
        pausestates = 3;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainBGMSli);
    }
    private void VolumeBack()
    {
        pausestates = 2;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(Optionfirstbtn);
    }


    #endregion

}


