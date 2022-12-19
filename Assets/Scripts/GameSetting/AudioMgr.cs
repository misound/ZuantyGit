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


        Step();
        if (enemyBomb.explosioned)
        {
            //PlayBoom();
        }

        AudioBigSmall();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 1000, 160, 100), "BGM1"))
        {
            Play(eAudio.BGM1);
        }
    }
    #region ���ֺ޲z
    public enum eAudio
    {
        BGM1,
        BGM2,
        SE3,
    }

    public void Play(eAudio audio)
    {
        switch (audio)
        {
            case eAudio.BGM1:
                BGM_audioSource.PlayOneShot(BGM[1]);
                break;
            case eAudio.BGM2:
                SE_audioSource.PlayOneShot(SE[1]);
                break;
            default:
                Debug.LogError("NONE DEF");
                break;
        }
    }
    #endregion
    #region �}�B�n
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
        Play(eAudio.BGM2);
        SE_audioSource = GetComponent<AudioSource>();
    }
    #region BGM�j�p
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


        if (gameMgr != null && BGMCheck) //�ǻ��ܼ�
        {
            gameMgr.mainBGM.value = GameSetting.BGMAudio.BGM_audioSource.volume;
            
            BGMCheck = false;
        }
        if (gameMgr != null && !BGMCheck) //��X�ܥi���Ʊ�
        {
            GameSetting.BGMAudio.BGM_audioSource.volume = gameMgr.mainBGM.value;
        }

    }
    #endregion
}
