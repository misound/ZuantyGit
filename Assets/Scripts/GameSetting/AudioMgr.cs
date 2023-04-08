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
    public OldPlayerController Playermoves;
    public EnemyBomb enemyBomb;

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
        BGM_audioSource.PlayOneShot(BGM[0]);
        BGM_audioSource.loop = true;
    }
    private void Start()
    {
        Playermoves = FindObjectOfType<OldPlayerController>();
        enemyBomb = FindObjectOfType<EnemyBomb>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameSetting.AudioReady)
        {
            titleMgr = FindObjectOfType<TitleMgr>();
            gameMgr = FindObjectOfType<GameMgr>();
        }

        if (!BGM_audioSource.isPlaying)
        {
            BGM_audioSource.PlayOneShot(BGM[0]);
        }
        AudioBigSmall();
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
            default:
                Debug.LogError("NONE DEF");
                break;
        }
    }
    #endregion
    public void PlayBoom()
    {
        if (SE_audioSource == null)
        {
            return;
        }
        SE_audioSource.PlayOneShot(SE[1]);
        
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


        if (gameMgr != null && BGMCheck) //傳遞
        {
            //gameMgr.mainBGM.value = GameSetting.AudioVolume;
            gameMgr.mainBGM.value = GameSetting.BGMAudio.BGM_audioSource.volume;


            BGMCheck = false;
        }
        if (gameMgr != null && !BGMCheck) //接收
        {
            //GameSetting.AudioVolume = gameMgr.mainBGM.value;
            GameSetting.BGMAudio.BGM_audioSource.volume  = gameMgr.mainBGM.value;
        }

    }
    #endregion
}
