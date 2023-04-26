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

    [SerializeField] public bool BGMCheck = false;
    [SerializeField] public bool SECheck = false;

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
        BGM_audioSource.PlayOneShot(BGM[0]);
        BGM_audioSource.loop = true;
    }

    private void Start()
    {
        AudioBigSmall();
        SEAudioBigSmall();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameSetting.AudioReady)
        {
            titleMgr = FindObjectOfType<TitleMgr>();
            gameMgr = FindObjectOfType<GameMgr>();
            GameSetting.AudioReady = false;
        }

        if (!BGM_audioSource.isPlaying)
        {
            BGM_audioSource.PlayOneShot(BGM[0]);
        }

        //Debug.Log(GameSetting.AudioReady);
        
        AudioBigSmall();
        SEAudioBigSmall();
    }

    #region BGM管理

    public enum eAudio
    {
        BGM1,
        BGM2,
        SE_BoomRun,
        SE_BoomBloom,
        SE_BoomAtk,
        SE_BoomDie,
        SE_Player_Attack_01,
        SE_Player_Attack_02,
        SE_Player_Attack_03,
        SE_Player_Hit,
        SE_Elevator,
        SE_Atk_Door_Broken,
        SE_Atk_Wall_Broken,
        SE_WalkRun,
        SE_WalkFind,
        SE_WalkAtk,
    }

    public void Play(eAudio audio)
    {
        switch (audio)
        {
            case eAudio.BGM1:
                BGM_audioSource.PlayOneShot(BGM[0]);
                break;
            case eAudio.BGM2:
                SE_audioSource.PlayOneShot(SE[1]);
                break;
            case eAudio.SE_Elevator:
                SE_audioSource.PlayOneShot(SE[2]);
                break;
            case eAudio.SE_BoomRun:
                SE_audioSource.PlayOneShot(SE[4]);
                break;
            case eAudio.SE_BoomBloom:
                SE_audioSource.PlayOneShot(SE[5]);
                break;
            case eAudio.SE_BoomAtk:
                SE_audioSource.PlayOneShot(SE[6]);
                break;
            case eAudio.SE_BoomDie:
                SE_audioSource.PlayOneShot(SE[7]);
                break;
            case eAudio.SE_Player_Attack_01:
                SE_audioSource.PlayOneShot(SE[8]);
                break;
            case eAudio.SE_Player_Attack_02:
                SE_audioSource.PlayOneShot(SE[9]);
                break;
            case eAudio.SE_Player_Attack_03:
                SE_audioSource.PlayOneShot(SE[10]);
                break;
            case eAudio.SE_Player_Hit:
                SE_audioSource.PlayOneShot(SE[11]);
                break;
            case eAudio.SE_Atk_Door_Broken:
                SE_audioSource.PlayOneShot(SE[12]);
                break;
            case eAudio.SE_Atk_Wall_Broken:
                SE_audioSource.PlayOneShot(SE[13]);
                break;
            case eAudio.SE_WalkRun:
                //SE_audioSource.PlayOneShot(SE[14]);
                break;
            case eAudio.SE_WalkFind:
                SE_audioSource.PlayOneShot(SE[15]);
                break;
            case eAudio.SE_WalkAtk:
                SE_audioSource.PlayOneShot(SE[16]);
                break;
            default:
                Debug.LogError("NONE DEF");
                break;
        }
    }

    #endregion

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
            GameSetting.BGMAudio.BGM_audioSource.volume = titleMgr.TBGMSli.value;
        }


        if (gameMgr != null && BGMCheck) //傳遞
        {
            
            gameMgr.mainBGM.value = GameSetting.BGMAudio.BGM_audioSource.volume;


            BGMCheck = false;
        }

        if (gameMgr != null && !BGMCheck) //接收
        {
            
            GameSetting.BGMAudio.BGM_audioSource.volume = gameMgr.mainBGM.value;
        }
    }

    #endregion

    #region SE大小

    public void SEAudioBigSmall()
    {
        if (titleMgr != null && !SECheck)
        {
            titleMgr.TSESli.value = GameSetting.SEAudio.SE_audioSource.volume;

            SECheck = true;
        }

        if (titleMgr != null && SECheck)
        {
            GameSetting.SEAudio.SE_audioSource.volume = titleMgr.TSESli.value;
        }


        if (gameMgr != null && SECheck) //傳遞
        {
            gameMgr.mainSE.value = GameSetting.SEAudio.SE_audioSource.volume;
            SECheck = false;
        }

        if (gameMgr != null && !SECheck) //接收
        {
            GameSetting.SEAudio.SE_audioSource.volume = gameMgr.mainSE.value;
        }
        
    }

    #endregion
}