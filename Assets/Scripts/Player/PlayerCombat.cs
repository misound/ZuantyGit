using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Animations;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    
    public float attackRange = 0.5f;
    public int attackDamage = 40;

    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    
    
    
    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
         if (Input.GetKeyDown(KeyCode.C))
         {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
         }
        }
    }
    public void Attack()
    {
        //播放攻擊動畫
        animator.SetTrigger(("Attack"));
        //偵測敵人是否在攻擊範圍
        Collider2D[]hitEnemy=Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        //攻擊敵人
        foreach (Collider2D enemy in hitEnemy)
        {
            enemy.GetComponent<Enemy>().TakeDamege(attackDamage);
        }
        

    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }
}
