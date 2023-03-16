using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class S3Mgr : MonoBehaviour
{
    public GameObject DoorPrefab;
    public GameObject AWPrefab;
    public GameObject[] CheckPoint;

    public GameObject[] DPos;
    public GameObject[] AWPos;

    private bool EnteredS3 = false;

    private void Awake()
    {   //判斷是否為新遊戲
        
        EnteredS3 = bool.Parse((PlayerPrefs.GetString("S3Enter")));
        PlayerPrefs.SetString("D3-1S", "false");
        PlayerPrefs.SetString("AW3-1S", "false");
        PlayerPrefs.SetString("AW3-2S", "false");
        PlayerPrefs.SetString("AW3-3S", "false");
        PlayerPrefs.SetString("AW3-4S", "false");
        PlayerPrefs.SetString("AW3-5S", "false");
        PlayerPrefs.SetString("AW3-6S", "false");
        PlayerPrefs.SetString("AW3-7S", "false");
        PlayerPrefs.SetString("AW3-8S", "false");
        PlayerPrefs.SetString("AW3-9S", "false");
        PlayerPrefs.SetString("AW3-10S", "false");
        if (EnteredS3)
        {
            GameSetting.DList = CakeData1();
            GameSetting.WList = CakeData2();
            string json = PlayerPrefs.GetString("data");
            string json2 = PlayerPrefs.GetString("data2");
            GameSetting.DList = JsonConvert.DeserializeObject<IList<Itemdata>>(json);
            GameSetting.WList = JsonConvert.DeserializeObject<IList<AtkWData>>(json2);
        }
        else if (!EnteredS3)
        {
            S3Item S3Item = (S3Item)Factory.reset("S3");
            GameSetting.DList = S3Item.FakeData1();
            GameSetting.WList = S3Item.FakeData2();
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
    }

    #region 第三關可破壞物件資料

        public IList<Itemdata> CakeData1()
        {
            IList<Itemdata> result = new List<Itemdata>();
    
            result.Add(new Itemdata() { Name = "D3-1", States = bool.Parse((PlayerPrefs.GetString("D3-1S"))) });
            return result;
        }
    
        public IList<AtkWData> CakeData2()
        {
            IList<AtkWData> result = new List<AtkWData>();
    
            result.Add(new AtkWData() { AWName = "AW3-1", AWStates = bool.Parse((PlayerPrefs.GetString("AW3-1S"))) });
            result.Add(new AtkWData() { AWName = "AW3-2", AWStates = bool.Parse((PlayerPrefs.GetString("AW3-2S"))) });
            result.Add(new AtkWData() { AWName = "AW3-3", AWStates = bool.Parse((PlayerPrefs.GetString("AW3-3S"))) });
            result.Add(new AtkWData() { AWName = "AW3-4", AWStates = bool.Parse((PlayerPrefs.GetString("AW3-4S"))) });
            result.Add(new AtkWData() { AWName = "AW3-5", AWStates = bool.Parse((PlayerPrefs.GetString("AW3-5S"))) });
            result.Add(new AtkWData() { AWName = "AW3-6", AWStates = bool.Parse((PlayerPrefs.GetString("AW3-6S"))) });
            result.Add(new AtkWData() { AWName = "AW3-7", AWStates = bool.Parse((PlayerPrefs.GetString("AW3-7S"))) });
            result.Add(new AtkWData() { AWName = "AW3-8", AWStates = bool.Parse((PlayerPrefs.GetString("AW3-8S"))) });
            result.Add(new AtkWData() { AWName = "AW3-9", AWStates = bool.Parse((PlayerPrefs.GetString("AW3-9S"))) });
            result.Add(new AtkWData() { AWName = "AW3-10", AWStates = bool.Parse((PlayerPrefs.GetString("AW3-10S"))) });
            
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
                        PlayerPrefs.SetString("S3Enter", "true");
                        PlayerPrefs.Save();
                    }
                }
                else if (hitR.collider != null)
                {
                    if (hitR.collider.gameObject.CompareTag("Player"))
                    {
                        Save();
                        PlayerPrefs.SetString("S3Enter", "true");
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
    private void OnDrawGizmos()
    {
        for (int i = 0; i < CheckPoint.Length; i++)
        {
            Gizmos.DrawRay(CheckPoint[i].transform.position, Vector3.right * 2);
            Gizmos.DrawRay(CheckPoint[i].transform.position, Vector3.left * 2);
        }
    }
}