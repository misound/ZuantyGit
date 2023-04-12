using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AtkWallHandler : MonoBehaviour
{
    [Header("Data")] [SerializeField] private int AWHP;

    public GameObject WallBody;
    public Collider2D My_Col;
    public SpeedPlayerController playerController;

    public bool AWBreak = false;

    public bool AWBroken;

    public string AWName;

    public GameObject Particle;

    private bool _isDirty = false;

    private AtkWData _data;


    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<SpeedPlayerController>();
        if (Particle.GetComponent<ParticleSystem>().isPlaying == true)
        {
            Particle.GetComponent<ParticleSystem>().Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AWBreak)
        { 
            Particle.GetComponent<ParticleSystem>().Play();
            WallBody.SetActive(false);
            My_Col.enabled = false;
        }

        if (AWBroken)
        {
            WallBody.SetActive(false);
            Particle.GetComponent<ParticleSystem>().Stop();
        }

        if (_isDirty)
        {
            AWBroken = _data.AWStates;
            AWName = _data.AWName;
            _isDirty = false;
        }
    }

    public void StartAtkWState(int HP)
    {
        WallBody.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        AWHP = HP;
        HP = 100;
    }

    public void TakeAtkWHP(int dmg)
    {
        AWHP -= dmg;
        if (AWHP <= 0)
        {
            GameSetting.SEAudio.Play(AudioMgr.eAudio.SE_Atk_Wall_Broken);
            AWHP = 0;
            AWBreak = true;
            _data.AWStates = true;
            _isDirty = true;
        }
    }

    #region 資料更新

    public void SetDoorName(string Name)
    {
        AWName = Name;
        _isDirty = true;
    }

    public void SetDoorStates(bool States)
    {
        AWBroken = States;
        _isDirty = true;
    }

    public void SetWallData(AtkWData data)
    {
        _data = data;
        _isDirty = true;
    }

    #endregion
}