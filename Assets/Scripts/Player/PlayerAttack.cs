using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public int atkDamage=40;

    private Animator _anim;
    private Collider2D atkCol;
    public float startTime;
    public float endTime;
    public bool recover;

    [Header("Atk Combo")] 
    [SerializeField] public float cooldown;
    [SerializeField] public float attackTimer;
    [SerializeField] public int combo;
    [SerializeField] public int maxCombo = 3;

    [SerializeField] public bool isAttack;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        atkCol = GetComponent<Collider2D>();
        combo = 0;
        atkCol.enabled = false;
    }

    // Update is called once per frame 
    void Update()
    {
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
            StartCoroutine(startHitBox());
        }
        else if(combo==1&& cooldown>=0)
        {
            _anim.SetTrigger("Attack2");
            StartCoroutine(startHitBox());
        }
        else if(combo==2&& cooldown>=0)
        {
            _anim.SetTrigger("Attack3");
            StartCoroutine(startHitBox());
        }
        else if (combo>=maxCombo)
        {
            combo = 0;
            _anim.SetTrigger("Attack1");
            StartCoroutine(startHitBox());
        }
        
        
    }
    IEnumerator startHitBox()
    {
        recover = true;
        yield return new WaitForSeconds(startTime);
        atkCol.enabled = true;
        StartCoroutine(disableHitBox());
    }

    IEnumerator disableHitBox()
    {
        yield return new WaitForSeconds(endTime);
        atkCol.enabled = false;
        recover = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<EnemyBomb>() != null)
        {
            other.GetComponent<EnemyBomb>().TakeBombHealth(atkDamage);
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
    
}
