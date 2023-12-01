using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    #region �C������M����
    [Header("Components & GameObj")]
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    [SerializeField] private GameObject[] PokaCan;
    [Header("Camera")]
    [SerializeField] private CameraMgr _cameraMgr;
    #endregion
    //�e��(�q�`�O��q)��s
    private bool _isDirty = false;
    private void Awake()
    {
        _cameraMgr = FindObjectOfType<CameraMgr>();
    }
    private void Update()
    {
        //�e��(�q�`�O��q)��s�A�󧹴N�����ۤv
        if (_isDirty)
        {
            SetHealth(GameSetting.PlayerHP);
            _isDirty = false;
        }

        //�ɦ�
        if (Input.GetKeyDown(KeyCode.F))
        {
            INeedHealing();
        }
    }

    #region ��v���s��(��ڸ����j�b�@�_)
    public void CameraAction()
    {
        //�S��D�����ˮ`
        if (GameSetting.PlayerHP <= 0 && !GameSetting.Falled && !GameSetting.Falling)
        {
            if (_cameraMgr.Blackscreenalpha >= 1)
            {
                GameSetting.Respawn();
            }
        }
        //�S��B�O�����ˮ`
        else if (GameSetting.PlayerHP <= 0 && GameSetting.Falled && GameSetting.Falling)
        {
            if (_cameraMgr.Blackscreenalpha >= 1)
            {
                GameSetting.Respawn();
            }
        }
        //���q�������ˮ`
        else if (GameSetting.PlayerHP > 0 || GameSetting.Falled && GameSetting.Falling)
        {
            if (_cameraMgr.Blackscreenalpha >= 1)
            {
                GameSetting.FallOut();
            }
        }
    }

    /// <summary>
    /// ���˪�����S��
    /// </summary>
    /// <param name="Hp">���a��q</param>
    public void CameraE(int Hp)
    {
        if (Hp < 99)
            _cameraMgr.CameraStatusSwitcher(CameraMgr.CameraStatus.Hurt);
    }
    #endregion
    #region ����B�z
    /// <summary>
    /// ���a�̤j��q�]�m
    /// </summary>
    /// <param name="health">���a�̤j��q</param>
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
    /// ���a��q���
    /// </summary>
    /// <param name="health">��s�᪺���a�ثe��q</param>
    public void SetHealth(int health)
    {
        //��q��UI��s
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        //�ƭȦP�B
        GameSetting.PlayerHP = health;
        //�p�G���˫ᬡ��
        if (health > 0)
        {
            //������q�ƭȡA�b�����Χ�Scene�ɨϥ�
            PlayerPrefs.SetInt("PlayerHP", GameSetting.PlayerHP);
            //�ƭȦP�B
            GameSetting.PlayerHP = health;
        }
        //������
        if (health <= 0)
        {
            health = 0;
            //������q�ƭȡA�b�����Χ�Scene�ɨϥ�
            PlayerPrefs.SetInt("PlayerHP", GameSetting.PlayerHP);
            //�ƭȦP�B(���ڨ����٨S�Ǩ�ref�A�i�o��F)
            GameSetting.PlayerHP = health;
            health = GameSetting.PlayerHP;
        }

        _isDirty = true;
    }

    /// <summary>
    /// Ū����������q
    /// </summary>
    /// <param name="health">�qPlayerPrefsŪ������q</param>
    public void GetHealth(int health)
    {
        //��q��UI��s
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        //Ū����q
        health = PlayerPrefs.GetInt("PlayerHP");
        GameSetting.PlayerHP = health;

        _isDirty = true;
    }
    #endregion
    #region ��_�D��B�z
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
    /// �q�s���I����^�_�D�㪺��k
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
    /// Ū���^�_�D��ƶq
    /// </summary>
    /// <param name="poka">�{���^�_�D��ƶq</param>
    public void GetPoka(ref int poka)
    {
        //Ū��
        poka = PlayerPrefs.GetInt("Poka");
        //��ܼƶq
        for (int i = 0; i < GameSetting.Poka; i++)
        {
            PokaCan[i].SetActive(true);
        }
    }
    #endregion
    #region �����
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
