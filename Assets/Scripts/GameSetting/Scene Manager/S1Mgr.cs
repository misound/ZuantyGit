using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class S1Mgr : MonoBehaviour
{
    public GameObject DoorPrefab;
    public GameObject AWPrefab;
    public GameObject[] CheckPoint;

    public GameObject[] DPos;
    public GameObject[] AWPos;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        ResetItem();
        PlayerPrefs.SetString("D1-1S", "false");
        PlayerPrefs.SetString("AW1-1S", "false");
        PlayerPrefs.SetString("AW1-2S", "false");
        PlayerPrefs.SetString("AW1-3S", "false");
        PlayerPrefs.SetString("AW1-4S", "false");
        PlayerPrefs.SetString("AW1-5S", "false");
        string json = PlayerPrefs.GetString("data");
        string json2 = PlayerPrefs.GetString("data2");
        GameSetting.DList = JsonConvert.DeserializeObject<IList<Itemdata>>(json);
        GameSetting.WList = JsonConvert.DeserializeObject<IList<AtkWData>>(json2);
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

        for (int i = 0; i < GameSetting.WList.Count; i++)
        {
            GameObject temp = Instantiate(AWPrefab, AWPos[i].transform);
            temp.transform.position = AWPos[i].transform.position;
            temp.transform.localScale = Vector3.one;
            temp.gameObject.name = GameSetting.WList[i].AWName;
            AtkWallHandler wall = temp.GetComponent<AtkWallHandler>();
            wall.SetWallData(GameSetting.WList[i]);
        }

        Debug.Log(GameSetting.WList.Count);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckPoints();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameSetting.Load();
            load();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            ResetItem();
        }
    }

    public IList<Itemdata> FakeData1()
    {
        IList<Itemdata> result = new List<Itemdata>();

        result.Add(new Itemdata() { Name = "D1-1", States = bool.Parse((PlayerPrefs.GetString("D1-1S"))) });
        return result;
    }

    public IList<AtkWData> FakeData2()
    {
        IList<AtkWData> result = new List<AtkWData>();

        result.Add(new AtkWData() { AWName = "AW1-1", AWStates = bool.Parse((PlayerPrefs.GetString("AW1-1S"))) });
        result.Add(new AtkWData() { AWName = "AW1-2", AWStates = bool.Parse((PlayerPrefs.GetString("AW1-2S"))) });
        result.Add(new AtkWData() { AWName = "AW1-3", AWStates = bool.Parse((PlayerPrefs.GetString("AW1-3S"))) });
        result.Add(new AtkWData() { AWName = "AW1-4", AWStates = bool.Parse((PlayerPrefs.GetString("AW1-4S"))) });
        result.Add(new AtkWData() { AWName = "AW1-5", AWStates = bool.Parse((PlayerPrefs.GetString("AW1-5S"))) });

        return result;
    }

    private void Save()
    {

        string json = JsonConvert.SerializeObject(GameSetting.DList);
        string json2 = JsonConvert.SerializeObject(GameSetting.WList);
        SpeedPlayerController SPC = GameObject.FindObjectOfType<SpeedPlayerController>();

        Vector3 pos = SPC.transform.position;
        GameSetting.Playerposx = pos.x;
        GameSetting.Playerposy = pos.y;
        PlayerPrefs.SetFloat("x", GameSetting.Playerposx);
        PlayerPrefs.SetFloat("y", GameSetting.Playerposy);
        PlayerPrefs.SetString("data", json);
        PlayerPrefs.SetString("data2", json2);
        PlayerPrefs.Save();
    }

    void CheckPoints()
    {
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
                    Save();
                    PlayerPrefs.Save();
                }
            }
            else if (hitR.collider != null)
            {
                if (hitR.collider.gameObject.CompareTag("Player"))
                {
                    Save();
                    PlayerPrefs.Save();
                }
            }
        }
    }

    private void load()
    {
        SpeedPlayerController SPC = GameObject.FindObjectOfType<SpeedPlayerController>();

        SPC.transform.position = GameSetting.Playerpos;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < CheckPoint.Length; i++)
        {
            Gizmos.DrawRay(CheckPoint[i].transform.position, Vector3.right * 2);
            Gizmos.DrawRay(CheckPoint[i].transform.position, Vector3.left * 2);
        }
    }

    private void ResetItem()
    {
        var data1 = FakeData1();
        var data2 = FakeData2();
        string json = JsonConvert.SerializeObject(data1);
        string json2 = JsonConvert.SerializeObject(data2);
        PlayerPrefs.SetString("data", json);
        PlayerPrefs.SetString("data2", json2);
    }
}