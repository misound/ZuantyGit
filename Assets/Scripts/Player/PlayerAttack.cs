using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public int atkDamage=40;
    public SpeedPlayerController speedPlayerController;
    private Animator _anim;
    public Collider2D[] atkCol;
    public Collider2D atkCol1;
    public Collider2D atkCol2;
    public Collider2D atkCol3;
    public float startTime;
    public float endTime;
    public bool recover;
    public bool canKill;

    [Header("Atk Combo")] 
    [SerializeField] public float cooldown;
    [SerializeField] public float attackTimer;
    [SerializeField] public int combo;
    [SerializeField] public int maxCombo = 3;
    [SerializeField] public bool isAttack;

    [Header("Ray")] 
    public LayerMask wall;
    public LayerMask enemy;
    public float distance;
    private Vector3 M_pos;
    private Vector3 M_Center;
    private Vector3 M_dir;

    public EnemyWalk enemyExMode;
    public EnemyBomb bombExMode;
    
    // Start is called before the first frame update
    void Start()
    {
        _anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        speedPlayerController = GetComponent<SpeedPlayerController>();

        atkCol = GetComponents<Collider2D>();

        atkCol1 = atkCol[0];
        atkCol2 = atkCol[1];
        atkCol3 = atkCol[2];

        for (int i = 0; i < 3; i++)
        {
            atkCol[i].enabled = false;
        }
        
        combo = 0;

    }

    // Update is called once per frame 
    void Update()
    {
    }

    private void FixedUpdate()
    {
        CheckRay();
    }

    public void AttackCount()
    {
        if (isAttack)
        {
            cooldown -= Time.deltaTime;
        }
        else if(!isAttack)
        {
            cooldown = 1f;
        }

        if (cooldown<0)
        {
            isAttack = false;
            combo = 0;
        }
        
    }
    public void MeleeAttack()
    {
        if (combo==0)
        {
            _anim.SetTrigger("Attack1");
            StartCoroutine(startHitBox1());
        }
        else if(combo==1&& cooldown>=0)
        {
            _anim.SetTrigger("Attack2");
            StartCoroutine(startHitBox2());
        }
        else if(combo==2&& cooldown>=0)
        {
            _anim.SetTrigger("Attack3");
            StartCoroutine(startHitBox3());
        }
        else if (combo>=maxCombo)
        {
            combo = 0;
            _anim.SetTrigger("Attack1");
            StartCoroutine(startHitBox1());
        }
        
        
    }
    IEnumerator startHitBox1()
    {
        recover = true;
        GameSetting.SEAudio.Play(AudioMgr.eAudio.SE_Player_Attack_01);
        yield return new WaitForSeconds(startTime);
        atkCol[0].enabled = true;
        StartCoroutine(disableHitBox1());
    }

    IEnumerator disableHitBox1()
    {
        yield return new WaitForSeconds(endTime);
        atkCol[0].enabled = false;
        recover = false;
    }
    IEnumerator startHitBox2()
    {
        recover = true;
        GameSetting.SEAudio.Play(AudioMgr.eAudio.SE_Player_Attack_02);
        yield return new WaitForSeconds(startTime);
        atkCol[1].enabled = true;
        StartCoroutine(disableHitBox2());
    }

    IEnumerator disableHitBox2()
    {
        yield return new WaitForSeconds(endTime);
        atkCol[1].enabled = false;
        recover = false;
    }
    IEnumerator startHitBox3()
    {
        recover = true;
        GameSetting.SEAudio.Play(AudioMgr.eAudio.SE_Player_Attack_03);
        yield return new WaitForSeconds(startTime);
        atkCol[2].enabled = true;
        StartCoroutine(disableHitBox3());
    }

    IEnumerator disableHitBox3()
    {
        yield return new WaitForSeconds(endTime);
        atkCol[2].enabled = false;
        recover = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<EnemyBomb>() != null)
        {
            GameSetting.SEAudio.Play(AudioMgr.eAudio.SE_Player_Hit);
            other.GetComponent<EnemyBomb>().TakeBombHealth(atkDamage);
        }
        if (other.GetComponent<EnemyWalk>() != null)
        {
            GameSetting.SEAudio.Play(AudioMgr.eAudio.SE_Player_Hit);
            other.GetComponent<EnemyWalk>().TakeWalkHealth(atkDamage);
        }
        else if (other.GetComponent<CanAtkDoor>() != null)
        {
            other.GetComponent<CanAtkDoor>().TakeDoorHP(atkDamage);
        }
        else if (other.GetComponent<AtkWallHandler>() != null)
        {
            other.GetComponent<AtkWallHandler>().TakeAtkWHP(atkDamage);
        }
        else
        {
            Debug.Log("沒啥好打的");
        }
    }

    public void CheckRay()
    {
        M_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        M_Center = transform.position;
        M_dir = M_pos - M_Center;
        //RaycastHit2D hit=Physics2D.Raycast(transform.position, M_dir, distance,wall);
        RaycastHit2D[] hit2D = Physics2D.RaycastAll(transform.position, M_dir, distance,wall);
        Debug.DrawRay(M_Center,M_dir,Color.cyan);

        /*
        if (hit.collider!=null)
        {
            if (hit.collider.CompareTag("WallEnemy"))
            {
                canKill = true;
            }

            if (hit.collider.CompareTag("Enemy"))
            {
                enemyExMode = hit.transform.GetComponent<EnemyWalk>();
                
                if (enemyExMode.exMode)
                {
                    canKill = true;
                }
            }

        }*/

        

        for (int i = 0; i < hit2D.Length; i++)
        {
            if (hit2D[i].collider!=null &&hit2D[i].collider.tag!="Player")
            {
                if (hit2D[0].collider.CompareTag("WallEnemy"))
                {
                    canKill = true;
                }
                else
                {
                    canKill=false;
                }

                if (hit2D[0].collider.CompareTag("Enemy"))
                {
                    enemyExMode = hit2D[0].transform.GetComponent<EnemyWalk>();
                    if (enemyExMode!=null)
                    {
                        if (enemyExMode.exMode)
                        {
                            canKill = true;
                        }
                        else
                        {
                            canKill = false;
                        }
                    }
                }

                if (hit2D[0].collider.CompareTag("Bomb"))
                {
                    bombExMode = hit2D[0].transform.GetComponent<EnemyBomb>();
                    if (bombExMode.bombExMode)
                    {
                        canKill = true;
                    }
                    else
                    {
                        canKill = false;
                    }
                }
            }

        }

        

    }

    
    
}