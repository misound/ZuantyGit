using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public GameObject[] PokaCan;

    private CameraMgr _cameraMgr;
    

    private bool _isDirty = false;
    private void Awake()
    {
        _cameraMgr = FindObjectOfType<CameraMgr>();
    }

    private void Start()
    {

    }
    private void Update()
    {
        Debug.Log(GameSetting.Poka);
        
        if (_isDirty)
        {
            SetHealth(GameSetting.PlayerHP);
            _isDirty = false;
        }

        if (GameSetting.PlayerHP <= 0 && !GameSetting.Falled && !GameSetting.Falling)
        {

            if (_cameraMgr.Blackscreenalpha >= 1)
            {
                GameSetting.Respawn();
            }
        }
        else if (GameSetting.PlayerHP <= 0 && GameSetting.Falled && GameSetting.Falling)
        {
            if (_cameraMgr.Blackscreenalpha >= 1)
            {
                GameSetting.Respawn();
            }
        }
        else if (GameSetting.PlayerHP > 0 || GameSetting.Falled && GameSetting.Falling)
        {
            if (_cameraMgr.Blackscreenalpha >= 1)
            {
                GameSetting.FallOut();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            INeedHealing();
        }
    }
    
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        health = 100;
        GameSetting.PlayerHP = health;
        _isDirty = true;
        fill.color = gradient.Evaluate(1f);
        //PlayerPrefs.SetInt("PlayerHP", health);
    }

    public void SetHealth(int health)
    {

        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        GameSetting.PlayerHP = health;
        if(health > 0)
        {
            PlayerPrefs.SetInt("PlayerHP", GameSetting.PlayerHP);
            GameSetting.PlayerHP = health;
        }
        if(health <= 0)
        {
            health = 0;
            PlayerPrefs.SetInt("PlayerHP", GameSetting.PlayerHP);
            GameSetting.PlayerHP = health;
            health = GameSetting.PlayerHP;
        }
        
        _isDirty = true;
    }
    
    public void GetHealth(int health)
    {

        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        health = PlayerPrefs.GetInt("PlayerHP");
        GameSetting.PlayerHP = health;

        _isDirty = true;
    }

    public void INeedHealing()
    {
        if (PokaCan != null)
        {
            if (GameSetting.Poka >= 1)
            {
                SetMaxHealth(GameSetting.PlayerHP = 100);

                GameSetting.Poka -= 1;
                PokaCan[GameSetting.Poka].SetActive(false);
                PlayerPrefs.SetInt("Poka",GameSetting.Poka);
            }
            else if (GameSetting.Poka <= 0)
            {
                GameSetting.Poka = 0;
                PlayerPrefs.SetInt("Poka",GameSetting.Poka);
            }
        }
    }

    public void BuyPoka()
    {
        GameSetting.Poka = GameSetting.MaxPoka;
        for (int i = 0; i < PokaCan.Length; i++)
        {
            PokaCan[i].SetActive(true);
        }
    }

    public void GetPoka(int poka)
    {
        poka = PlayerPrefs.GetInt("Poka");
        GameSetting.Poka  = poka;
        for (int i = 0; i < GameSetting.Poka; i++)
        {
            PokaCan[i].SetActive(true);
        }
    }
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
        /*
        if (GUI.Button(new Rect(100, 240, 160, 100), "MaxHP"))
        {
            SetMaxHealth(GameSetting.PlayerHP = 100);
            _isDirty = true;
        }
        if (GUI.Button(new Rect(100, 960, 160, 100), "-HP"))
        {
            SetHealth(GameSetting.PlayerHP -= 60);
        }*/
    }
}
