using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S4Mgr : MonoBehaviour
{
    #region 屬性與變數們
    [Header("GameObject Management")]
    [SerializeField] private GameObject DoorPrefab;
    [SerializeField] private GameObject AWPrefab;
    [SerializeField] private GameObject[] CheckPoint;
    [SerializeField] private GameObject[] DPos;
    [SerializeField] private GameObject[] AWPos;

    [Header("Components")]
    [SerializeField] private HealthBar PlayerHP;

    //到了最後一關，但因為是常重載關卡所以好像沒那麼需要
    private bool EnteredS4 = false;
    #endregion

    private void Awake()
    {
        Enter();

        //音量更新
        FindObjectOfType<AudioMgr>().BGMCheck = true;
        FindObjectOfType<AudioMgr>().SECheck = true;
    }

    private void Start()
    {
        //載入遊戲登錄物件
        InstantiateGameObj();
        //關滑鼠顯示
        Cursor.visible = false;
    }
    #region 入口
    private void Enter()
    {
        //抓元件
        PlayerHP = FindObjectOfType<HealthBar>();
        //判斷是否為新遊戲
        EnteredS4 = bool.Parse((PlayerPrefs.GetString("S4Enter")));
        //賦予PlayerPrefs初始值
        PlayerPrefs.SetString("D4-1S", "false");
        //賦予PlayerPrefs初始值
        for (int i = 1; i < 7; i++)
        {
            PlayerPrefs.SetString($"AW4-{i}S", "false");
        }
        if (EnteredS4)
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
            //最後一關沒有存檔點，所以回復道具一律補滿
            PlayerHP.BuyPoka();
        }
        else if (!EnteredS4)
        {
            //重置已登錄物件狀態
            S4Item S4Item = (S4Item)Factory.reset("S4");
            //已破壞狀態全部切為false
            GameSetting.DList = S4Item.FakeData1();
            GameSetting.WList = S4Item.FakeData2();
            //最後一關沒有存檔點，所以回復道具一律補滿
            PlayerHP.BuyPoka();
        }
        //血量也補滿
        PlayerHP.SetMaxHealth(GameSetting.PlayerHP = 100);
        if (GameSetting.PlayerHP <= 0)
        {
            PlayerHP.SetMaxHealth(GameSetting.PlayerHP = PlayerPrefs.GetInt("PlayerHP"));
            //被揍死就補滿
            PlayerHP.BuyPoka();
        }
        //初始化
        GameSetting.Falling = false;
        GameSetting.Falled = false;
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
        for (int i = 1; i < 7; i++)
        {
            result.Add(new AtkWData() { AWName = $"AW4-{i}", AWStates = bool.Parse((PlayerPrefs.GetString($"AW4-{i}S"))) });

        }

        return result;
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
    }
    #endregion
}