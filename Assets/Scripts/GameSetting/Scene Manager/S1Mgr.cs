using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class S1Mgr : MonoBehaviour
{
    #region 屬性與變數們
    [Header("GameObject Management")]
    [SerializeField] private GameObject DoorPrefab;
    [SerializeField] private GameObject AWPrefab;
    [SerializeField] private GameObject[] CheckPoint;
    [SerializeField] private GameObject[] RespawnPoint;
    [SerializeField] private GameObject[] DPos;
    [SerializeField] private GameObject[] AWPos;

    [Header("Fall Out of Screen")]
    [SerializeField] private GameObject[] FallingLine;
    [SerializeField] private int FallDmg = 30;
    [SerializeField] private float FallSec = 3.0f;

    [Header("Components")]
    [SerializeField] private HealthBar PlayerHP;
    [SerializeField] private SpeedPlayerController SPC;

    private bool EnteredS1 = false;
    #endregion

    private void Awake()
    {
        Enter();
    }

    private void Start()
    {
        //載入遊戲登錄物件
        InstantiateGameObj();

        //關滑鼠顯示
        Cursor.visible = false;
        //音量更新
        FindObjectOfType<AudioMgr>().BGMCheck = true;
        FindObjectOfType<AudioMgr>().SECheck = true;
    }

    void Update()
    {
        StartCoroutine(FallLine());
    }

    #region 入口
    private void Enter()
    {
        GameSetting.AudioReady = true;

        //判斷是否為新遊戲
        EnteredS1 = bool.Parse((PlayerPrefs.GetString("S1Enter")));

        //賦予PlayerPrefs初始值
        PlayerPrefs.SetString("D1-1S", "false");
        for(int i = 0; i < 6; i++)
        {
            PlayerPrefs.SetString($"AW1-{i}S", "false");
        }

        //抓元件
        PlayerHP = FindObjectOfType<HealthBar>();
        SPC = FindObjectOfType<SpeedPlayerController>();

        if (EnteredS1)
        {
            //則載入被存檔點寫入的資料
            GameSetting.DList = CakeData1();
            GameSetting.WList = CakeData2();
            //載入PlayerPrefs儲存的資料
            string json = PlayerPrefs.GetString("data");
            string json2 = PlayerPrefs.GetString("data2");
            //反序列化
            GameSetting.DList = JsonConvert.DeserializeObject<IList<Itemdata>>(json);
            GameSetting.WList = JsonConvert.DeserializeObject<IList<AtkWData>>(json2);


            //重載場景時方法為掉出畫面時，可使用快速存檔點方法
            if (GameSetting.Falling)
            {
                GameSetting.TempPoint();
            }
            else //除了掉落不外乎就是死亡，直接回大存檔點吧
            {
                GameSetting.Load();
            }



            //回檔時的血量判斷處理
            if (GameSetting.PlayerHP <= 0)
            {
                PlayerHP.SetMaxHealth(GameSetting.PlayerHP = 100); //最高生命值
                PlayerHP.SetHealth(GameSetting.PlayerHP); //刷新當前血量
                //回復道具自動補滿
                PlayerHP.BuyPoka();
            }
            else if (GameSetting.PlayerHP > 0)
            {
                PlayerHP.SetMaxHealth(GameSetting.PlayerHP = 100); //最高生命值
                PlayerHP.GetHealth(GameSetting.PlayerHP = PlayerPrefs.GetInt("PlayerHP")); //讀取血量
                PlayerHP.SetHealth(GameSetting.PlayerHP); //刷新當前血量
                //讀取回復道具
                PlayerHP.GetPoka(ref GameSetting.Poka);
            }

            //設定玩家讀檔位置
            SPC.transform.position = GameSetting.Playerpos;
        }
        else if (!EnteredS1)
        {
            //重置已登錄物件狀態
            S1Item S1Item = (S1Item)Factory.reset("S1");
            //已破壞狀態全部切為false
            GameSetting.DList = S1Item.FakeData1();
            GameSetting.WList = S1Item.FakeData2();
            //最大血量重置
            PlayerHP.SetMaxHealth(GameSetting.PlayerHP = 100);

            if (GameSetting.PlayerHP <= 0)
            {
                PlayerHP.SetMaxHealth(GameSetting.PlayerHP = 100); //最大血量重置
                PlayerHP.SetHealth(GameSetting.PlayerHP); //刷新當前血量
            }


            //初始化
            GameSetting.Falling = false;
            GameSetting.Falled = false;
        }
    }

    #endregion
    #region 生產遊戲物件

    private void InstantiateGameObj()
    {
        //生產可破壞的門
        for (int i = 0; i < GameSetting.DList.Count; i++)
        {
            GameObject temp = Instantiate(DoorPrefab, DPos[i].transform);
            temp.transform.position = DPos[i].transform.position;
            temp.transform.localScale = Vector3.one;
            temp.gameObject.name = GameSetting.DList[i].Name;
            CanAtkDoor door = temp.GetComponent<CanAtkDoor>();
            door.SetDoorData(GameSetting.DList[i]);
        }

        //生產可破壞的牆
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

    #endregion
    #region 第一關可破壞物件資料

    /// <summary>
    /// 獲取物件資料是否被破壞的狀態
    /// </summary>
    /// <returns>回傳資料狀態清單</returns>
    public IList<Itemdata> CakeData1()
    {
        IList<Itemdata> result = new List<Itemdata>();

        result.Add(new Itemdata() { Name = "D1-1", States = bool.Parse((PlayerPrefs.GetString("D1-1S"))) });
        return result;
    }

    /// <summary>
    /// 獲取物件資料是否被破壞的狀態
    /// </summary>
    /// <returns>回傳資料狀態清單</returns>
    public IList<AtkWData> CakeData2()
    {
        IList<AtkWData> result = new List<AtkWData>();

        for (int i = 1; i < 6; i++)
        {
            result.Add(new AtkWData() { AWName = $"AW1-{i}", AWStates = bool.Parse((PlayerPrefs.GetString($"AW1-{i}S"))) });
        }

        return result;
    }

    #endregion
    #region 掉落處理
    /// <summary>
    /// 掉落線
    /// </summary>
    /// <returns></returns>
    IEnumerator FallLine()
    {
        //設定清單內遊戲物件的位置作為掉落點
        for (int i = 0; i < FallingLine.Length; i++)
        {
            //設定物件左右射線範圍和圖層
            RaycastHit2D hitR = Physics2D.Raycast(FallingLine[i].transform.position, Vector3.right * 10, 10,
                1 << LayerMask.NameToLayer("Default"));
            RaycastHit2D hit = Physics2D.Raycast(FallingLine[i].transform.position, Vector3.left * 10, 10,
                1 << LayerMask.NameToLayer("Default"));

            //左邊探測判定
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    //暴力掉落，沒處理得很好
                    SPC.transform.position = new Vector3
                    (FallingLine[i].transform.position.x,
                        FallingLine[i].transform.position.y - 10);
                    //掉落中，是給攝影機有緩衝過渡的時間
                    GameSetting.Falling = true;
                    //扣掉落傷害
                    GameSetting.PlayerHP -= FallDmg;
                    //更新血量
                    PlayerHP.SetHealth(GameSetting.PlayerHP);
                    //動作時間
                    yield return new WaitForSeconds(FallSec);
                    //動作完成
                    GameSetting.Falled = true;
                }
            }

            if (hitR.collider != null)
            {
                if (hitR.collider.gameObject.CompareTag("Player"))
                {
                    //暴力掉落，沒處理得很好
                    SPC.transform.position = new Vector3
                    (FallingLine[i].transform.position.x,
                        FallingLine[i].transform.position.y - 10);
                    //掉落中，是給攝影機有緩衝過渡的時間
                    GameSetting.Falling = true;
                    //扣掉落傷害
                    GameSetting.PlayerHP -= FallDmg;
                    //更新血量
                    PlayerHP.SetHealth(GameSetting.PlayerHP);
                    //動作時間
                    yield return new WaitForSeconds(FallSec);
                    //動作完成
                    GameSetting.Falled = true;
                }
            }

            yield return null;
        }
    }

    #endregion
    #region 射線顯示
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
    #endregion
    #region 場景內存檔點的碰撞處理

    /// <summary>
    /// 剛進入存檔點區域的處理
    /// </summary>
    /// <param name="col">通常是玩家</param>
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

    /// <summary>
    /// 在存檔區域內才能動作
    /// </summary>
    /// <param name="other">通常是玩家</param>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<SpeedPlayerController>() != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //紀錄玩家目前位置
                GameSetting.Playerpos = SPC.transform.position;
                GameSetting.Playerposx = GameSetting.Playerpos.x;
                GameSetting.Playerposy = GameSetting.Playerpos.y;
                PlayerPrefs.SetFloat("x", GameSetting.Playerposx);
                PlayerPrefs.SetFloat("y", GameSetting.Playerposy);
                //序列化
                string json = JsonConvert.SerializeObject(GameSetting.DList);
                string json2 = JsonConvert.SerializeObject(GameSetting.WList);
                PlayerPrefs.SetString("data", json);
                PlayerPrefs.SetString("data2", json2);
                //有紀錄代表已經有入場證明
                PlayerPrefs.SetString("S1Enter", "true");
                Debug.Log("Saved!!!");
                //撥音效
                GameSetting.SEAudio.Play(AudioMgr.eAudio.Saveed);
                //補滿回復道具
                PlayerHP.BuyPoka();
                //存檔
                GameSetting.Save();
                PlayerPrefs.Save();
            }
        }
    }
    #endregion
}