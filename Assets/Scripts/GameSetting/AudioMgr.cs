using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : MonoBehaviour
{
    [SerializeField] public AudioSource MBGM;
    // Start is called before the first frame update
    void Start()
    {
        if (GameSetting.AudioReady)
        {
            Destroy(gameObject); //kill selft
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            GameSetting.Audio = this;
            GameSetting.AudioReady = true;
        }
        MBGM = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        MBGM.pitch = Time.timeScale;
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
        GameSetting.Audio.PlayBoom();
        GameSetting.Audio.Play(eAudio.BGM1);
    }
    public void Btn2()
    {
        GameSetting.Audio.PlayBoom();
        GameSetting.Audio.Play(eAudio.BGM2);
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
