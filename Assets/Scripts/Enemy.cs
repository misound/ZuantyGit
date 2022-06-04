using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    #region 更新日誌
    /// 2022/05/27更新日誌
    /// 新增了控制QTE按鈕產生機制
    /// 原本為動態產生，改為一開始就生好擺在敵人上面並透過onTrigger來控制
    /// 產生的機制也改為由Enemy的cs來產生。
    /// 而且現在只能指定到第一個遇見的敵人，未來會改為敵人全體。
    /// 我理想是想要每次產生(重新開啟遊戲, 死掉重來, etc...)都是不同按鍵(預設3個)
    /// 而boss一次攻擊得按比較多，所以固定按鍵。
    /// 

    /// 2022/05/29更新日誌
    /// 讓QTE按鈕作用的方式由GetComponent變為第一個產生的子類別。
    /// 隨機產生按鈕的方式可以嘗試用GetSet來寫試試看
    /// 不過擊殺動作得要改去PlayerController應該不能寫在Enemy
    /// 

    /// 2022/05/35更新日誌
    /// QTE按鈕產生方式改為抓QTE.cs，同時可自定義圖片和按鈕
    /// 但也遇到了BUG，像是SetActive無法作用在Hierarchy的QTE按鈕上，但是Project那邊有作用
    /// 還有就是target都有抓到QTE.cs和QTEspriteMgr.cs，但就是無法換圖片
    /// 
    #endregion
    [Header("Objects")]
    [SerializeField] public QTE qte;
    [SerializeField] public QTESpriteMgr qTESpriteMgr;

    [SerializeField] public GameObject QTEBtn;
    [SerializeField] public GameObject pool;

    [SerializeField] public GameObject Player;



    UnityEvent m_MyEvent = new UnityEvent(); //QTE按鈕開啟關閉事件觸發

    [Header("hp")]
    public int maxHealth = 100;

    private int currentHealth;
    // Start is called before the first frame update
    void Start()
    {

        currentHealth = maxHealth;

        m_MyEvent.AddListener(QTEBtnActive);

        GameObject target = Instantiate(QTEBtn, transform.position, transform.rotation); //實例化QTE按鈕並跟隨目標
        target.transform.parent = pool.transform; //丟去父類別

        QTE _qte = target.GetComponent<QTE>();
        _qte.QTEButton = qTESpriteMgr.sprites[Random.Range(0, 2)];
        if (GetComponent<QTE>())
        {
            Debug.Log("最好給我動喔");
        }


        //target = gameObject.transform.GetChild(0).gameObject;   //直接指定第一個子類別
        Player = GameObject.Find("player");         //僅讓玩家順移，之後考慮有無更好作法

    }

    void Update()
    {

    }

    public void TakeDamege(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    { 
        //Die ani,ation
        
        //Disable the enemy
        Debug.Log(gameObject.name+"DIE!!!");
        Destroy(this.gameObject);
    }

    #region 碰撞相關
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.timeScale <= 0.4 && m_MyEvent != null)
        {
            m_MyEvent.Invoke();            //Begin the action
            if (Input.GetKeyDown(KeyCode.U))
            {
                Destroy(this.gameObject);
                Player.transform.position = this.gameObject.transform.localPosition;
            }
        }
        else
        {
            QTEBtn.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        QTEBtn.SetActive(false);
        //Debug.Log("gggggggggg");
    }
    #endregion

    /// <summary>
    /// QTE開啟關閉事件觸發
    /// </summary>
    void QTEBtnActive()
    {
        QTEBtn.SetActive(true);
        if(QTEBtn.activeInHierarchy == true)
        {
        Debug.Log("我是Enemy觸發");
        }

    }
}
