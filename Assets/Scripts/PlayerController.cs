using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    //記得看更新日誌//記得看更新日誌//記得看更新日誌//記得看更新日誌
    #region 更新日誌
    /// 2022/05/17更新日誌
    /// 換為骷弓之後在白色方格下面跳躍時有BUG，那個白色平台到底是何方神聖?!
    /// 新增有時會無法跳躍的問題
    /// 換為骷弓之後collider的頭太大導致會卡在半空中的問題
    ///

    ///2022/05/22更新日誌
    ///把子彈時間跟QTE做結合了但仍有BUG(請參照約第291行)
    ///新增QTE的cs檔
    ///新增還沒有想做開始畫面的意思
    ///還沒新增冷卻時間
    /// 

    /// 2022/5/35更新日誌
    /// 新增冷卻時間(按技能時開始計算冷卻時間)
    /// 
    /// 
    #endregion
    //記得看更新日誌//記得看更新日誌//記得看更新日誌//記得看更新日誌
    [Header("Components")]
    private Rigidbody2D rb;
    public CircleCollider2D QTESlow;
    public Transform Enemy;
    public GameObject Terry;

    [Header("Layer Mask")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask cornerCorrectLayer;


    [Header("Movement Variables")]
    [SerializeField] private float movementAcceleration;
    [SerializeField] private float maxMovementSpeed;
    [SerializeField] private float groundLinearDrag;
    public bool facingRight = false;
    private float horizontalDirection;
    private float verticalDirection;
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
    private bool canJump => jumpBufferCounter > 0f && (hangTimeCounter > 0f || extraJumpValue > 0 || onWall);
    private bool isJumping = false;

    [Header("Ground Collision Variables")]
    [SerializeField] private float groundRaycastLength;
    [SerializeField] private Vector3 groundRaycastOffset;
    private bool onGround;
    
    [Header("Wall Movement Variables")]
    [SerializeField] private float wallSlideModifier = 0.5f;
    [SerializeField] private float wallJumpXVelocityHaltDelay = 0.2f;
    private bool wallGrab => onWall && !onGround && Input.GetButton("WallGrab");
    private bool wallSlide => onWall && !onGround && !Input.GetButton("WallGrab") && rb.velocity.y < 0f;

    [Header("Wall Collision Variables")] 
    [SerializeField] private float wallRaycastLength;
    public bool onWall;
    public bool onRightWall;

    [Header("Dash Variables")]
    [SerializeField] private float dashSpeed= 15f;
    [SerializeField] private float dashLength = 0.3f;
    [SerializeField] private float dashBufferLength = 0.1f;
    private float dashBufferCounter;
    private bool isDashing;
    private bool hasDashed;
    private bool canDash => dashBufferCounter > 0f && !hasDashed;
        
    
    [Header("Corner Correction Variable")]
    [SerializeField] private float topRaycastLength;
    [SerializeField] private Vector3 edgeRaycastOffset;
    [SerializeField] private Vector3 innerRaycastOffset;
    private bool canCornerCorrect;

    [Header("Animation")]
    [SerializeField] public Animator animator;
    [SerializeField] public float HorizontalaMovement;
    
    [Header("SlowMotion")]
    [SerializeField] public float slowdownFactor = 0.05f;
    [SerializeField] public float slowdownLength = 2f;
    [SerializeField] public float cooldownTime = 6.0f;
    [SerializeField] private float timer = 0;
    [SerializeField] private bool isStartTime = false;
    [SerializeField] private bool skillInvalid = false;

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
        verticalDirection = GetInput().y;
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferLength;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Dash"))
        {
            dashBufferCounter = dashBufferLength;
        }
        else
        {
            dashBufferCounter -= Time.deltaTime;
        }
        Animation();
    }
    private void FixedUpdate()
    {
        CheckCollisions();
        SlowMotionBtn();
        if (canDash)
        {
            StartCoroutine(Dash(horizontalDirection, verticalDirection));
        }
        if (!isDashing)
        {
            if (canMove) MoveCharacter();
            else rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(horizontalDirection * maxMovementSpeed, rb.velocity.y)), .5f * Time.deltaTime);
            if (onGround)
            {
                ApplyGroundLinearDrag();
                extraJumpValue = extraJump;
                hangTimeCounter = hangTime;
                hasDashed = false;
            }
            else
            {
                ApplyAirLinearDrag();
                FallMultiplier();
                hangTimeCounter -= Time.fixedDeltaTime;
                if (!onWall || rb.velocity.y < 0f) isJumping = false;
            }
            if (canJump)
            {
                if (onWall && !onGround)
                {
                    if (onRightWall && horizontalDirection > 0f || !onRightWall && horizontalDirection < 0f)
                    {
                        StartCoroutine(NeutralWallJump());
                    }
                    else
                    {
                        WallJump();
                    }
                    Flip();
                }
                else
                {
                    Jump(Vector2.up);
                }
            }
            if (!isJumping)
            {
                if (wallSlide) WallSlide();
                if (wallGrab) WallGrab();
                if (onWall) StickWall();
            }
        }
        if (canCornerCorrect) CornerCorrect(rb.velocity.y);
    }

    #region 讀取數據
   private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 20), "HorizontalaMovement=" + HorizontalaMovement, guiStyle);
        GUI.Label(new Rect(0, 40, 100, 20), "horizontalDirection=" + horizontalDirection, guiStyle);
        GUI.Label(new Rect(0, 80, 100, 20), "movementAcceleration=" + movementAcceleration, guiStyle);
        GUI.Label(new Rect(0, 120, 100, 20), "TimeScale=" + Time.timeScale, guiStyle);
        GUI.Label(new Rect(0, 160, 100, 20), "Timer=" + timer, guiStyle);

    }
    #endregion
    #region 移動數據

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    }
    #endregion
    #region 移動角色
    private void MoveCharacter()
    {
        rb.AddForce(new Vector2(horizontalDirection, 0f) * movementAcceleration);

        if (Mathf.Abs(rb.velocity.x) > maxMovementSpeed)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxMovementSpeed,rb.velocity.y);
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
    #region 落下空氣阻力
    private void FallMultiplier()
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
    #region 跳躍
    private void Jump(Vector2 direction)
    {
        if (!onGround && !onWall)
            extraJumpValue--;

        ApplyAirLinearDrag();
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
        hangTimeCounter = 0f;
        jumpBufferCounter = 0f;
        isJumping = true;
    }
    #endregion
    #region 登牆跳
    private void WallJump()
    {
        Vector2 jumpDirection = onRightWall ? Vector2.left : Vector2.right;
        Jump(Vector2.up + jumpDirection);
    }
    #endregion
    #region 一般跳躍
    IEnumerator NeutralWallJump()
    {
        Vector2 jumpDirection = onRightWall ? Vector2.left : Vector2.right;
        Jump(Vector2.up + jumpDirection);
        yield return new WaitForSeconds(wallJumpXVelocityHaltDelay);
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }
        #endregion
    #region 抓牆
        public void WallGrab()
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            StickWall();
        }
        #endregion
    #region 滑牆
        public void WallSlide()
        {
            rb.velocity = new Vector2(rb.velocity.x, -maxMovementSpeed * wallSlideModifier);
            StickWall();
        }
    
        #endregion
    #region 抓牆程式
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
        #endregion
    #region 翻轉角色
        void Flip()
        {
            facingRight= !facingRight;
            transform.Rotate(0f, 180f, 0f);
        }
        #endregion
    #region Dash協程
        IEnumerator Dash(float x, float y)
        {
            float dashStartTime = Time.time;
            hasDashed = true;
            isDashing = true;
            isJumping = false;

            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            rb.drag = 0f;

            Vector2 dir;
            if (x != 0f || y != 0f) dir = new Vector2(x,y);
            else
            {
                if (facingRight) dir = new Vector2(1f, 0f);
                else dir = new Vector2(-1f, 0f);
            }

            while (Time.time < dashStartTime + dashLength)
            {
                rb.velocity = dir.normalized * dashSpeed;
                yield return null;
            }

            isDashing = false;
        }
        #endregion
    #region 動畫相關
      
    void Animation()
        {
            if (isDashing)
        {
            animator.SetBool("isDashing", true);
            animator.SetBool("isGrounded", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("wallGrab", false);
            animator .SetBool("isJumping", false);
            animator.SetFloat("horizontalDirection", 0f);
            animator.SetFloat("verticalDirection", 0f);
        }
        else
        {
            animator.SetBool("isDashing", false);

            if ((horizontalDirection > 0f && facingRight || horizontalDirection < 0f && !facingRight) && !wallGrab && !wallSlide)
            {
                Flip();
            }
            if (onGround)
            {
                animator.SetBool("isGrounded", true);
                animator.SetBool("isFalling", false);
                    animator.SetBool("wallGrab", false);
                animator.SetFloat("horizontalDirection", Mathf.Abs(horizontalDirection));
            }
            else
            {
                animator.SetBool("isGrounded", false);
            }
            if (isJumping)
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", false);
                animator.SetBool("wallGrab", false);
                animator.SetFloat("verticalDirection", Mathf.Abs(0f));
            }
            else
            {
                animator.SetBool("isJumping", false);

                if (wallGrab || wallSlide)
                {
                    animator.SetBool("wallGrab", true);
                    animator.SetBool("isFalling", false);
                    animator.SetFloat("verticalDirection", 0f);
                }
                else if (rb.velocity.y < 0f)
                {
                    animator.SetBool("isFalling", true);
                    animator.SetBool("wallGrab", false);
                    animator.SetFloat("verticalDirection", 0f);
                }
            }
        }
        }
        #endregion
    #region 修正跳躍
 private void CheckCollisions()  //這殺虫
    {
        onGround = Physics2D.Raycast(transform.position * groundRaycastLength, Vector2.down, groundRaycastLength, groundLayer);
        
        //Corner Collision
        var position = transform.position;
        canCornerCorrect = Physics2D.Raycast(transform.position + edgeRaycastOffset, Vector2.up, topRaycastLength, groundLayer) &&
                           !Physics2D.Raycast(transform.position + innerRaycastOffset, Vector2.up, topRaycastLength, groundLayer) ||
                           Physics2D.Raycast(transform.position - edgeRaycastOffset, Vector2.up, topRaycastLength, groundLayer) &&
                           !Physics2D.Raycast(transform.position - innerRaycastOffset, Vector2.up, topRaycastLength, groundLayer);
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
        Gizmos.DrawLine(position + innerRaycastOffset, position + innerRaycastOffset + Vector3.up * topRaycastLength);
        Gizmos.DrawLine(position - innerRaycastOffset, position - innerRaycastOffset + Vector3.up * topRaycastLength);
        //Corner Distence Check
        Gizmos.DrawLine(position - innerRaycastOffset + Vector3.up * topRaycastLength,
                        position - innerRaycastOffset + Vector3.up * topRaycastLength + Vector3.left * topRaycastLength);
        Gizmos.DrawLine(position + innerRaycastOffset + Vector3.up * topRaycastLength,
                        position + innerRaycastOffset + Vector3.up * topRaycastLength + Vector3.right * topRaycastLength);
        //WallCheck
        Gizmos.DrawLine(transform.position,transform.position+Vector3.right*wallRaycastLength);
        Gizmos.DrawLine(transform.position,transform.position+Vector3.left*wallRaycastLength);
    }

    void CornerCorrect(float Yvelocity)
    {
        //Push player to the right
        RaycastHit2D _hit = Physics2D.Raycast(transform.position - innerRaycastOffset + Vector3.up * topRaycastLength,Vector3.left, topRaycastLength, cornerCorrectLayer);
        if (_hit.collider != null)
        {
            float _newPos = Vector3.Distance(new Vector3(_hit.point.x, transform.position.y, 0f) + Vector3.up * topRaycastLength,
                transform.position - edgeRaycastOffset + Vector3.up * topRaycastLength);
            transform.position = new Vector3(transform.position.x + _newPos, transform.position.y, transform.position.z);
            rb.velocity = new Vector2(rb.velocity.x, Yvelocity);
            return;
        }

        //Push player to the left
        _hit = Physics2D.Raycast(transform.position + innerRaycastOffset + Vector3.up * topRaycastLength, Vector3.right, topRaycastLength, cornerCorrectLayer);
        if (_hit.collider != null)
        {
            float _newPos = Vector3.Distance(new Vector3(_hit.point.x, transform.position.y, 0f) + Vector3.up * topRaycastLength,
                transform.position + edgeRaycastOffset + Vector3.up * topRaycastLength);
            transform.position = new Vector3(transform.position.x - _newPos, transform.position.y, transform.position.z);
            rb.velocity = new Vector2(rb.velocity.x, Yvelocity);
        }
    }
        #endregion

    #region 物件互動相關

    

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
            isStartTime = true;
            skillInvalid = true;
        }
        if (isStartTime)
        {

            if (skillInvalid && timer == 0)
            {
                DoSlowMotion();
            }

            if (timer >= cooldownTime)
            {

                timer = 0;
                isStartTime = false;
                skillInvalid = false;
            }
            else
            {
            timer += Time.unscaledDeltaTime;
            }
        }
    }
    #endregion
}
