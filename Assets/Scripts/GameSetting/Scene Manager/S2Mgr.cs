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
    public SpeedPlayerController SPC;

    public float FallSec = 3.0f;

    private void Awake()
    {
        GameSetting.AudioReady = true;
        
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
        
        PlayerHP = FindObjectOfType<HealthBar>();
        SPC = FindObjectOfType<SpeedPlayerController>();
        
        if (EnteredS2)
        {
            GameSetting.DList = CakeData1();
            GameSetting.WList = CakeData2();
            string json = PlayerPrefs.GetString("data");
            string json2 = PlayerPrefs.GetString("data2");
            GameSetting.DList = JsonConvert.DeserializeObject<IList<Itemdata>>(json);
            GameSetting.WList = JsonConvert.DeserializeObject<IList<AtkWData>>(json2);

            
            if (GameSetting.Falling)
            {
                GameSetting.TempPoint();
            }
            else
            {
                GameSetting.Load();
            }
            
            //回檔測試，但未處理其他場景的互動
            if (GameSetting.PlayerHP <= 0) 
            {
                PlayerHP.SetMaxHealth(GameSetting.PlayerHP = 100); //最高生命值
                PlayerHP.SetHealth(GameSetting.PlayerHP); //刷新當前血量
            }
            else if(GameSetting.PlayerHP > 0) 
            {
                PlayerHP.SetMaxHealth(GameSetting.PlayerHP = 100); //最高生命值
                PlayerHP.GetHealth(GameSetting.PlayerHP = PlayerPrefs.GetInt("PlayerHP")); //讀取血量
                PlayerHP.SetHealth(GameSetting.PlayerHP); //刷新當前血量
                PlayerHP.GetPoka(GameSetting.Poka);
            }
            SPC.transform.position = GameSetting.Playerpos;
        }
        else if (!EnteredS2)
        {
            S2Item S2Item = (S2Item)Factory.reset("S2");
            GameSetting.DList = S2Item.FakeData1();
            GameSetting.WList = S2Item.FakeData2();
            
            PlayerHP.SetMaxHealth(GameSetting.PlayerHP = 100); //最高生命值
            PlayerHP.GetHealth(GameSetting.PlayerHP = PlayerPrefs.GetInt("PlayerHP")); //讀取血量
            PlayerHP.SetHealth(GameSetting.PlayerHP); //刷新當前血量
            PlayerHP.GetPoka(GameSetting.Poka);
            
            if (GameSetting.PlayerHP <= 0) 
            {
                PlayerHP.SetMaxHealth(GameSetting.PlayerHP = 100); //最高生命值
                PlayerHP.SetHealth(GameSetting.PlayerHP); //刷新當前血量
            }
            
            GameSetting.Falling = false;
            GameSetting.Falled = false;
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

        FindObjectOfType<AudioMgr>().BGMCheck = true; //關卡間傳遞
        FindObjectOfType<AudioMgr>().SECheck = true;
        
        Cursor.visible = false;
    }

    void Update()
    {
        StartCoroutine(FallLine());
    }

    #region 第二關可破壞物件資料

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

    public void TempPoint()
    {

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

    #endregion

    #region 掉落處理

    IEnumerator FallLine()
    {

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
                    GameSetting.Falling = true;
                    GameSetting.PlayerHP -= FallDmg;
                    PlayerHP.SetHealth(GameSetting.PlayerHP);
                    yield return new WaitForSeconds(FallSec);
                    GameSetting.Falled = true;
                }
            }

            if (hitR.collider != null)
            {
                if (hitR.collider.gameObject.CompareTag("Player"))
                {
                    SPC.transform.position = new Vector3
                    (FallingLine[i].transform.position.x,
                        FallingLine[i].transform.position.y - 10);
                    GameSetting.Falling = true;
                    GameSetting.PlayerHP -= FallDmg;
                    PlayerHP.SetHealth(GameSetting.PlayerHP);
                    yield return new WaitForSeconds(FallSec);
                    GameSetting.Falled = true;
                }
            }

            yield return null;
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
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<SpeedPlayerController>() != null)
        {
            GameSetting.Playerpos = SPC.transform.position;
            GameSetting.Playerposx = GameSetting.Playerpos.x;
            GameSetting.Playerposy = GameSetting.Playerpos.y;
            PlayerPrefs.SetFloat("Tempx", GameSetting.Playerposx);
            PlayerPrefs.SetFloat("Tempy", GameSetting.Playerposy);
            Debug.Log("AutoSaved!!!");
            GameSetting.Save();
            PlayerPrefs.Save();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<SpeedPlayerController>() != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameSetting.Playerpos = SPC.transform.position;
                GameSetting.Playerposx = GameSetting.Playerpos.x;
                GameSetting.Playerposy = GameSetting.Playerpos.y;
                PlayerPrefs.SetFloat("x", GameSetting.Playerposx);
                PlayerPrefs.SetFloat("y", GameSetting.Playerposy);
                string json = JsonConvert.SerializeObject(GameSetting.DList);
                string json2 = JsonConvert.SerializeObject(GameSetting.WList);
                PlayerPrefs.SetString("data", json);
                PlayerPrefs.SetString("data2", json2);
                //PlayerHP.SetMaxHealth(GameSetting.PlayerHP = 100); //最高生命值
                PlayerPrefs.SetString("S2Enter", "true");
                Debug.Log("Saved!!!");
                PlayerHP.BuyPoka();
                GameSetting.Save();
                PlayerPrefs.Save();
            }
        }
    }
}