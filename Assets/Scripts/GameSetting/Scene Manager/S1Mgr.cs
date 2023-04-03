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
    public GameObject[] RespawnPoint;

    public GameObject[] DPos;
    public GameObject[] AWPos;
    
    
    public GameObject[] FallingLine;

    public int FallDmg = 30;
    
    public float FallSec = 3.0f;
    
    public HealthBar PlayerHP;

    private bool EnteredS1 = false;

    private void Awake()
    {   //判斷是否為新遊戲
        
        EnteredS1 = bool.Parse((PlayerPrefs.GetString("S1Enter")));
        PlayerPrefs.SetString("D1-1S", "false");
        PlayerPrefs.SetString("AW1-1S", "false");
        PlayerPrefs.SetString("AW1-2S", "false");
        PlayerPrefs.SetString("AW1-3S", "false");
        PlayerPrefs.SetString("AW1-4S", "false");
        PlayerPrefs.SetString("AW1-5S", "false");
        if (EnteredS1)
        {
            GameSetting.DList = CakeData1();
            GameSetting.WList = CakeData2();
            string json = PlayerPrefs.GetString("data");
            string json2 = PlayerPrefs.GetString("data2");
            GameSetting.DList = JsonConvert.DeserializeObject<IList<Itemdata>>(json);
            GameSetting.WList = JsonConvert.DeserializeObject<IList<AtkWData>>(json2);
        }
        else if (!EnteredS1)
        {
            S1Item S1Item = (S1Item)Factory.reset("S1");
            GameSetting.DList = S1Item.FakeData1();
            GameSetting.WList = S1Item.FakeData2();
        }

    }

    private void Start()
    {   //生產可破壞物件
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

    #region 第一關可破壞物件資料

        public IList<Itemdata> CakeData1()
        {
            IList<Itemdata> result = new List<Itemdata>();
    
            result.Add(new Itemdata() { Name = "D1-1", States = bool.Parse((PlayerPrefs.GetString("D1-1S"))) });
            return result;
        }
    
        public IList<AtkWData> CakeData2()
        {
            IList<AtkWData> result = new List<AtkWData>();
    
            result.Add(new AtkWData() { AWName = "AW1-1", AWStates = bool.Parse((PlayerPrefs.GetString("AW1-1S"))) });
            result.Add(new AtkWData() { AWName = "AW1-2", AWStates = bool.Parse((PlayerPrefs.GetString("AW1-2S"))) });
            result.Add(new AtkWData() { AWName = "AW1-3", AWStates = bool.Parse((PlayerPrefs.GetString("AW1-3S"))) });
            result.Add(new AtkWData() { AWName = "AW1-4", AWStates = bool.Parse((PlayerPrefs.GetString("AW1-4S"))) });
            result.Add(new AtkWData() { AWName = "AW1-5", AWStates = bool.Parse((PlayerPrefs.GetString("AW1-5S"))) });
    
            return result;
        }

    #endregion
    #region 存檔管理
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
                        PlayerPrefs.SetString("S1Enter", "true");
                        PlayerPrefs.Save();
                    }
                }
                else if (hitR.collider != null)
                {
                    if (hitR.collider.gameObject.CompareTag("Player"))
                    {
                        Save();
                        PlayerPrefs.SetString("S1Enter", "true");
                        PlayerPrefs.Save();
                    }
                }
            }
        }

    #endregion
    #region 讀檔(但只讀位置)

        private void load()
        {
            SpeedPlayerController SPC = GameObject.FindObjectOfType<SpeedPlayerController>();
    
            SPC.transform.position = GameSetting.Playerpos;
        }

    #endregion
    
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