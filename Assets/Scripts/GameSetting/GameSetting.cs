using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSetting
{
    public static bool AudioReady = false;

    public static AudioMgr BGMAudio;
    public static AudioMgr SEAudio;

    public static float Playerposx;
    public static float Playerposy;

    public static Vector3 Playerpos;

    public static int PlayerHP;

    public static IList<Itemdata> DList;
    public static IList<AtkWData> WList;
    
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
        Playerpos.x = PlayerPrefs.GetFloat("x");
        Playerpos.y = PlayerPrefs.GetFloat("y");
        PlayerHP = PlayerPrefs.GetInt("AAA");
        BGMAudio.BGM_audioSource.volume = PlayerPrefs.GetFloat("BBB");
    }
}
