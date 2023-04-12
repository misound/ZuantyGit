 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanAtkDoor : MonoBehaviour
{
    public GameObject[] DoorBody;
    public int DoorHP;
    public SpeedPlayerController playerController;
    float timer = 0f;
    float distimer = 3f;
    bool DisOn = false;

    public Collider2D My_col;

    public bool Open;

    private bool _isDirty = false;

    public string DName;

    public bool Opened = false;

    private Itemdata _data;

    // Start is called before the first frame update
    void Awake()
    {
        playerController = FindObjectOfType<SpeedPlayerController>();
        StartDoorState(DoorHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (DisOn)
        {
            DisppTimer();
        }

        if (Open)
        {
            SetDoorRig();
        }

        if (Opened)
        {
            DoorBody[0].SetActive(false);
            DoorBody[1].SetActive(false);
            DoorBody[2].SetActive(false);
            DoorBody[3].SetActive(false);
        }

        if (_isDirty)
        {
            Opened = _data.States;
            DName = _data.Name;
            _isDirty = false;
        }

    }


    #region 狀態處理

    public void TakeDoorHP(int dmg)
    {
        DoorHP -= dmg;
        if (DoorHP <= 0)
        {
            GameSetting.SEAudio.Play(AudioMgr.eAudio.SE_Atk_Door_Broken);
            DoorHP = 0;
            Open = true;
        }
    }

    public void StartDoorState(int HP)
    {
        DoorBody[0].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        DoorBody[1].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        DoorBody[2].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        DoorHP = HP;
        HP = 100;
    }

    public void SetDoorRig()
    {
        DoorBody[0].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        DoorBody[1].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        DoorBody[2].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        DoorBody[3].SetActive(false);
        My_col.enabled = false;
        DisOn = true;
    }

    public void DisppTimer()
    {
        timer += Time.deltaTime;
        if (timer >= distimer)
        {
            DoorBody[0].SetActive(false);
            DoorBody[1].SetActive(false);
            DoorBody[2].SetActive(false);
        }
    }

    #endregion

    #region 資料更新

    public void SetDoorName(string Name)
    {
        DName = Name;
        _isDirty = true;
    }

    public void SetDoorStates(bool States)
    {
        Opened = States;
        _isDirty = true;
    }

    public void SetDoorData(Itemdata data)
    {
        _data = data;
        _isDirty = true;
    }

    #endregion
}