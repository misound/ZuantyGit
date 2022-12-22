using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerCombat : MonoBehaviour
{
    public Animator _anim;
    public Transform attackPoint;
    public float attackRange =0.5f;
    public LayerMask enemyLayers;
    public Collider2D attackbox;
    
    private bool faceRight;

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
            enemy.GetComponent<EnemyBomb>().TakeBombHealth(attackDamage);
        }
        TrangeRotate();
        
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }

    public void TrangeRotate()
    {
        
        Vector3 mousepos = Input.mousePosition;
        if (mousepos.x < transform.position.x)
        {
            //faceRight=false;
            transform.Rotate(0f, 180f, 0f);
            Debug.Log(mousepos.x);
            Debug.Log(mousepos.y);
            
        }
        else
        {
            //faceRight = true;
            transform.Rotate(0f, 0f, 0f);
        }
    }

    public void Flip()
    {
        faceRight = !faceRight;
        transform.Rotate(0f, 180f, 0f);
    }
    
}
