using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioMgr : MonoBehaviour
{
    [SerializeField] public AudioSource[] Array_audioSource;
    [SerializeField] public AudioSource BGM_audioSource;
    [SerializeField] public AudioSource SE_audioSource;

    public GameMgr gameMgr;
    public TitleMgr titleMgr;
    public Test Playermoves;

    public bool BGMCheck = false;

    public AudioClip[] BGM;
    public AudioClip[] SE;
    // Start is called before the first frame update
    void Awake()
    {
        Array_audioSource = GetComponents<AudioSource>();

        if (GameSetting.AudioReady)
        {
            Destroy(gameObject); //kill self
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            GameSetting.BGMAudio = this;
            GameSetting.SEAudio = this;
            GameSetting.AudioReady = true;
        }
        BGM_audioSource = Array_audioSource[0];
        SE_audioSource = Array_audioSource[1];
    }
    private void Start()
    {
        Playermoves = FindObjectOfType<Test>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameSetting.AudioReady)
        {
            titleMgr = FindObjectOfType<TitleMgr>();
            gameMgr = FindObjectOfType<GameMgr>();
        }


        Step();
        AudioBigSmall();
        Debug.Log(BGMCheck);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 80, 160, 100), "BGM1"))
        {
            Play(eAudio.BGM1);
        }
    }
    #region 音樂管理
    public enum eAudio
    {
        BGM1,
        BGM2,
    }

    public void Play(eAudio audio)
    {
        switch (audio)
        {
            case eAudio.BGM1:
                BGM_audioSource.PlayOneShot(BGM[0]);
                break;
            case eAudio.BGM2:
                break;
            default:
                Debug.LogError("NONE DEF");
                break;
        }
    }
    #endregion
    #region 腳步聲
    public void Step()
    {
        if(Playermoves == null)
        {
            return;
        }
        if (Playermoves._horizontalDirection != 0)
        {
            Playermoves.isRunning = true;
        }
        else
        {
            Playermoves.isRunning = false;
        }

        if (Playermoves.isRunning)
        {
            if (!SE_audioSource.isPlaying)
            {
                SE_audioSource.PlayOneShot(SE[0]);
            }
        }
        else
        {
            SE_audioSource.Stop();
        }
    }
    #endregion
    public void PlayBoom()
    {
        if (SE_audioSource == null)
        {
            return;
        }

        SE_audioSource = GetComponent<AudioSource>();
    }
    #region BGM大小
    public void AudioBigSmall()
    {
        if (titleMgr != null && !BGMCheck)
        {
            titleMgr.TBGMSli.value = GameSetting.BGMAudio.BGM_audioSource.volume;

            BGMCheck = true;
        }

        if (titleMgr != null && BGMCheck)
        {
            GameSetting.BGMAudio.BGM_audioSource.volume=titleMgr.TBGMSli.value;
        }


        if (gameMgr != null && BGMCheck) //傳遞變數
        {
            gameMgr.mainBGM.value = GameSetting.BGMAudio.BGM_audioSource.volume;
            
            BGMCheck = false;
        }
        if (gameMgr != null && !BGMCheck) //輸出至可控滑桿
        {
            GameSetting.BGMAudio.BGM_audioSource.volume = gameMgr.mainBGM.value;
        }

    }
    #endregion
}
