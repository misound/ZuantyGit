using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHandler : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpSpeed = 5.0f;
    public Rigidbody2D rb;
    public Animator animator;

    public int CherryCount = 0;

    private bool isHurt = false;
    private int collX = 0;
    public int collDefaultMoveX = 5;  //撞到敵人的反彈速度  
    private GameObject door = null;
    public GameObject cherry;
    
    
    public GameObject enterDialogPanel;

    void Start()
    {
        enterDialogPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(1);
        }
        Movement();
    }

    void Movement()
    {

        if (isHurt)
        {
            rb.velocity = new Vector2(collX, rb.velocity.y);
            return;//下面通通不處理
        }

        // 移動位置
        float horizontalMove = Input.GetAxis("Horizontal"); //讀取輸入數據
        if (horizontalMove != 0)
        {
            //Debug.Log("horizontalMove-->"+horizontalMove);
            rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y); //物理位移
            animator.SetFloat("running", Mathf.Abs(horizontalMove)); //動畫控制
        }

        //跳躍控制
        if (Input.GetButtonDown("Jump"))
        {
            
            //Debug.Log("Jumping....");
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }

        // 控制顯示方向
        float horizontalRaw = Input.GetAxisRaw("Horizontal"); //讀取輸入數據(方向
        if (horizontalRaw != 0)
        {
            transform.localScale = new Vector3(horizontalRaw, 1, 1);
        }
    }


    #region 處理相關事件

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.ToLower().Contains("cherry"))
        {
            CherryCount++;
            Destroy(cherry);
        }

        

        if (collision.name.ToLower().Contains("door"))
        {
            door = collision.gameObject;
            enterDialogPanel.SetActive(true);
        }
        
        
       

    }

    



    #endregion
}