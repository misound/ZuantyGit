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

    private CameraMgr _cameraMgr;
    public SpeedPlayerController speedPlayerController;


    private bool _isDirty = false;
    private void Awake()
    {
        _cameraMgr = FindObjectOfType<CameraMgr>();

    }

    private void Start()
    {
        if (GameSetting.PlayerHP <= 0) 
        { 
            SetMaxHealth(GameSetting.PlayerHP = 100);
        }
    }
    private void Update()
    {
        if (_isDirty)
        {
            SetHealth(GameSetting.PlayerHP);
            _isDirty = false;
        }

        if (GameSetting.PlayerHP <= 0)
        {
            SpeedPlayerController SPC = GameObject.FindObjectOfType<SpeedPlayerController>(); 
            if (_cameraMgr.Blackscreenalpha >= 1)
            {
                speedPlayerController.playerDead = true;
                SPC.transform.position = GameSetting.Playerpos; 
                GameSetting.Respawn();
            }
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
        if(health <= 0)
        {
            health = 0;
            
        }
        PlayerPrefs.SetInt("PlayerHP", health);
        _isDirty = true;
    }
    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 240, 160, 100), "MaxHP"))
        {
            SetMaxHealth(GameSetting.PlayerHP = 100);
            _isDirty = true;
        }
        if (GUI.Button(new Rect(100, 960, 160, 100), "-HP"))
        {
            SetHealth(GameSetting.PlayerHP -= 60);
        }
    }
}
