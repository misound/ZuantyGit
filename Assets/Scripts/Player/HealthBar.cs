using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    #region 遊戲元件和物件
    [Header("Components & GameObj")]
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    [SerializeField] private GameObject[] PokaCan;
    [Header("Camera")]
    [SerializeField] private CameraMgr _cameraMgr;
    #endregion
    //畫布(通常是血量)更新
    private bool _isDirty = false;
    private void Awake()
    {
        _cameraMgr = FindObjectOfType<CameraMgr>();
    }
    private void Update()
    {
        //畫布(通常是血量)更新，更完就關掉自己
        if (_isDirty)
        {
            SetHealth(GameSetting.PlayerHP);
            _isDirty = false;
        }

        //補血
        if (Input.GetKeyDown(KeyCode.F))
        {
            INeedHealing();
        }
    }

    #region 攝影機連動(對我跟血條綁在一起)
    public void CameraAction()
    {
        //沒血非掉落傷害
        if (GameSetting.PlayerHP <= 0 && !GameSetting.Falled && !GameSetting.Falling)
        {
            if (_cameraMgr.Blackscreenalpha >= 1)
            {
                GameSetting.Respawn();
            }
        }
        //沒血且是掉落傷害
        else if (GameSetting.PlayerHP <= 0 && GameSetting.Falled && GameSetting.Falling)
        {
            if (_cameraMgr.Blackscreenalpha >= 1)
            {
                GameSetting.Respawn();
            }
        }
        //普通的掉落傷害
        else if (GameSetting.PlayerHP > 0 || GameSetting.Falled && GameSetting.Falling)
        {
            if (_cameraMgr.Blackscreenalpha >= 1)
            {
                GameSetting.FallOut();
            }
        }
    }

    /// <summary>
    /// 受傷的紅色特效
    /// </summary>
    /// <param name="Hp">玩家血量</param>
    public void CameraE(int Hp)
    {
        if (Hp < 99)
            _cameraMgr.CameraStatusSwitcher(CameraMgr.CameraStatus.Hurt);
    }
    #endregion
    #region 血條處理
    /// <summary>
    /// 玩家最大血量設置
    /// </summary>
    /// <param name="health">玩家最大血量</param>
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        health = 100;
        GameSetting.PlayerHP = health;
        _isDirty = true;
        fill.color = gradient.Evaluate(1f);
    }

    /// <summary>
    /// 玩家血量更動
    /// </summary>
    /// <param name="health">更新後的玩家目前血量</param>
    public void SetHealth(int health)
    {
        //血量條UI更新
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        //數值同步
        GameSetting.PlayerHP = health;
        //如果受傷後活著
        if (health > 0)
        {
            //紀錄血量數值，在重載或更換Scene時使用
            PlayerPrefs.SetInt("PlayerHP", GameSetting.PlayerHP);
            //數值同步
            GameSetting.PlayerHP = health;
        }
        //活不成
        if (health <= 0)
        {
            health = 0;
            //紀錄血量數值，在重載或更換Scene時使用
            PlayerPrefs.SetInt("PlayerHP", GameSetting.PlayerHP);
            //數值同步(但我那時還沒學到ref，懶得改了)
            GameSetting.PlayerHP = health;
            health = GameSetting.PlayerHP;
        }

        _isDirty = true;
    }

    /// <summary>
    /// 讀取紀錄的血量
    /// </summary>
    /// <param name="health">從PlayerPrefs讀取的血量</param>
    public void GetHealth(int health)
    {
        //血量條UI更新
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        //讀取血量
        health = PlayerPrefs.GetInt("PlayerHP");
        GameSetting.PlayerHP = health;

        _isDirty = true;
    }
    #endregion
    #region 恢復道具處理
    public void INeedHealing()
    {
        if (PokaCan != null)
        {
            if (GameSetting.Poka >= 1)
            {
                SetMaxHealth(GameSetting.PlayerHP = 100);

                GameSetting.Poka -= 1;
                _cameraMgr.CameraStatusSwitcher(CameraMgr.CameraStatus.FullHP);
                PokaCan[GameSetting.Poka].SetActive(false);
                PlayerPrefs.SetInt("Poka", GameSetting.Poka);
            }
            else if (GameSetting.Poka <= 0)
            {
                GameSetting.Poka = 0;
                PlayerPrefs.SetInt("Poka", GameSetting.Poka);
            }
        }
    }

    /// <summary>
    /// 從存檔點獲取回復道具的方法
    /// </summary>
    public void BuyPoka()
    {
        GameSetting.Poka = GameSetting.MaxPoka;
        for (int i = 0; i < PokaCan.Length; i++)
        {
            PokaCan[i].SetActive(true);
        }
    }

    /// <summary>
    /// 讀取回復道具數量
    /// </summary>
    /// <param name="poka">現有回復道具數量</param>
    public void GetPoka(ref int poka)
    {
        //讀取
        poka = PlayerPrefs.GetInt("Poka");
        //顯示數量
        for (int i = 0; i < GameSetting.Poka; i++)
        {
            PokaCan[i].SetActive(true);
        }
    }
    #endregion
    #region 金手指
    private void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            SetMaxHealth(GameSetting.PlayerHP = 100);
            _isDirty = true;
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            SetHealth(GameSetting.PlayerHP -= 20);

        }
    }
    #endregion
}
