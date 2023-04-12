using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAudioBGM : MonoBehaviour
{
    public AudioSource SeAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        SeAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        SeAudioSource.volume = GameSetting.SEAudio.SE_audioSource.volume;
    }
}
