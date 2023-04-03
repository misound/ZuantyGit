using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class S2Mgr : MonoBehaviour
{
    public GameObject DoorPrefab;
    public GameObject AWPrefab;
    public GameObject[] CheckPoint;
    public GameObject[] RespawnPoint;

    public GameObject[] DPos;
    public GameObject[] AWPos;
    
    private bool EnteredS2 = false;
    
    public GameObject[] FallingLine;

    public int FallDmg = 30;

    public HealthBar PlayerHP;

    public float FallSec = 3.0f;

    private void Awake()
    {
        EnteredS2 = bool.Parse((PlayerPrefs.GetString("S2Enter")));
        PlayerPrefs.SetString("D2-1S", "false");
        PlayerPrefs.SetString("D2-2S", "false");
        PlayerPrefs.SetString("D2-3S", "false");
        PlayerPrefs.SetString("D2-4S", "false");
        PlayerPrefs.SetString("D2-5S", "false");
        PlayerPrefs.SetString("D2-6S", "false");
        PlayerPrefs.SetString("D2-7S", "false");
        PlayerPrefs.SetString("D2-8S", "false");
        PlayerPrefs.SetString("AW2-1S", "false");
        PlayerPrefs.SetString("AW2-2S", "false");
        PlayerPrefs.SetString("AW2-3S", "false");
        PlayerPrefs.SetString("AW2-4S", "false");
        if (EnteredS2)
        {
            GameSetting.DList = CakeData1();
            GameSetting.WList = CakeData2();
            string json = PlayerPrefs.GetString("data");
            string json2 = PlayerPrefs.GetString("data2");
            GameSetting.DList = JsonConvert.DeserializeObject<IList<Itemdata>>(json);
            GameSetting.WList = JsonConvert.DeserializeObject<IList<AtkWData>>(json2);
        }
        else if (!EnteredS2)
        {
            S2Item S2Item = (S2Item)Factory.reset("S2");
            GameSetting.DList = S2Item.FakeData1();
            GameSetting.WList = S2Item.FakeData2();
        }
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
        StartCoroutine(FallLine());
        TempPoint();
    }

    public IList<Itemdata> CakeData1()
    {
        IList<Itemdata> result = new List<Itemdata>();

        result.Add(new Itemdata() { Name = "D2-1", States = bool.Parse((PlayerPrefs.GetString("D2-1S"))) });
        result.Add(new Itemdata() { Name = "D2-2", States = bool.Parse((PlayerPrefs.GetString("D2-2S"))) });
        result.Add(new Itemdata() { Name = "D2-3", States = bool.Parse((PlayerPrefs.GetString("D2-3S"))) });
        result.Add(new Itemdata() { Name = "D2-4", States = bool.Parse((PlayerPrefs.GetString("D2-4S"))) });
        result.Add(new Itemdata() { Name = "D2-5", States = bool.Parse((PlayerPrefs.GetString("D2-5S"))) });
        result.Add(new Itemdata() { Name = "D2-6", States = bool.Parse((PlayerPrefs.GetString("D2-6S"))) });
        result.Add(new Itemdata() { Name = "D2-7", States = bool.Parse((PlayerPrefs.GetString("D2-7S"))) });
        result.Add(new Itemdata() { Name = "D2-8", States = bool.Parse((PlayerPrefs.GetString("D2-8S"))) });

        return result;
    }

    public IList<AtkWData> CakeData2()
    {
        IList<AtkWData> result = new List<AtkWData>();

        result.Add(new AtkWData() { AWName = "AW2-1", AWStates = bool.Parse((PlayerPrefs.GetString("AW2-1S"))) });
        result.Add(new AtkWData() { AWName = "AW2-2", AWStates = bool.Parse((PlayerPrefs.GetString("AW2-2S"))) });
        result.Add(new AtkWData() { AWName = "AW2-3", AWStates = bool.Parse((PlayerPrefs.GetString("AW2-3S"))) });
        result.Add(new AtkWData() { AWName = "AW2-4", AWStates = bool.Parse((PlayerPrefs.GetString("AW2-4S"))) });

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
                    PlayerPrefs.SetString("S2Enter", "true");
                    PlayerPrefs.Save();
                }
            }
            else if (hitR.collider != null)
            {
                if (hitR.collider.gameObject.CompareTag("Player"))
                {
                    Save();
                    PlayerPrefs.SetString("S2Enter", "true");
                    PlayerPrefs.Save();
                }
            }
        }
    }
    
    void TempPoint()
    {
        SpeedPlayerController SPC = GameObject.FindObjectOfType<SpeedPlayerController>();

        for (int i = 0; i < RespawnPoint.Length; i++)
        {
            RaycastHit2D hitR = Physics2D.Raycast(RespawnPoint[i].transform.position, Vector3.right * 1, 2,
                1 << LayerMask.NameToLayer("Default"));
            RaycastHit2D hit = Physics2D.Raycast(RespawnPoint[i].transform.position, Vector3.left * 1, 2,
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

    private void load()
    {
        SpeedPlayerController SPC = GameObject.FindObjectOfType<SpeedPlayerController>();

        SPC.transform.position = GameSetting.Playerpos;
    }

    IEnumerator FallLine()
    {
        SpeedPlayerController SPC = GameObject.FindObjectOfType<SpeedPlayerController>();

        for (int i = 0; i < FallingLine.Length; i++)
        {
            RaycastHit2D hitR = Physics2D.Raycast(FallingLine[i].transform.position, Vector3.right * 10, 10,
                1 << LayerMask.NameToLayer("Default"));
            RaycastHit2D hit = Physics2D.Raycast(FallingLine[i].transform.position, Vector3.left * 10, 10,
                1 << LayerMask.NameToLayer("Default"));


            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    SPC.transform.position = new Vector3
                    (FallingLine[i].transform.position.x,
                        FallingLine[i].transform.position.y - 10);
                    yield return new WaitForSeconds(FallSec);
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
                    SPC.transform.position = new Vector3
                    (FallingLine[i].transform.position.x,
                        FallingLine[i].transform.position.y - 10);

                    yield return new WaitForSeconds(FallSec);
                    GameSetting.FallOut();
                    GameSetting.PlayerHP -= FallDmg;
                    PlayerHP.SetHealth(GameSetting.PlayerHP);
                    SPC.transform.position = GameSetting.Playerpos;
                }
            }

            yield return null;
        }
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
        
        for (int i = 0; i < FallingLine.Length; i++)
        {
            Gizmos.DrawRay(FallingLine[i].transform.position, Vector3.left * 10);
            Gizmos.DrawRay(FallingLine[i].transform.position, Vector3.right * 10);
        }
    }
}
