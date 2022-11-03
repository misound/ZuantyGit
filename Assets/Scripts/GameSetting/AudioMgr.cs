using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : MonoBehaviour
{
    [SerializeField] public AudioSource MBGM;

    public GameMgr gameMgr;
    public TitleMgr titleMgr;

    [SerializeField] public AudioClip BGM1;
    [SerializeField] public AudioClip SE1;
    // Start is called before the first frame update
    void Awake()
    {
        //MBGM.Play();
        if (GameSetting.AudioReady)
        {
            Destroy(gameObject); //kill self
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            GameSetting.BGMAudio = this;
            GameSetting.AudioReady = true;
        }
        MBGM = GetComponent<AudioSource>();
        titleMgr = FindObjectOfType<TitleMgr>();
        gameMgr = FindObjectOfType<GameMgr>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameSetting.AudioReady)
        {
            titleMgr = FindObjectOfType<TitleMgr>();
            gameMgr = FindObjectOfType<GameMgr>();
        }

        //MBGM.volume = gameMgr.mainBGM.value = titleMgr.TBGMSli.value;
        MBGM.pitch = Time.timeScale;
        if(Time.timeScale < 0.7&& Time.timeScale > 0.05)
        {
            MBGM.pitch = 0.7f;
        }
        if (Time.timeScale > 0.7 && Time.timeScale < 0.05)
        {
            MBGM.pitch = Time.timeScale;
        }
    }
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
                break;
            case eAudio.BGM2:
                break;
            default:
                Debug.LogError("NONE DEF");
                break;
        }
    }
    public void Btn()
    {
        GameSetting.BGMAudio.PlayBoom();
        GameSetting.BGMAudio.Play(eAudio.BGM1);
    }
    public void Btn2()
    {
        GameSetting.BGMAudio.PlayBoom();
        GameSetting.BGMAudio.Play(eAudio.BGM2);
    }
    public void PlayBoom()
    {
        if (MBGM == null)
        {
            return;
        }

        MBGM = GetComponent<AudioSource>();
        //_audioSource.Play();
        //_audioSource.PlayOneShot(boom);
    }
}
