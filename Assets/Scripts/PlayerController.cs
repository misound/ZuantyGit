﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    //記得看更新日誌//記得看更新日誌//記得看更新日誌//記得看更新日誌
    #region 更新日誌
    /// 
    /// 2022/05/17更新日誌
    /// 換為骷弓之後在白色方格下面跳躍時有BUG，那個白色平台到底是何方神聖?!
    /// 新增有時會無法跳躍的問題
    /// 換為骷弓之後collider的頭太大導致會卡在半空中的問題
    ///

    /// 
    ///2022/05/22更新日誌
    ///把子彈時間跟QTE做結合了但仍有BUG(請參照約第291行)
    ///新增QTE的cs檔
    ///新增還沒有想做開始畫面的意思
    ///還沒新增冷卻時間
    /// 
    #endregion
    //記得看更新日誌//記得看更新日誌//記得看更新日誌//記得看更新日誌
    [Header("Components")]
    private Rigidbody2D rb;
    public CircleCollider2D QTESlow;
    public GameObject QTEBtn;
    public GameObject pool;

    public Transform Enemy;
    public GameObject Terry;

    [Header("Layer Mask")]
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private LayerMask wallLayer;


    [Header("Movement Variables")]
    [SerializeField] private float movementAcceleration;
    [SerializeField] private float maxMovementSpeed;
    [SerializeField] private float groundLinearDrag;
    [SerializeField] public bool facingRight = false;
    private float horizontalDirection;
    private bool changingDirection => (rb.velocity.x > 0f && horizontalDirection < 0f) || (rb.velocity.x < 0f && horizontalDirection > 0f);
    private bool canMove => !wallGrab;

    [Header("Jump Variables")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float airLinearDrag = 2.5f;
    [SerializeField] private float fallMultiplier = 8f;
    [SerializeField] private float lowJumpFallMultiplier = 5f;
    [SerializeField] private int extraJump = 2;
    [SerializeField] private float hangTime = .1f;
    [SerializeField] private float jumpBufferLength = .1f;
    private int extraJumpValue;
    private float jumpBufferCounter;
    private float hangTimeCounter;

    private bool wallGrab => onWall && !onGround && Input.GetButton("WallGrab");

    [Header("Ground Collision Variables")]
    [SerializeField] private float groundRaycastLength;
    [SerializeField] private Vector3 groundRaycastOffset;
    private bool onGround;

    [Header("Wall Collision Variables")] 
    [SerializeField] private float wallRaycastLength;
    [SerializeField] public bool onWall;
    [SerializeField] public bool onRightWall;
    [SerializeField] public float wallSlideModifier = 0.5f;
    [SerializeField] public bool wallSlide => onWall && !onGround && !Input.GetButton("WallGrab") && rb.velocity.y <=0f;
    
    [Header("Corner Correction Variable")]
    [SerializeField] private float topRaycastLength;
    [SerializeField] private Vector3 edgeRaycastOffset;
    [SerializeField] private Vector3 innerRaycastoffset;
    private bool canCornerCorrect;

    [Header("Animation")]
    [SerializeField] public Animator animator;
    [SerializeField] public float HorizontalaMovement;
    
    
    

    [Header("SlowMotion")]
    [SerializeField] public float slowdownFactor = 0.05f;
    [SerializeField] public float slowdownLength = 2f;

    private bool canJump => jumpBufferCounter >= 0f && (hangTimeCounter > 0f || extraJumpValue > 0);

    private GUIStyle guiStyle = new GUIStyle(); //create a new variable

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        QTESlow = GetComponent<CircleCollider2D>();

        guiStyle.fontSize = 40; //for debug, 設定onGUI用
        guiStyle.normal.textColor = Color.red;

    }
    private void Update()
    {
        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

        horizontalDirection = GetInput().x;

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferLength;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        if (canJump) Jump();
    }
    private void FixedUpdate()
    {
        CheckCollisions();
        if(canMove)MoveCharacter();
        ApplyGroundLinearDrag();
        FallMultilplier();
        Animator();
        SlowMotionBtn();
        //跳躍
        if (onGround)
        {
            hangTimeCounter = hangTime;
            extraJumpValue = extraJump;
            ApplyGroundLinearDrag();
            //Animation
            animator.SetBool("isFalling",false);
            animator.SetBool("isJumping",false);
        }
        else
        {
            ApplyAirLinearDrag();
            hangTimeCounter -= Time.fixedDeltaTime;
        }

        if (canJump)
        {
            Jump();
        }
        if (canCornerCorrect)
        {
            CornerCorrect(rb.velocity.y);
        }

        if (wallGrab)
        {
            WallGrab();
        }

        if (wallSlide)
        { 
            WallSlide();
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 20), "HorizontalaMovement=" + HorizontalaMovement, guiStyle);
        GUI.Label(new Rect(0, 40, 100, 20), "horizontalDirection=" + horizontalDirection, guiStyle);
        GUI.Label(new Rect(0, 80, 100, 20), "movementAcceleration=" + movementAcceleration, guiStyle);
    }

    public void WallGrab()
    {
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        StickWall();
    }

    public void WallSlide()
    {
        rb.velocity = new Vector2(rb.velocity.x, -maxMovementSpeed * wallSlideModifier);
        StickWall();
    }
    void Flip()
    {
        facingRight= !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }


    #region 讀取數據
    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    }
    #endregion
    #region 移動
    private void MoveCharacter()
    {
        //動畫數值
        HorizontalaMovement = Input.GetAxis("Horizontal");
        animator.SetFloat("horizontalDirection", HorizontalaMovement);
        //加速與最高速
        rb.AddForce(new Vector2(horizontalDirection, 0f) * movementAcceleration);
    }
    #endregion
    #region 地面奔跑阻力
    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(horizontalDirection) < 0.4f || changingDirection)
        {
            rb.drag = groundLinearDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }
    #endregion
    #region 空中阻力
    private void ApplyAirLinearDrag()
    {

        rb.drag = airLinearDrag;

    }
    #endregion
    #region 跳躍
    private void Jump()
    {
        ApplyAirLinearDrag();
        if (!onGround)
        {
            extraJumpValue--;
        }

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        hangTimeCounter = 0f;
        jumpBufferCounter = 0f;

    }
    #endregion

    public void StickWall()
    {
        //push player torwards wall
        if (onRightWall && horizontalDirection >= 0f)
        {
            rb.velocity = new Vector2(1f, rb.velocity.y);
        }
        else if(!onRightWall&& horizontalDirection<=0f)
        {
            rb.velocity = new Vector2(-1f, rb.velocity.y);
        }
        //Face correct direction
        if (onRightWall && facingRight)
        {
            Flip();
        }
        else if (!onRightWall&& facingRight)
        {
            Flip();
        }

    }
    #region 落下空氣阻力
    private void FallMultilplier()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetButtonDown("Jump"))
        {
            rb.gravityScale = lowJumpFallMultiplier;
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }
    #endregion
    private void CheckCollisions()  //這殺虫
    {
        onGround = Physics2D.Raycast(transform.position * groundRaycastLength, Vector2.down, groundRaycastLength, groundLayer);
        
        //Corner Collisions
        var position = transform.position;
        canCornerCorrect = Physics2D.Raycast(transform.position + edgeRaycastOffset, Vector2.up, topRaycastLength, groundLayer) &&
                           !Physics2D.Raycast(transform.position + innerRaycastoffset, Vector2.up, topRaycastLength, groundLayer) ||
                           Physics2D.Raycast(transform.position - edgeRaycastOffset, Vector2.up, topRaycastLength, groundLayer) &&
                           !Physics2D.Raycast(transform.position - innerRaycastoffset, Vector2.up, topRaycastLength, groundLayer);
        //Wall Collision
        onWall = Physics2D.Raycast(transform.position, Vector2.right, wallRaycastLength, wallLayer) ||
                 Physics2D.Raycast(transform.position, Vector2.left, wallRaycastLength, wallLayer);
        onRightWall = Physics2D.Raycast(transform.position, Vector2.right, wallRaycastLength, wallLayer);
    }
    /// <summary>
    /// 顯示觸發範圍(跳躍修正偵測範圍)
    /// </summary>
    private void OnDrawGizmos() //這又是殺虫
    {
        Gizmos.color = Color.green;
        var position = transform.position;
        Gizmos.DrawLine(position, position + Vector3.down * groundRaycastLength);
        //CornerCheck
        Gizmos.DrawLine(position + edgeRaycastOffset, position + edgeRaycastOffset + Vector3.up * topRaycastLength);
        Gizmos.DrawLine(position - edgeRaycastOffset, position - edgeRaycastOffset + Vector3.up * topRaycastLength);
        Gizmos.DrawLine(position + innerRaycastoffset, position + innerRaycastoffset + Vector3.up * topRaycastLength);
        Gizmos.DrawLine(position - innerRaycastoffset, position - innerRaycastoffset + Vector3.up * topRaycastLength);
        //Corner Distence Check
        Gizmos.DrawLine(position - innerRaycastoffset + Vector3.up * topRaycastLength,
                        position - innerRaycastoffset + Vector3.up * topRaycastLength + Vector3.left * topRaycastLength);
        Gizmos.DrawLine(position + innerRaycastoffset + Vector3.up * topRaycastLength,
                        position + innerRaycastoffset + Vector3.up * topRaycastLength + Vector3.right * topRaycastLength);
        //WallCheck
        Gizmos.DrawLine(transform.position,transform.position+Vector3.right*wallRaycastLength);
        Gizmos.DrawLine(transform.position,transform.position+Vector3.left*wallRaycastLength);
    }

    #region Collider轉角處理
    void CornerCorrect(float Yvelocity)
    {
        //Push player to the right


        RaycastHit2D hit = Physics2D.Raycast(transform.position - innerRaycastoffset + Vector3.up * topRaycastLength, Vector3.left, topRaycastLength, groundLayer);
        if (hit.collider != null)
        {
            float newPos = Vector3.Distance(new Vector3(hit.point.x, transform.position.y, 0f) + Vector3.up * topRaycastLength,
                transform.position - edgeRaycastOffset + Vector3.up * topRaycastLength);
            transform.position = new Vector3(transform.position.x + newPos, transform.position.y, transform.position.z);
            rb.velocity = new Vector2(rb.velocity.x, Yvelocity);
            return;
        }
        //Push player to the left
        hit = Physics2D.Raycast(transform.position + innerRaycastoffset + Vector3.up * topRaycastLength, Vector3.right, topRaycastLength, groundLayer);
        if (hit.collider != null)
        {
            float newPos = Vector3.Distance(new Vector3(hit.point.x, transform.position.y, 0f) + Vector3.up * topRaycastLength,
                transform.position + edgeRaycastOffset + Vector3.up * topRaycastLength);
            transform.position = new Vector3(transform.position.x - newPos, transform.position.y, transform.position.z);
            rb.velocity = new Vector2(rb.velocity.x, Yvelocity);
        }
    }
    #endregion
    #region 動畫相關
    void Animator()
    {
        if (HorizontalaMovement < 0)
        {
            facingRight = false;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            facingRight = true;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
    #endregion
    #region 物件互動相關


    //private void OnTriggerStay2D(Collider2D collision)
    //{

    //    if (Input.GetKeyDown(KeyCode.LeftShift) | Time.timeScale <= 0.4)
    //    {
    //        if (collision.name.ToLower().Contains("terry"))
    //        {

    //            Debug.Log("敵");
    //            GameObject target = Instantiate(QTEBtn);
    //            target.transform.parent = pool.transform; //丟去父類別
    //            target.transform.localScale = Vector3.one; //reset
    //            target.transform.position = Enemy.transform.localPosition; //在敵人身上放按鈕

    //            if (Input.GetKeyDown(KeyCode.Q) & Time.timeScale <= 0.4)
    //            {
    //                transform.position = Enemy.transform.localPosition; //teleport
    //                Destroy(Terry);
    //            }
    //        }
    //    }
    //}

    #endregion
    #region 子彈時間相關
    void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }

    void SlowMotionBtn()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DoSlowMotion();
        }
    }
    #endregion
}
