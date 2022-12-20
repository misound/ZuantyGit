using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator _anim;
    public Transform attackPoint;
    public float attackRange =0.5f;
    public LayerMask enemyLayers;
    public bool hurtEnemy;
    public Collider2D attackbox;

    public int attackDamage = 40; 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }
    public void Attack()
    {
        _anim.SetTrigger("Attack1");

        Collider2D[] hitEnemy=Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemy)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }

    
}
