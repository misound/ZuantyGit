using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] public TakeEnemy takeEnemy;
    [SerializeField] public OldPlayerController playerController;

    [SerializeField] public GameObject QTEBtn_U;
    [SerializeField] public GameObject QTEBtn_I;
    [SerializeField] public GameObject QTEBtn_O;
    [SerializeField] public GameObject pool;
    [SerializeField] public GameObject Trigger;
    [SerializeField] public GameObject Player;

    [Header("Componets")]
    [SerializeField] public Rigidbody2D rb;

    [Header("SuccessTimeReset")]
    [SerializeField] public float slowdownFactor = 0.05f;

    [Header("QTE CoolDownTime")]
    [SerializeField] public float slowdownLength = 2f;
    [SerializeField] public float cooldownTime = 3.0f;
    [SerializeField] private float timer = 0;
    [SerializeField] private bool isStartTime = false;
    [SerializeField] private bool skillInvalid = false;
    [SerializeField] private bool QTEInvalid = false;

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
        

        currentHealth = maxHealth;

        m_MyEvent_U.AddListener(QTEBtn_UActive);
        m_MyEvent_I.AddListener(QTEBtn_IActive);
        m_MyEvent_O.AddListener(QTEBtn_OActive);
        m_MyEvent_Trigger.AddListener(TriggerActive);

        takeEnemy = FindObjectOfType<TakeEnemy>();
        playerController = FindObjectOfType<OldPlayerController>();
    }

    void Update()
    {
        QTE_invalid();
        QTETrigger();
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
    #region QTE失效
    public void QTE_invalid()
    {
        if (QTEInvalid)
        {
            isStartTime = true;
            skillInvalid = true;
        }
        if (isStartTime)
        {
            if (skillInvalid && timer == 0)
            {
                QTEInvalid = false;
            }

            if (timer >= cooldownTime)
            {
                timer = 0;
                isStartTime = false;
                skillInvalid = false;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }
    #endregion
    #region 子彈時間重置
    void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
    #endregion
    #region QTE觸發
    void QTETrigger()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //RandomQTE = Random.Range(1, 4);
        }
        if (takeEnemy.EnemyTargets != null && Time.timeScale <= 0.4 && Time.timeScale >= 0.05 && playerController.CanKill)
        {
            takeEnemy.EnemyTargets.Trigger.SetActive(true);
            if (takeEnemy.EnemyTargets.RandomQTE == 1)
            {
                m_MyEvent_U.Invoke(); //Begin the action
                if (Input.GetKeyDown(KeyCode.U) && QTEBtn_U.activeInHierarchy == true && QTEInvalid == false)
                {
                    float distoEnemy = Vector3.Distance(transform.position, playerController.transform.position);
                    if (distoEnemy < takeEnemy.range)
                    {
                        DoSlowMotion();
                        Time.timeScale = 1;
                        takeEnemy.slaind = true;
                    }
                }
                if (Input.GetKeyDown(KeyCode.I) && QTEBtn_U.activeInHierarchy == true)
                {
                    QTEInvalid = true;
                }
                if (Input.GetKeyDown(KeyCode.O) && QTEBtn_U.activeInHierarchy == true)
                {
                    QTEInvalid = true;
                }
            }
            if (takeEnemy.EnemyTargets.RandomQTE == 2)
            {
                
                m_MyEvent_I.Invoke();
                if (Input.GetKeyDown(KeyCode.U) && QTEBtn_I.activeInHierarchy == true)
                {
                    QTEInvalid = true;
                }
                if (Input.GetKeyDown(KeyCode.I) && QTEBtn_I.activeInHierarchy == true && QTEInvalid == false)
                {
                    float distoEnemy = Vector3.Distance(transform.position, playerController.transform.position);
                    if (distoEnemy < takeEnemy.range)
                    {
                        DoSlowMotion();
                        Time.timeScale = 1;
                        takeEnemy.slaind = true;
                    }
                }
                if (Input.GetKeyDown(KeyCode.O) && QTEBtn_I.activeInHierarchy == true)
                {
                    QTEInvalid = true;
                }
            }
            if (takeEnemy.EnemyTargets.RandomQTE == 3)
            {
                
                m_MyEvent_O.Invoke();
                if (Input.GetKeyDown(KeyCode.U) && QTEBtn_O.activeInHierarchy == true)
                {
                    QTEInvalid = true;
                }
                if (Input.GetKeyDown(KeyCode.I) && QTEBtn_O.activeInHierarchy == true)
                {
                    QTEInvalid = true;
                }
                if (Input.GetKeyDown(KeyCode.O) && QTEBtn_O.activeInHierarchy == true && QTEInvalid == false)
                {
                    float distoEnemy = Vector3.Distance(transform.position, playerController.transform.position);
                    if (distoEnemy < takeEnemy.range)
                    {
                        DoSlowMotion();
                        Time.timeScale = 1;
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
    #endregion
}
