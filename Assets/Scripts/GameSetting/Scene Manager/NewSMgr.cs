using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class NewSMgr : MonoBehaviour
{
    public GameObject DoorPrefab;
    public GameObject[] CheckPoint;

    public GameObject[] DPos;


    // Start is called before the first frame update
    void Awake()
    {
        PlayerPrefs.SetString("DoorT", "false");
        PlayerPrefs.SetString("DoorT01", "true");
        string json = PlayerPrefs.GetString("data");
        GameSetting.DList = JsonConvert.DeserializeObject<IList<Itemdata>>(json);
    }

    private void Start()
    {
        for (int i = 0; i < GameSetting.DList.Count; i++)
        {
            GameObject temp = Instantiate(DoorPrefab, DPos[i].transform);
            temp.transform.position = DPos[i].transform.position;
            temp.transform.localScale = Vector3.one;
            temp.gameObject.name = GameSetting.DList[i].Name;
            CanAtkDoor door = temp.GetComponent<CanAtkDoor>();
            door.SetDoorData(GameSetting.DList[i]);
        }

        Debug.Log(GameSetting.DList.Count);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckPoints();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            GameSetting.Load();
            SpeedPlayerController SPC = FindObjectOfType<SpeedPlayerController>();
            SPC.transform.position = GameSetting.Playerpos;
        }
    }

    public IList<Itemdata> FakeData1()
    {
        IList<Itemdata> result = new List<Itemdata>();

        result.Add(new Itemdata() { Name = "D1-1", States = bool.Parse((PlayerPrefs.GetString("DoorT01"))) });
        result.Add(new Itemdata() { Name = "D1-2", States = bool.Parse((PlayerPrefs.GetString("DoorT"))) });

        return result;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(1820, 160, 80, 50), "data1"))
        {
            Btn1();
        }
    }

    private void Btn1()
    {
        var data1 = FakeData1();
        string json = JsonConvert.SerializeObject(data1);
        SpeedPlayerController SPC = GameObject.FindObjectOfType<SpeedPlayerController>();

        Vector3 pos = SPC.transform.position;
        GameSetting.Playerposx = pos.x;
        GameSetting.Playerposy = pos.y;
        PlayerPrefs.SetFloat("x", GameSetting.Playerposx);
        PlayerPrefs.SetFloat("y", GameSetting.Playerposy);
        PlayerPrefs.SetString("data", json);
        PlayerPrefs.Save();
    }

    void CheckPoints()
    {
        SpeedPlayerController SPC = GameObject.FindObjectOfType<SpeedPlayerController>();

        for (int i = 0; i < CheckPoint.Length; i++)
        {
            RaycastHit2D hitR = Physics2D.Raycast(CheckPoint[i].transform.position, Vector3.right * 2, 2,
                1 << LayerMask.NameToLayer("Default"));
            RaycastHit2D hit = Physics2D.Raycast(CheckPoint[i].transform.position, Vector3.left * 2, 2,
                1 << LayerMask.NameToLayer("Default"));
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    Vector3 pos = SPC.transform.position;
                    GameSetting.Playerposx = pos.x;
                    GameSetting.Playerposy = pos.y;
                    PlayerPrefs.SetFloat("x", GameSetting.Playerposx);
                    PlayerPrefs.SetFloat("y", GameSetting.Playerposy);
                    PlayerPrefs.Save();
                }
            }
            else if (hitR.collider != null)
            {
                if (hitR.collider.gameObject.CompareTag("Player"))
                {
                    Vector3 pos = SPC.transform.position;
                    GameSetting.Playerposx = pos.x;
                    GameSetting.Playerposy = pos.y;
                    PlayerPrefs.SetFloat("x", GameSetting.Playerposx);
                    PlayerPrefs.SetFloat("y", GameSetting.Playerposy);
                    PlayerPrefs.Save();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < CheckPoint.Length; i++)
        {
            Gizmos.DrawRay(CheckPoint[i].transform.position, Vector3.right * 2);
            Gizmos.DrawRay(CheckPoint[i].transform.position, Vector3.left * 2);
        }
    }
}