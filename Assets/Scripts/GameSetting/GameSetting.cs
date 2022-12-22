using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSetting
{
    public static OldPlayerController playerController;

    public static bool AudioReady = false;

    public static AudioMgr BGMAudio;
    public static AudioMgr SEAudio;

    public static float AudioVolume;
    public static float[] Playerpos;

    public static int PlayerHP;

    public static bool DoorBroken;
    public static void Save()
    {
        PlayerPrefs.SetString("AudioReady", AudioReady.ToString());
        PlayerPrefs.SetInt("AAA", PlayerHP);
        PlayerPrefs.SetFloat("BBB", BGMAudio.BGM_audioSource.volume);
        PlayerPrefs.Save();
    }

    public static void Load()
    {
        AudioReady = bool.Parse(PlayerPrefs.GetString("AudioReady", "false"));

        PlayerHP = PlayerPrefs.GetInt("AAA");
        BGMAudio.BGM_audioSource.volume = PlayerPrefs.GetFloat("BBB");
    }
}
