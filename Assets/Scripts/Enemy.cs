using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    #region 更新日誌
    /// <summary>
    /// 2022/05/27更新日誌
    /// 新增了控制QTE按鈕產生機制
    /// 原本為動態產生，改為一開始就生好擺在敵人上面並透過onTrigger來控制
    /// 產生的機制也改為由Enemy的cs來產生。
    /// 而且現在只能指定到第一個遇見的敵人，未來會改為敵人全體。
    /// 我理想是想要每次產生(重新開啟遊戲, 死掉重來, etc...)都是不同按鍵(預設3個)
    /// 而boss一次攻擊得按比較多，所以固定按鍵。
    /// </summary>
    #endregion
    [Header("Objects")]
    [SerializeField] public GameObject QTEBtn;
    [SerializeField] public GameObject pool;
    [SerializeField] public Transform enemy;
    [SerializeField] public GameObject Player;

    UnityEvent m_MyEvent = new UnityEvent(); //QTE按鈕開啟關閉事件觸發
    //[SerializeField] public GameObject Terry;
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

        QTEBtn = GameObject.Find("samurai ixel");   //把Find用在Start只能抓一次，應該有更好做法
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
            if (Input.GetKeyDown(KeyCode.Q))
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
        Debug.Log("gggggggggg");
    }
    #endregion

    /// <summary>
    /// QTE開啟關閉事件觸發
    /// </summary>
    void QTEBtnActive()
    {
        Debug.Log("got it");
        QTEBtn.SetActive(true);
    }
}
