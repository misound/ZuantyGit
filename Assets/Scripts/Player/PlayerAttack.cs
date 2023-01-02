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
    // Start is called before the first frame update
    void Start()
    {
        _anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        atkCol = GetComponent<Collider2D>();
        atkCol.enabled = false;
    }

    // Update is called once per frame 
    void Update()
    {
    }

    public void Attack()
    {
            _anim.SetTrigger("Attack1");
            StartCoroutine(startHitBox());
        
    }
    IEnumerator startHitBox()
    {
        yield return new WaitForSeconds(startTime);
        atkCol.enabled = true;
        StartCoroutine(disableHitBox());
    }

    IEnumerator disableHitBox()
    {
        yield return new WaitForSeconds(endTime);
        atkCol.enabled = false;
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
