using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSetting
{
    public static bool AudioReady = false;

    public static AudioMgr BGMAudio;
    public static AudioMgr SEAudio;

    public static float Playerposx;
    public static float Playerposy;

    public static Vector3 Playerpos;

    public static int PlayerHP;
    public static int Poka;
    public static readonly int MaxPoka = 2;

    public static IList<Itemdata> DList;
    public static IList<AtkWData> WList;

    public static string Level;

    public static bool Falling = false;
    public static bool Falled = false;
    
    public static void Save() 
    {
        PlayerPrefs.SetInt("PlayerHP", PlayerHP);
        PlayerPrefs.SetInt("Poka", Poka);
        Level = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("level",Level);
        PlayerPrefs.Save();
    }
        

    public static void Load()
    {
        string json = PlayerPrefs.GetString("data");
        string json2 = PlayerPrefs.GetString("data2");
        DList = JsonConvert.DeserializeObject<IList<Itemdata>>(json);
        WList = JsonConvert.DeserializeObject<IList<AtkWData>>(json2);
        AudioReady = bool.Parse(PlayerPrefs.GetString("AudioReady", "false"));
        Playerpos.x = PlayerPrefs.GetFloat("x");
        Playerpos.y = PlayerPrefs.GetFloat("y");
    }

    public static void Respawn()  //2023/2/15:做經過就能存檔的不會回血的臨時存檔點(在碰到陷阱或掉落時啟用)
    {
        string json = PlayerPrefs.GetString("data");
        string json2 = PlayerPrefs.GetString("data2");
        DList = JsonConvert.DeserializeObject<IList<Itemdata>>(json);
        WList = JsonConvert.DeserializeObject<IList<AtkWData>>(json2);
        AudioReady = bool.Parse(PlayerPrefs.GetString("AudioReady", "false"));
        Playerpos.x = PlayerPrefs.GetFloat("x");
        Playerpos.y = PlayerPrefs.GetFloat("y");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public static void FallOut()
    {
        string json = PlayerPrefs.GetString("data");
        string json2 = PlayerPrefs.GetString("data2");
        DList = JsonConvert.DeserializeObject<IList<Itemdata>>(json);
        WList = JsonConvert.DeserializeObject<IList<AtkWData>>(json2);
        Playerpos.x = PlayerPrefs.GetFloat("Tempx");
        Playerpos.y = PlayerPrefs.GetFloat("Tempy");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void TempPoint()
    {
        string json = PlayerPrefs.GetString("data");
        string json2 = PlayerPrefs.GetString("data2");
        DList = JsonConvert.DeserializeObject<IList<Itemdata>>(json);
        WList = JsonConvert.DeserializeObject<IList<AtkWData>>(json2);
        Playerpos.x = PlayerPrefs.GetFloat("Tempx");
        Playerpos.y = PlayerPrefs.GetFloat("Tempy");
        Falling = false;
        Falled = false;
    }

    public static void OptionSave()
    {
        PlayerPrefs.SetString("AudioReady", AudioReady.ToString());
        PlayerPrefs.SetFloat("BGMV", BGMAudio.BGM_audioSource.volume);
        PlayerPrefs.SetFloat("SEV", SEAudio.SE_audioSource.volume);
    }
}
