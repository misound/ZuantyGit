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
    

    private bool _isDirty = false;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        health = 100;
        GameSetting.PlayerHP = health;
        _isDirty = true;
        fill.color = gradient.Evaluate(1f);
        PlayerPrefs.SetInt("PlayerHP", health);
    }

    public void SetHealth(int health)
    {
        SpeedPlayerController SPC = GameObject.FindObjectOfType<SpeedPlayerController>();
        
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        if(health <= 0)
        {
            health = 0;
            SPC.transform.position = GameSetting.Playerpos;
            GameSetting.Respawn();
        }

        _isDirty = true;
    }
    private void Start()
    {
        SetMaxHealth(GameSetting.PlayerHP = 100);
    }
    private void Update()
    {
        if (_isDirty)
        {
            SetHealth(GameSetting.PlayerHP);
            _isDirty = false;
        }
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
