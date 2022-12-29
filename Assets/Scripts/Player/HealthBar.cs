using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    private int dmgg = 20;


    private bool isDirty = false;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        if(health <= 0)
        {
            health = 0;
            //Debug.Log("�A���F");
        }
    }
    private void Start()
    {
        SetMaxHealth(GameSetting.PlayerHP = 100);
    }
    private void Update()
    {
        if (isDirty)
        {
            SetHealth(GameSetting.PlayerHP);
            isDirty = false;
        }
    }
    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 80, 160, 100), "SAVE"))
        {
            GameSetting.Save();
        }
        if (GUI.Button(new Rect(100, 160, 160, 100), "Load"))
        {
            GameSetting.Load();
            isDirty = true;
        }
        if (GUI.Button(new Rect(100, 240, 160, 100), "MaxHP"))
        {
            SetMaxHealth(GameSetting.PlayerHP = 100);
        }
        if (GUI.Button(new Rect(100, 320, 160, 100), "-HP"))
        {
            GameSetting.PlayerHP -= dmgg;
            SetHealth(GameSetting.PlayerHP);
        }

    }
}
