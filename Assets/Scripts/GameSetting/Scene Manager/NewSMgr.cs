using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.SceneManagement;

public class NewSMgr : MonoBehaviour
{
    private bool Entered = false;

    public GameObject DoorPrefab;
    public GameObject AWPrefab;
    public GameObject[] CheckPoint;
    public GameObject[] RespawnPoint;

    public GameObject[] DPos;
    public GameObject[] AWPos;

    public GameObject Line;

    public int FallDmg = 30;

    public HealthBar PlayerHP;

    // Start is called before the first frame update
    void Awake()
    {
        NewSItem Item = (NewSItem)Factory.reset("NS");
        GameSetting.DList = Item.FakeData1();
        GameSetting.WList = Item.FakeData2();
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

        PlayerHP = FindObjectOfType<HealthBar>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckPoints();
        }

        TempPoint();

        StartCoroutine(FallLine());

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameSetting.Load();
            SpeedPlayerController SPC = FindObjectOfType<SpeedPlayerController>();
            SPC.transform.position = GameSetting.Playerpos;
        }

        Debug.Log(GameSetting.Level);
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
        string json = JsonConvert.SerializeObject(GameSetting.DList);
        string json2 = JsonConvert.SerializeObject(GameSetting.WList);
        PlayerPrefs.SetString("data", json);
        PlayerPrefs.SetString("data2", json2);
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
                    string json = JsonConvert.SerializeObject(GameSetting.DList);
                    string json2 = JsonConvert.SerializeObject(GameSetting.WList);
                    GameSetting.Save();
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
                    string json = JsonConvert.SerializeObject(GameSetting.DList);
                    string json2 = JsonConvert.SerializeObject(GameSetting.WList);
                    GameSetting.Save();
                }
            }
        }
    }

    void TempPoint()
    {
        SpeedPlayerController SPC = GameObject.FindObjectOfType<SpeedPlayerController>();

        for (int i = 0; i < RespawnPoint.Length; i++)
        {
            RaycastHit2D hitR = Physics2D.Raycast(RespawnPoint[i].transform.position, Vector3.right * 2, 2,
                1 << LayerMask.NameToLayer("Default"));
            RaycastHit2D hit = Physics2D.Raycast(RespawnPoint[i].transform.position, Vector3.left * 2, 2,
                1 << LayerMask.NameToLayer("Default"));
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    GameSetting.Playerpos = SPC.transform.position;
                    GameSetting.Playerposx = GameSetting.Playerpos.x;
                    GameSetting.Playerposy = GameSetting.Playerpos.y;
                    PlayerPrefs.SetFloat("Tempx", GameSetting.Playerposx);
                    PlayerPrefs.SetFloat("Tempy", GameSetting.Playerposy);
                    string json = JsonConvert.SerializeObject(GameSetting.DList);
                    string json2 = JsonConvert.SerializeObject(GameSetting.WList);
                    GameSetting.Save();
                    PlayerPrefs.Save();
                }
            }

            if (hitR.collider != null)
            {
                if (hitR.collider.gameObject.CompareTag("Player"))
                {
                    Vector3 pos = SPC.transform.position;
                    GameSetting.Playerposx = pos.x;
                    GameSetting.Playerposy = pos.y;
                    PlayerPrefs.SetFloat("Tempx", GameSetting.Playerposx);
                    PlayerPrefs.SetFloat("Tempy", GameSetting.Playerposy);
                    string json = JsonConvert.SerializeObject(GameSetting.DList);
                    string json2 = JsonConvert.SerializeObject(GameSetting.WList);
                    GameSetting.Save();
                    PlayerPrefs.Save();
                }
            }
        }
    }


    IEnumerator FallLine()
    {
        SpeedPlayerController SPC = GameObject.FindObjectOfType<SpeedPlayerController>();

        RaycastHit2D hitR = Physics2D.Raycast(Line.transform.position, Vector3.right * 20000, 20000,
            1 << LayerMask.NameToLayer("Default"));
        RaycastHit2D hit = Physics2D.Raycast(Line.transform.position, Vector3.left * 20000, 20000,
            1 << LayerMask.NameToLayer("Default"));


        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                SPC.transform.position = new Vector3(Line.transform.position.x, Line.transform.position.y - 10);
                yield return new WaitForSeconds(3f);
                GameSetting.FallOut();
                GameSetting.PlayerHP -= FallDmg;
                PlayerHP.SetHealth(GameSetting.PlayerHP);
                SPC.transform.position = GameSetting.Playerpos;
            }
        }

        if (hitR.collider != null)
        {
            if (hitR.collider.gameObject.CompareTag("Player"))
            {
                SPC.transform.position = new Vector3(Line.transform.position.x, Line.transform.position.y - 10);
                yield return new WaitForSeconds(3f);
                GameSetting.FallOut();
                GameSetting.PlayerHP -= FallDmg;
                PlayerHP.SetHealth(GameSetting.PlayerHP);
                SPC.transform.position = GameSetting.Playerpos;
            }
        }

        yield return null;
    }

    public IList<Itemdata> CakeData1()
    {
        IList<Itemdata> result = new List<Itemdata>();

        result.Add(new Itemdata() { Name = "D1-1", States = bool.Parse((PlayerPrefs.GetString("DN-1S"))) });
        result.Add(new Itemdata() { Name = "D1-2", States = bool.Parse((PlayerPrefs.GetString("DN-1S"))) });
        return result;
    }

    public IList<AtkWData> CakeData2()
    {
        IList<AtkWData> result = new List<AtkWData>();

        result.Add(new AtkWData() { AWName = "AW1-1", AWStates = bool.Parse((PlayerPrefs.GetString("AWN-1S"))) });
        result.Add(new AtkWData() { AWName = "AW1-2", AWStates = bool.Parse((PlayerPrefs.GetString("AWN-1S"))) });

        return result;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < CheckPoint.Length; i++)
        {
            Gizmos.DrawRay(CheckPoint[i].transform.position, Vector3.right * 2);
            Gizmos.DrawRay(CheckPoint[i].transform.position, Vector3.left * 2);
        }

        for (int i = 0; i < RespawnPoint.Length; i++)
        {
            Gizmos.DrawRay(RespawnPoint[i].transform.position, Vector3.right * 1);
            Gizmos.DrawRay(RespawnPoint[i].transform.position, Vector3.left * 1);
        }

        Gizmos.DrawRay(Line.transform.position, Vector3.left * 20000);
        Gizmos.DrawRay(Line.transform.position, Vector3.right * 20000);
    }
}