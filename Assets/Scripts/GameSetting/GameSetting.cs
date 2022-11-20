using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSetting
{
    public static bool AudioReady = false;

    public static AudioMgr BGMAudio;
    public static AudioMgr SEAudio;
    public static void Save()
    {
        PlayerPrefs.SetString("AudioReady", AudioReady.ToString());


        PlayerPrefs.Save();
    }

    public static void Load()
    {
        AudioReady = bool.Parse(PlayerPrefs.GetString("AudioReady", "false"));
    }
}
