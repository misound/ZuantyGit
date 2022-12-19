using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class SpeedPlayerController : MonoBehaviour
{
    [Header("Gun")] 
    [SerializeField] public GameObject bulet;
    [SerializeField] public Transform launchSite;
    
    
    [Header("Components")] public Rigidbody2D _rb;
    public Animator _anim;
    public GameObject Trigger;
    public GameObject Player;

    [Header("Layer Masks")] [SerializeField]
    private LayerMask _groundLayer;

    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _cornerCorrectLayer;
    [SerializeField] public LayerMask _onOneWayPlatformLayerMask;

    [Header("Movement Variables")] [SerializeField]
    private float _movementAcceleration = 70f;

    [SerializeField] private float _maxMoveSpeed = 12f;
    [SerializeField] private float _groundLinearDrag = 7f;
    public float _horizontalDirection;
    private float _verticalDirection;

    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) ||
                                       (_rb.velocity.x < 0f && _horizontalDirection > 0f);

    private bool _facingRight = true;
    private bool _canMove => !_wallGrab;


    [Header("Jump Variables")] [SerializeField]
    private float _jumpForce = 12f;

    [SerializeField] private float _airLinearDrag = 2.5f;
    [SerializeField] private float _fallMultiplier = 8f;
    [SerializeField] private float _lowJumpFallMultiplier = 5f;
    [SerializeField] private float _downMultiplier = 12f;
    [SerializeField] private int _extraJumps = 1;
    [SerializeField] private float _hangTime = 0.1f;
    [SerializeField] private float _jumpBufferLength = 0.1f;
    private int _extraJumpsValue;
    private float _hangTimeCounter;
    private float _jumpBufferCounter;
    private bool _canJump => _jumpBufferCounter > 0f && (_hangTimeCounter > 0f || _extraJumpsValue > 0 || _onWall);
    public bool _isJumping = false;

    [Header("Wall Movement Variables")] [SerializeField]
    private float _wallSlideModifier = 0.5f;
    [SerializeField] private float _wallJumpXVelocityHaltDelay = 0.2f;
    private bool _wallGrab => _onWall && !_onGround && Input.GetButton("WallGrab") ;

    private bool _wallSlide => _onWall && !_onGround && !Input.GetButton("WallGrab") && _rb.velocity.y < 0f ;

    [Header("Dash Variables")]
    [SerializeField] private float _dashSpeed = 15f;
    [SerializeField] private float _dashLength = .3f;
    [SerializeField] private float _dashBufferLength = .1f;
    private float _dashBufferCounter;
    private bool _isDashing;
    private bool _hasDashed;
    private bool _canDash => _dashBufferCounter > 0f && !_hasDashed;
    public bool _isAttack;

    [Header("Ground Collision Variables")] [SerializeField]
    private float _groundRaycastLength;

    [SerializeField] private Vector3 _groundRaycastOffset;
    [SerializeField] public bool _onGround;

    [Header("Wall Collision Variables")] [SerializeField]
    private float _wallRaycastLength;

    private bool _onWall;
    private bool _onRightWall;

    [Header("Corner Correction Variables")] [SerializeField]
    private float _topRaycastLength;
    [SerializeField] private Vector3 _edgeRaycastOffset;
    [SerializeField] private Vector3 _innerRaycastOffset;
    private bool _canCornerCorrect;

    [Header("OneWayPlatform")] [SerializeField]
    private float _oneWayRaycastLength;

    [SerializeField] private Vector3 _oneWayRaycastOffset;
    [SerializeField] public float checkRadius;
    [SerializeField] public bool _onOneWayPlatform;
    [SerializeField] public float DownwardDistance;

    [Header("Respawn")] [SerializeField] public Transform spawnPoint;
    [SerializeField] public bool die;
    [SerializeField] public bool inTrap;
    [SerializeField] public LayerMask _trapLayer;


    public GameObject atteck;
    public Collider2D[] ArrayTriger;
    public Collider2D LAttack;
    public Collider2D RAttack;

    private Vector2 M_pos;
    private Vector2 M_Center;


    float timer = 0; //計時器
    float Atime = 1.0f;  //攻擊中
    bool attacking = false; //判斷是否攻擊中

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        Reborn();

        ArrayTriger = atteck.GetComponents<Collider2D>();
        LAttack = ArrayTriger[0];
        RAttack = ArrayTriger[1];
    }

    private void Update()
    {
        _horizontalDirection = GetInput().x;
        _verticalDirection = GetInput().y;
        if (Input.GetButtonDown("Jump"))
        {
            _jumpBufferCounter = _jumpBufferLength;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
        //Animation();

        if (Input.GetButtonDown("Dash"))
        {
            _dashBufferCounter = _dashBufferLength;
        }
        else
        {
            _dashBufferCounter -= Time.deltaTime;
        }
        M_pos = Input.mousePosition;
        M_Center = Camera.main.WorldToScreenPoint(transform.position);
        M_pos.x = M_pos.x - M_Center.x;
        M_pos.y = M_pos.y - M_Center.y;
        //Debug.Log(M_Center);
    }

    private void FixedUpdate()
    {

        Attack();
        StartCoroutine(MouseDown(_horizontalDirection, _verticalDirection));

        CanBeDropDown();
        CheckCollisions();
        

        if (_canDash) StartCoroutine(Dash(_horizontalDirection, _verticalDirection));

        if (_isDashing)
        {
            return;
        }
        if (!_isDashing)
        {
            if (_canMove)
            {
                MoveCharacter();
            }
            else
            {
                _rb.velocity = Vector2.Lerp(_rb.velocity,
                    (new Vector2(_horizontalDirection * _maxMoveSpeed, _rb.velocity.y)), .5f * Time.deltaTime);
            }

            if (_onGround || _onOneWayPlatform)
            {
                ApplyGroundLinearDrag();
                _extraJumpsValue = _extraJumps;
                _hangTimeCounter = _hangTime;
                _hasDashed = false;
            }
            else
            {
                ApplyAirLinearDrag();
                FallMultiplier();
                _hangTimeCounter -= Time.fixedDeltaTime;
                if (!_onWall || _rb.velocity.y < 0f ) _isJumping = false;
            }

            if (_canJump)
            {
                if (_onWall && !_onGround && !_onOneWayPlatform)
                {
                    if (_onRightWall && _horizontalDirection > 0f || !_onRightWall && _horizontalDirection < 0f)
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

            if (!_isJumping)
            {
                if (_wallSlide) WallSlide();
                if (_wallGrab) WallGrab();
                if (_onWall) StickToWall();
            }
        }

        //轉角
        if (_canCornerCorrect)
        {
            CornerCorrect(_rb.velocity.y);
        }


        //重生
        Respawn();

    }

    #region 腳色移動

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MoveCharacter()
    {
        _rb.AddForce(new Vector2(_horizontalDirection, 0f) * _movementAcceleration);

        if (Mathf.Abs(_rb.velocity.x) > _maxMoveSpeed)
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxMoveSpeed, _rb.velocity.y);
    }


    #endregion

    #region 地面磨擦

    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(_horizontalDirection) < 0.8f || _changingDirection)
        {
            _rb.drag = _groundLinearDrag;
        }
        else
        {
            _rb.drag = 0f;
        }
    }

    #endregion

    #region 空中摩擦力

    private void ApplyAirLinearDrag()
    {
        _rb.drag = _airLinearDrag;
    }

    private void Jump(Vector2 direction)
    {
        if (!_onGround && !_onWall && !_onOneWayPlatform)
            _extraJumpsValue--;

        ApplyAirLinearDrag();
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(direction * _jumpForce, ForceMode2D.Impulse);
        _hangTimeCounter = 0f;
        _jumpBufferCounter = 0f;
        _isJumping = true;
    }

    #endregion

    #region 蹬牆跳

    private void WallJump()
    {
        Vector2 jumpDirection = _onRightWall ? Vector2.left : Vector2.right;
        Jump(Vector2.up + jumpDirection);
    }

    IEnumerator NeutralWallJump()
    {
        Vector2 jumpDirection = _onRightWall ? Vector2.left : Vector2.right;
        Jump(Vector2.up + jumpDirection);
        yield return new WaitForSeconds(_wallJumpXVelocityHaltDelay);
        _rb.velocity = new Vector2(0f, _rb.velocity.y);
    }

    #endregion

    #region 下墜摩擦力

    private void FallMultiplier()
    {
        if (_verticalDirection < 0f)
        {
            _rb.gravityScale = _downMultiplier;
        }
        else
        {
            if (_rb.velocity.y < 0)
            {
                _rb.gravityScale = _fallMultiplier;
            }
            else if (_rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                _rb.gravityScale = _lowJumpFallMultiplier;
            }
            else
            {
                _rb.gravityScale = 1f;
            }
        }
    }

    #endregion

    #region 滑牆抓牆

    void WallGrab()
    {
        _rb.gravityScale = 0f;
        _rb.velocity = Vector2.zero;
    }

    void WallSlide()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, -_maxMoveSpeed * _wallSlideModifier);
    }
    

    void StickToWall()
    {
        //Push player torwards wall
        if (_onRightWall && _horizontalDirection >= 0f)
        {
            _rb.velocity = new Vector2(1f, _rb.velocity.y);
        }
        else if (!_onRightWall && _horizontalDirection <= 0f)
        {
            _rb.velocity = new Vector2(-1f, _rb.velocity.y);
        }

        //Face correct direction
        if (_onRightWall && !_facingRight)
        {
            Flip();
        }
        else if (!_onRightWall && _facingRight)
        {
            Flip();
        }
    }

    #endregion

    #region 腳色翻轉

    void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    IEnumerator Dash(float x, float y)
    {
        float dashStartTime = Time.time;
        _hasDashed = true;
        _isDashing = true;
        _isJumping = false;

        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 0f;
        _rb.drag = 0f;

        Vector2 dir;
        if (x != 0f || y != 0f)
        {
            dir = new Vector2(x,y);
        }
        else
        {
            if (_facingRight) dir = new Vector2(1f, 0f);
            else dir = new Vector2(-1f, 0f);
        }

        while (Time.time < dashStartTime + _dashLength)
        {
            _rb.velocity = dir.normalized * _dashSpeed;
            yield return null;
        }

        _isDashing = false;
    }


    #endregion

    #region 動畫相關
    /*
    void Animation()
    {
        if (_isDashing)
        {
            _anim.SetBool("isAttack", false);
            _anim.SetBool("isDashing",true);
            _anim.SetBool("isGrounded", false);
            _anim.SetBool("isFalling", false);
            _anim.SetBool("WallGrab", false);
            _anim.SetBool("isJumping", false);
            _anim.SetFloat("horizontalDirection", 0f);
            _anim.SetFloat("verticalDirection", 0f);
        }
        else if(_isAttack&&!_isDashing)
        {
            _anim.SetBool("isAttack", true);
            _anim.SetBool("isDashing",false);
            _anim.SetBool("isGrounded", false);
            _anim.SetBool("isFalling", false);
            _anim.SetBool("WallGrab", false);
            _anim.SetBool("isJumping", false);
            _anim.SetFloat("horizontalDirection", 0f);
            _anim.SetFloat("verticalDirection", 0f);
        }
        else
        {
            _anim.SetBool("isDashing", false);
            _anim.SetBool("isAttack",false);

            if ((_horizontalDirection < 0f && _facingRight || _horizontalDirection > 0f && !_facingRight) &&
                !_wallGrab && !_wallSlide)
            {
                Flip();
            }

            if (_onGround || _onOneWayPlatform)
            {
                _anim.SetBool("isGrounded", true);
                _anim.SetBool("isFalling", false);
                _anim.SetBool("WallGrab", false);
                _anim.SetFloat("horizontalDirection", Mathf.Abs(_horizontalDirection));
                _anim.SetBool("isJumping",false);
            }
            else
            {
                _anim.SetBool("isGrounded", false);
            }

            if (_isJumping)
            {
                _anim.SetBool("isJumping", true);
                _anim.SetBool("isFalling", false);
                _anim.SetBool("WallGrab", false);
                _anim.SetFloat("verticalDirection", 0f);
            }
            else
            {
                _anim.SetBool("isJumping", false);

                if (_wallGrab || _wallSlide)
                {
                    _anim.SetBool("WallGrab", true);
                    _anim.SetBool("isFalling", false);
                    _anim.SetFloat("verticalDirection", 0f);
                }
                else if (_rb.velocity.y < 0f)
                {
                    _anim.SetBool("isFalling", true);
                    _anim.SetBool("WallGrab", false);
                    _anim.SetFloat("verticalDirection", 0f);
                }
            }
        }
    }
    */
    #endregion

    #region 防撞角

    void CornerCorrect(float Yvelocity)
    {
        //Push player to the right
        RaycastHit2D _hit = Physics2D.Raycast(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength,
            Vector3.left, _topRaycastLength, _cornerCorrectLayer);
        if (_hit.collider != null)
        {
            float _newPos = Vector3.Distance(
                new Vector3(_hit.point.x, transform.position.y, 0f) + Vector3.up * _topRaycastLength,
                transform.position - _edgeRaycastOffset + Vector3.up * _topRaycastLength);
            transform.position =
                new Vector3(transform.position.x + _newPos, transform.position.y, transform.position.z);
            _rb.velocity = new Vector2(_rb.velocity.x, Yvelocity);
            return;
        }

        //Push player to the left
        _hit = Physics2D.Raycast(transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength,
            Vector3.right, _topRaycastLength, _cornerCorrectLayer);
        if (_hit.collider != null)
        {
            float _newPos = Vector3.Distance(
                new Vector3(_hit.point.x, transform.position.y, 0f) + Vector3.up * _topRaycastLength,
                transform.position + _edgeRaycastOffset + Vector3.up * _topRaycastLength);
            transform.position =
                new Vector3(transform.position.x - _newPos, transform.position.y, transform.position.z);
            _rb.velocity = new Vector2(_rb.velocity.x, Yvelocity);
        }
    }

    private void CheckCollisions()
    {
        //Ground Collisions
        _onGround = Physics2D.Raycast(transform.position + _groundRaycastOffset, Vector2.down, _groundRaycastLength,
                        _groundLayer) ||
                    Physics2D.Raycast(transform.position - _groundRaycastOffset, Vector2.down, _groundRaycastLength,
                        _groundLayer);


        //Corner Collisions
        _canCornerCorrect = Physics2D.Raycast(transform.position + _edgeRaycastOffset, Vector2.up, _topRaycastLength,
                                _cornerCorrectLayer) &&
                            !Physics2D.Raycast(transform.position + _innerRaycastOffset, Vector2.up, _topRaycastLength,
                                _cornerCorrectLayer) ||
                            Physics2D.Raycast(transform.position - _edgeRaycastOffset, Vector2.up, _topRaycastLength,
                                _cornerCorrectLayer) &&
                            !Physics2D.Raycast(transform.position - _innerRaycastOffset, Vector2.up, _topRaycastLength,
                                _cornerCorrectLayer);

        //Wall Collisions
        _onWall = Physics2D.Raycast(transform.position, Vector2.right, _wallRaycastLength, _wallLayer) ||
                  Physics2D.Raycast(transform.position, Vector2.left, _wallRaycastLength, _wallLayer);
        _onRightWall = Physics2D.Raycast(transform.position, Vector2.right, _wallRaycastLength, _wallLayer);
        //OneWayPlatform Collisions
        _onOneWayPlatform = Physics2D.Raycast(transform.position + _oneWayRaycastOffset, Vector2.down,
                                _oneWayRaycastLength, _onOneWayPlatformLayerMask) ||
                            Physics2D.Raycast(transform.position - _oneWayRaycastOffset, Vector2.down,
                                _oneWayRaycastLength, _onOneWayPlatformLayerMask);
        //Trap Collisions
        inTrap = Physics2D.Raycast(transform.position + _groundRaycastOffset, Vector2.down, _groundRaycastLength,
                     _trapLayer) ||
                 Physics2D.Raycast(transform.position - _groundRaycastOffset, Vector2.down, _groundRaycastLength,
                     _trapLayer);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        //Ground Check
        Gizmos.DrawLine(transform.position + _groundRaycastOffset,
            transform.position + _groundRaycastOffset + Vector3.down * _groundRaycastLength);
        Gizmos.DrawLine(transform.position - _groundRaycastOffset,
            transform.position - _groundRaycastOffset + Vector3.down * _groundRaycastLength);

        //OneWayPlatformCheck
        Gizmos.DrawLine(transform.position + _oneWayRaycastOffset,
            transform.position + _oneWayRaycastOffset + Vector3.down * _oneWayRaycastLength);
        Gizmos.DrawLine(transform.position - _oneWayRaycastOffset,
            transform.position - _oneWayRaycastOffset + Vector3.down * _oneWayRaycastLength);
        //Corner Check
        Gizmos.DrawLine(transform.position + _edgeRaycastOffset,
            transform.position + _edgeRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position - _edgeRaycastOffset,
            transform.position - _edgeRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position + _innerRaycastOffset,
            transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position - _innerRaycastOffset,
            transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength);

        //Corner Distance Check
        Gizmos.DrawLine(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength,
            transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength +
            Vector3.left * _topRaycastLength);
        Gizmos.DrawLine(transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength,
            transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength +
            Vector3.right * _topRaycastLength);

        //Wall Check
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * _wallRaycastLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * _wallRaycastLength);
    }

    #endregion

    #region 單向平台下向

    public void CanBeDropDown()
    {
        if (Input.GetKeyDown(KeyCode.S) && _onOneWayPlatform)
        {
            transform.Translate(0, DownwardDistance, 0);

        }
    }

    #endregion


    #region 重生機制

    void Respawn()
    {

        if (inTrap)
        {
            die = true;
        }

        if (die)
        {
            StartCoroutine(Reborn());
        }
        
    }

    IEnumerator Reborn()
    {
        die = false;
        yield return new WaitForSecondsRealtime(0.5f);
        transform.position = spawnPoint.position;
        


    }

    #endregion
    #region 滑鼠移動
    IEnumerator MouseDown(float x,float y)
    {
        float dashStartTime = Time.time;

        if (Input.GetMouseButtonDown(1))
        {
            _rb.velocity = Vector2.zero;
            _rb.gravityScale = 0f;
            _rb.drag = 0f;
            Vector2 dir;
            if (x != 0f || y != 0f)
            {
                dir = new Vector2(x, y);
            }
            else
            {
                if (_facingRight) dir = new Vector2(M_pos.x, M_pos.y);
                else dir = new Vector2(-M_pos.x, M_pos.y);
            }

            while (Time.time < dashStartTime + _dashLength)
            {
                _rb.velocity = dir.normalized * _dashSpeed;
                yield return null;
            }
        }

    }
    #endregion
    #region 打架
    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!attacking)
            {
                if(M_pos.x >= M_Center.x)
                {
                    RAttack.enabled = true;
                    LAttack.enabled = false;
                    attacking = true;
                }
                if (M_pos.x <= M_Center.x)
                {
                    RAttack.enabled = false;
                    LAttack.enabled = true;
                    attacking = true;
                }
            }

        }
        if (attacking)
        {
            timer += Time.deltaTime;
            if(timer >= Atime)
            {
                RAttack.enabled = false;
                LAttack.enabled = false;
                timer = 0;
                attacking = false;
            }
        }

        if(RAttack.gameObject != null)
        {
            if (RAttack.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("hitEnemy");
            }
        }

        //Debug.Log(attacking);
    }
    #endregion
    private void OnTriggerEnter2D(Collider2D att)
    {
        foreach(Collider2D enemy in ArrayTriger)
        {
            //Debug.Log("attack");
        }
    }
}
