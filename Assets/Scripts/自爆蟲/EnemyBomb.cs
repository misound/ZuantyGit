using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;

    public float speed;
    public float distance;

    private bool mustTurn;
    private bool mustPatrol;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Collider coll;
   

    
// Start is called before the first frame update
    void Start()
    {
        mustPatrol = true;

    }

    private void FixedUpdate()
    {
        if (mustPatrol)
        {
            Patrol();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mustPatrol)
        {
            mustTurn = !Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        }
    }

    #region 巡邏
    public void Patrol()
    {
        if (mustTurn)
        {
            Flip();
        }
        rb.velocity = new Vector2(speed * Time.deltaTime,rb.velocity.y);
        
    }
    #endregion
    #region 腳色翻轉

    void Flip()
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        speed *= -1;
        mustPatrol = true;
    }

    #endregion
    /*#region 攻擊玩家

    public void Attack()
    {
        distance = Vector2.Distance(transform.position,player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        transform.position =
            Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime); 
    }
    #endregion*/

    

    
}
