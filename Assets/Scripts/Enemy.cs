using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] public QTE qte;
    [SerializeField] public QTESpriteMgr qTESpriteMgr;

    [SerializeField] public GameObject QTEBtn_U;
    [SerializeField] public GameObject QTEBtn_I;
    [SerializeField] public GameObject QTEBtn_O;
    [SerializeField] public GameObject pool;

    [SerializeField] public GameObject Player;

    private int direction;
    public enum eDirection
    {
        U,
        I,
        O,
    }

    private int RandomQTE;

    UnityEvent m_MyEvent = new UnityEvent(); //QTE事件產生
    UnityEvent m_MyEvent_I = new UnityEvent();
    UnityEvent m_MyEvent_O = new UnityEvent();

    [Header("hp")]
    public int maxHealth = 100;

    private int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        RandomQTE = Random.Range(1, 4);
        Debug.Log(RandomQTE);

        currentHealth = maxHealth;
        switch(direction)
        {
            case 1:
                QTEBtn_U.SetActive(true);
                QTEBtn_I.SetActive(false);
                QTEBtn_O.SetActive(false);
                break;
            case 2:
                QTEBtn_IActive();
                break;
            case 3:
                QTEBtn_OActive();
                break;




        }

        m_MyEvent.AddListener(QTEBtn_UActive);
        m_MyEvent_I.AddListener(QTEBtn_IActive);
        m_MyEvent_O.AddListener(QTEBtn_OActive);

        /*
        GameObject target = Instantiate(QTEBtn, transform.position, transform.rotation); //實力畫
        target.transform.parent = pool.transform; //富類別

        QTE _qte = target.GetComponent<QTE>();
        _qte.QTEButton = qTESpriteMgr.sprites[Random.Range(0, 2)];
        if (GetComponent<QTE>())
        {
            Debug.Log("�̦n���ڰʳ�");
        }
        */

        //target = gameObject.transform.GetChild(0).gameObject;   //直接選取子類別
        Player = GameObject.Find("player");         //僅供玩家順移

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            direction = 1;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            direction = 2;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            direction = 3;
        }
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
            if(RandomQTE == 1)
            {
                m_MyEvent.Invoke();            //Begin the action
                if (Input.GetKeyDown(KeyCode.U))
                {
                Destroy(this.gameObject);
                Player.transform.position = this.gameObject.transform.localPosition;
                }
            }
            if (RandomQTE == 2)
            {
            Debug.Log("duck");
            m_MyEvent_O.Invoke();
                if (Input.GetKeyDown(KeyCode.I))
                {
                    Destroy(this.gameObject);
                    Player.transform.position = this.gameObject.transform.localPosition;
                }
            }
            if (RandomQTE == 3)
            {
            Debug.Log("giraffe");
            m_MyEvent_I.Invoke();
                if (Input.GetKeyDown(KeyCode.O))
                {
                    Destroy(this.gameObject);
                    Player.transform.position = this.gameObject.transform.localPosition;
                }
            }
 
        }
           else
            {
                QTEBtn_U.SetActive(false);
                QTEBtn_O.SetActive(false);
                QTEBtn_I.SetActive(false);
            }
        


 

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        QTEBtn_U.SetActive(false);
        QTEBtn_O.SetActive(false);
        QTEBtn_I.SetActive(false);
    }
    #endregion

    /// <summary>
    /// QTE顯示
    /// </summary>
    void QTEBtn_UActive()
    {
        QTEBtn_U.SetActive(true);
        QTEBtn_I.SetActive(false);
        QTEBtn_O.SetActive(false);
        if (QTEBtn_U.activeInHierarchy == true)
        {
            Debug.Log("我是大笑臉");
        }

    }
    void QTEBtn_IActive()
    {
        QTEBtn_U.SetActive(false);
        QTEBtn_I.SetActive(true);
        QTEBtn_O.SetActive(false);
        if (QTEBtn_I.activeInHierarchy == true)
        {
            Debug.Log("我是大比逼");
        }

    }
    void QTEBtn_OActive()
    {
        QTEBtn_U.SetActive(false);
        QTEBtn_I.SetActive(false);
        QTEBtn_O.SetActive(true);
        if (QTEBtn_O.activeInHierarchy == true)
        {
            Debug.Log("我是大俗投");
        }

    }
}
