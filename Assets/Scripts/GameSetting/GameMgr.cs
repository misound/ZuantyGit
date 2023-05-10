using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameMgr : MonoBehaviour
{
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
    
    [Header("KeyUI")]
    [SerializeField] public Button KeyBtn;
    [SerializeField] public Button KeyBackBtn;
    [SerializeField] public GameObject KeyBoardUI;
    
    [Header("StuffUI")]
    [SerializeField] public Button StuffBtn;
    [SerializeField] public GameObject StuffUI;
    [SerializeField] public Button StuffBackBtn;
    
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
        KeyBtn.onClick.AddListener(Key);
        StuffBtn.onClick.AddListener(Stuff);
        VBack.onClick.AddListener(VolumeBack);
        KeyBackBtn.onClick.AddListener(VolumeBack);
        StuffBackBtn.onClick.AddListener(VolumeBack);
        audioMgr = GameSetting.BGMAudio;

    }
    private void Update()
    {
        PauseStates();
        Pause();
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
        Volume,
        Key,
        Stuff,
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
                KeyBoardUI.SetActive(false);
                StuffUI.SetActive(false);
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
                KeyBoardUI.SetActive(false);
                StuffUI.SetActive(false);
                Time.timeScale = 0;
                break;
            case (int)ePauseStates.Volume:
                VolumeUI.SetActive(true);
                OptionUI.SetActive(false);
                pauseUI.SetActive(false);
                Panal.SetActive(true);
                KeyBoardUI.SetActive(false);
                StuffUI.SetActive(false);
                Time.timeScale = 0;
                break;
            case (int)ePauseStates.Key:
                VolumeUI.SetActive(false);
                OptionUI.SetActive(false);
                pauseUI.SetActive(false);
                Panal.SetActive(true);
                KeyBoardUI.SetActive(true);
                StuffUI.SetActive(false);
                Time.timeScale = 0;
                break;
            case (int)ePauseStates.Stuff:
                VolumeUI.SetActive(false);
                OptionUI.SetActive(false);
                pauseUI.SetActive(false);
                Panal.SetActive(true);
                KeyBoardUI.SetActive(false);
                StuffUI.SetActive(true);
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
            pausestates = 2;
        }
        else if(OpEnabled == true)
        {
            OpEnabled = false;
            OptionUI.SetActive(false);
            pausestates = 1;
        }
    }
    #endregion
    #region 音樂
    private void Volume()
    {
        pausestates = 3;
    }
    private void VolumeBack()
    {
        pausestates = 2;
    }
    
    #endregion
    
    public void Key()
    {
        pausestates = 4;
    }

    public void Stuff()
    {
        pausestates = 5;
    }
}


