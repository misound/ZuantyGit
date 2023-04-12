using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAudioSE : MonoBehaviour
{
    public AudioSource bgmAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        bgmAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bgmAudioSource.volume = GameSetting.BGMAudio.BGM_audioSource.volume;
    }
}
