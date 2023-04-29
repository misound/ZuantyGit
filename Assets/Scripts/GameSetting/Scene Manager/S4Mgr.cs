using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S4Mgr : MonoBehaviour
{
    public GameObject DoorPrefab;
    public GameObject AWPrefab;
    public GameObject[] CheckPoint;

    public GameObject[] DPos;
    public GameObject[] AWPos;

    public HealthBar PlayerHP;

    private bool EnteredS4 = false;


    private void Awake()
    {
        //判斷是否為新遊戲
        
        
        PlayerHP = FindObjectOfType<HealthBar>();

        EnteredS4 = bool.Parse((PlayerPrefs.GetString("S4Enter")));
        PlayerPrefs.SetString("D4-1S", "false");
        PlayerPrefs.SetString("AW4-1S", "false");
        PlayerPrefs.SetString("AW4-2S", "false");
        PlayerPrefs.SetString("AW4-3S", "false");
        PlayerPrefs.SetString("AW4-4S", "false");
        PlayerPrefs.SetString("AW4-5S", "false");
        PlayerPrefs.SetString("AW4-6S", "false");
        if (EnteredS4)
        {
            GameSetting.DList = CakeData1();
            GameSetting.WList = CakeData2();
            string json = PlayerPrefs.GetString("data");
            string json2 = PlayerPrefs.GetString("data2");
            GameSetting.DList = JsonConvert.DeserializeObject<IList<Itemdata>>(json);
            GameSetting.WList = JsonConvert.DeserializeObject<IList<AtkWData>>(json2);
        }
        else if (!EnteredS4)
        {
            S4Item S4Item = (S4Item)Factory.reset("S4");
            GameSetting.DList = S4Item.FakeData1();
            GameSetting.WList = S4Item.FakeData2();
            
            GameSetting.Falling = false;
            GameSetting.Falled = false;
        }

        PlayerHP = FindObjectOfType<HealthBar>();
        PlayerHP.SetMaxHealth(GameSetting.PlayerHP = 100);
        if (GameSetting.PlayerHP <= 0)
        {
            PlayerHP.SetMaxHealth(GameSetting.PlayerHP = PlayerPrefs.GetInt("PlayerHP"));
        }
    }

    private void Start()
    {
        //生產可破壞物件
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



        FindObjectOfType<AudioMgr>().BGMCheck = true;
        FindObjectOfType<AudioMgr>().SECheck = true;
        
        
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckPoints();
        }
    }

    #region 第四關可破壞物件資料

    public IList<Itemdata> CakeData1()
    {
        IList<Itemdata> result = new List<Itemdata>();

        result.Add(new Itemdata() { Name = "D4-1", States = bool.Parse((PlayerPrefs.GetString("D4-1S"))) });
        return result;
    }

    public IList<AtkWData> CakeData2()
    {
        IList<AtkWData> result = new List<AtkWData>();

        result.Add(new AtkWData() { AWName = "AW4-1", AWStates = bool.Parse((PlayerPrefs.GetString("AW4-1S"))) });
        result.Add(new AtkWData() { AWName = "AW4-2", AWStates = bool.Parse((PlayerPrefs.GetString("AW4-2S"))) });
        result.Add(new AtkWData() { AWName = "AW4-3", AWStates = bool.Parse((PlayerPrefs.GetString("AW4-3S"))) });
        result.Add(new AtkWData() { AWName = "AW4-4", AWStates = bool.Parse((PlayerPrefs.GetString("AW4-4S"))) });
        result.Add(new AtkWData() { AWName = "AW4-5", AWStates = bool.Parse((PlayerPrefs.GetString("AW4-5S"))) });
        result.Add(new AtkWData() { AWName = "AW4-6", AWStates = bool.Parse((PlayerPrefs.GetString("AW4-6S"))) });

        return result;
    }

    #endregion

    #region 存檔管理

    public void Save()
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

    public void CheckPoints()
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
                    PlayerPrefs.SetString("S4Enter", "true");
                    PlayerPrefs.Save();
                }
            }
            else if (hitR.collider != null)
            {
                if (hitR.collider.gameObject.CompareTag("Player"))
                {
                    Save();
                    PlayerPrefs.SetString("S4Enter", "true");
                    PlayerPrefs.Save();
                }
            }
        }
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