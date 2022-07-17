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
    [SerializeField] public TakeEnemy takeEnemy;

    [SerializeField] public GameObject QTEBtn_U;
    [SerializeField] public GameObject QTEBtn_I;
    [SerializeField] public GameObject QTEBtn_O;
    [SerializeField] public GameObject pool;
    [SerializeField] public GameObject Trigger;

    [SerializeField] public GameObject Player;

    [Header("SuccessTimeReset")]
    [SerializeField] public float slowdownFactor = 0.05f;



    private int RandomQTE;

    UnityEvent m_MyEvent_U = new UnityEvent(); //QTE事件產生
    UnityEvent m_MyEvent_I = new UnityEvent();
    UnityEvent m_MyEvent_O = new UnityEvent();
    UnityEvent m_MyEvent_Trigger = new UnityEvent();

    [Header("hp")]
    public int maxHealth = 100;

    private int currentHealth;

    private GUIStyle guiStyle = new GUIStyle();
    // Start is called before the first frame update
    void Start()
    {
        RandomQTE = Random.Range(1, 4);
        Debug.Log(RandomQTE);

        currentHealth = maxHealth;

        m_MyEvent_U.AddListener(QTEBtn_UActive);
        m_MyEvent_I.AddListener(QTEBtn_IActive);
        m_MyEvent_O.AddListener(QTEBtn_OActive);
        m_MyEvent_Trigger.AddListener(TriggerActive);

        Player = GameObject.Find("player");         //僅供玩家順移
        takeEnemy = FindObjectOfType<TakeEnemy>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //RandomQTE = Random.Range(1, 4);
        }
        if (takeEnemy.EnemyTargets != null && Time.timeScale <= 0.4)
        {
            takeEnemy.EnemyTargets.Trigger.SetActive(true);
            if (takeEnemy.EnemyTargets.RandomQTE == 1)
            {
                m_MyEvent_U.Invoke(); //Begin the action
                if (Input.GetKeyDown(KeyCode.U) && QTEBtn_U.activeInHierarchy == true)
                {
                    float distoEnemy = Vector3.Distance(transform.position, Player.transform.position);
                    if (distoEnemy < takeEnemy.range)
                    {
                        Destroy(this.gameObject);
                        Player.transform.position = this.gameObject.transform.localPosition;
                        DoSlowMotion();
                        takeEnemy.slaind = true;
                    }
                }
                if (Input.GetKeyDown(KeyCode.I) && QTEBtn_U.activeInHierarchy == true)
                {

                }
                if (Input.GetKeyDown(KeyCode.O) && QTEBtn_U.activeInHierarchy == true)
                {

                }
            }
            if (takeEnemy.EnemyTargets.RandomQTE == 2)
            {
                Debug.Log("duck");
                m_MyEvent_I.Invoke();
                if (Input.GetKeyDown(KeyCode.U) && QTEBtn_I.activeInHierarchy == true)
                {

                }
                if (Input.GetKeyDown(KeyCode.I) && QTEBtn_I.activeInHierarchy == true)
                {
                    float distoEnemy = Vector3.Distance(transform.position, Player.transform.position);
                    if (distoEnemy < takeEnemy.range)
                    {
                        Destroy(this.gameObject);
                        Player.transform.position = this.gameObject.transform.localPosition;
                        DoSlowMotion();
                        takeEnemy.slaind = true;
                    }
                }
                if (Input.GetKeyDown(KeyCode.O) && QTEBtn_I.activeInHierarchy == true)
                {

                }
            }
            if (takeEnemy.EnemyTargets.RandomQTE == 3)
            {
                Debug.Log("giraffe");
                m_MyEvent_O.Invoke();
                if (Input.GetKeyDown(KeyCode.U) && QTEBtn_O.activeInHierarchy == true)
                {

                }
                if (Input.GetKeyDown(KeyCode.I) && QTEBtn_O.activeInHierarchy == true)
                {

                }
                if (Input.GetKeyDown(KeyCode.O) && QTEBtn_O.activeInHierarchy == true)
                {
                    float distoEnemy = Vector3.Distance(transform.position, Player.transform.position);
                    if (distoEnemy < takeEnemy.range)
                    {
                        Destroy(this.gameObject);
                        Player.transform.position = this.gameObject.transform.localPosition;
                        DoSlowMotion();
                        takeEnemy.slaind = true;
                    }
                }
            }
        }
        else
        {
            QTEBtn_U.SetActive(false);
            QTEBtn_O.SetActive(false);
            QTEBtn_I.SetActive(false);
            Trigger.SetActive(false);
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
        Debug.Log(gameObject.name + "DIE!!!");
        Destroy(this.gameObject);
    }

    #region 碰撞相關
    private void OnTriggerStay2D(Collider2D collision)
    {
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
    }
    #endregion

    #region QTE顯示
    void QTEBtn_UActive()
    {
        takeEnemy.EnemyTargets.QTEBtn_U.SetActive(true);
        takeEnemy.EnemyTargets.QTEBtn_I.SetActive(false);
        takeEnemy.EnemyTargets.QTEBtn_O.SetActive(false);
    }
    void QTEBtn_IActive()
    {
        takeEnemy.EnemyTargets.QTEBtn_U.SetActive(false);
        takeEnemy.EnemyTargets.QTEBtn_I.SetActive(true);
        takeEnemy.EnemyTargets.QTEBtn_O.SetActive(false);
    }
    void QTEBtn_OActive()
    {
        takeEnemy.EnemyTargets.QTEBtn_U.SetActive(false);
        takeEnemy.EnemyTargets.QTEBtn_I.SetActive(false);
        takeEnemy.EnemyTargets.QTEBtn_O.SetActive(true);
    }

    void TriggerActive()
    {
        takeEnemy.EnemyTargets.Trigger.SetActive(true);
    }
    #endregion

    void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}
