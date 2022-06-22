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

    [Header("SuccessTimeReset")]
    [SerializeField] public float slowdownFactor = 0.05f;



    private int RandomQTE;

    UnityEvent m_MyEvent_U = new UnityEvent(); //QTE事件產生
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

        m_MyEvent_U.AddListener(QTEBtn_UActive);
        m_MyEvent_I.AddListener(QTEBtn_IActive);
        m_MyEvent_O.AddListener(QTEBtn_OActive);

        Player = GameObject.Find("player");         //僅供玩家順移

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
        if (Time.timeScale <= 0.4 && m_MyEvent_U != null)
        {
            if(RandomQTE == 1)
            {
                m_MyEvent_U.Invoke();            //Begin the action
                if (Input.GetKeyDown(KeyCode.U))
                {
                    Destroy(this.gameObject);
                    Player.transform.position = this.gameObject.transform.localPosition;
                    DoSlowMotion();
                }
            }
            if (RandomQTE == 2)
            {
            Debug.Log("duck");
            m_MyEvent_I.Invoke();
                if (Input.GetKeyDown(KeyCode.I))
                {
                    Destroy(this.gameObject);
                    Player.transform.position = this.gameObject.transform.localPosition;
                    DoSlowMotion();
                }
            }
            if (RandomQTE == 3)
            {
            Debug.Log("giraffe");
            m_MyEvent_O.Invoke();
                if (Input.GetKeyDown(KeyCode.O))
                {
                    Destroy(this.gameObject);
                    Player.transform.position = this.gameObject.transform.localPosition;
                    DoSlowMotion();
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

    #region QTE顯示
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
    #endregion

    void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}
