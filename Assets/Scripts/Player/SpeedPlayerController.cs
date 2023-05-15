using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SpeedPlayerController : MonoBehaviour
{
    [Header("Components")] public Rigidbody2D _rb;
    public Animator _anim;

    [Header("Layer Masks")] [SerializeField]
    private LayerMask _groundLayer;

    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _cornerCorrectLayer;
    [SerializeField] public LayerMask _onOneWayPlatformLayerMask;
    [SerializeField] public LayerMask wallEnemyLayer;

    [Header("Movement Variables")] [SerializeField]
    public bool playerDead;

    [SerializeField] public bool Reallydead;
    [SerializeField] private float _movementAcceleration = 70f;
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
    [SerializeField] private float _hangTime = 50f;
    [SerializeField] private float _jumpBufferLength = 0.1f;
    private int _extraJumpsValue;
    private float _hangTimeCounter;
    private float _jumpBufferCounter;
    private bool _canJump => _jumpBufferCounter > 0f && (_hangTimeCounter > 0f || _extraJumpsValue > 0 || _onWall);
    public bool _isJumping = false;

    [Header("Wall Movement Variables")] [SerializeField]
    private float _wallSlideModifier = 0.5f;

    [SerializeField] private float _wallJumpXVelocityHaltDelay = 0.2f;

    private bool _wallGrab => _onWall && !_onGround && Input.GetButton("WallGrab");

    private bool _wallSlide => _onWall && !_onGround && !Input.GetButton("WallGrab") && _rb.velocity.y < 0f;

    [Header("Dash Variables")] [SerializeField]
    private float _dashSpeed = 15f;

    [SerializeField] private float _dashLength = .3f;
    [SerializeField] private float _dashBufferLength = .1f;
    [SerializeField] private float dashCD = 5f;
    private float _dashBufferCounter;
    public bool _isDashing;
    private bool _hasDashed;

    [Header("Dash CD UI")] public Image dashImage;
    public float dashCoolDown = 5;
    public bool canDash;


    [Header("Ground Collision Variables")] 
    [SerializeField] private float _groundRaycastLength;

    [SerializeField] private Vector3 _groundRaycastOffset;
    [SerializeField] public bool _onGround;

    [Header("Wall Collision Variables")]
    [SerializeField] private float _wallRaycastLength;

    public bool _onWall;
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

    [Header("Trap")] [SerializeField] public bool InTrap;
    [SerializeField] public LayerMask _trapLayer;

    [Header("DashAttack")] [SerializeField]
    public bool isKilling;

    [SerializeField] public float airForce = 5f;
    [SerializeField] public bool wallEnemyIn;
    [SerializeField] public bool hasDashL;

    public bool hasDashR;
    //public Raycast raycast;


    [Header("Attack")] [SerializeField] public Transform attackPoint;
    [SerializeField] public LayerMask enemyLayers;
    [SerializeField] public MousePos mousePos;

    [SerializeField] public PlayerAttack playerAttack;


    [Header("Audio")] [SerializeField] public AudioSource Footstep;
    [SerializeField] public bool isRunning;
    private Vector3 M_pos;
    private Vector3 M_Center;
    private Vector3 M_dir;

    private void Start()
    {
        dashImage.fillAmount = 0;
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        Footstep = GetComponent<AudioSource>();
        mousePos = FindObjectOfType<MousePos>();
        playerAttack = FindObjectOfType<PlayerAttack>();
        playerDead = false;
    }

    private void Update()
    {
        
        if (playerDead)
        {
            _anim.SetBool("isGrounded", false);
            _anim.SetBool("isFalling", false);
            _anim.SetFloat("horizontalDirection", 0f);
            _anim.SetFloat("verticalDirection", 0f);
            _anim.SetBool("Dead", true);
            _anim.SetBool("isDashing", false);
        }
        else
        {
            _anim.SetBool("Dead",false);
        }

        if (!canDash)
        {
            dashImage.fillAmount -= 1 / dashCoolDown * Time.deltaTime;
            if (dashImage.fillAmount <= 0)
            {
                dashImage.fillAmount = 0;
                if (_onGround || _onOneWayPlatform || _onWall)
                {
                    canDash = true;
                }
            }
        }

        Step();

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

        Animation();

        if (Input.GetButtonDown("Dash"))
        {
            _dashBufferCounter = _dashBufferLength;
        }
        else
        {
            _dashBufferCounter -= Time.deltaTime;
        }

        M_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        M_Center = transform.position;
        M_dir = M_pos - M_Center;

        //可以改用上面那個 Ex: M_pos.x可變成M_dir.x
        M_pos.x -= M_Center.x;
        M_pos.y -= M_Center.y;

        //Attack
        playerAttack.AttackCount();

        if (Input.GetButtonDown("Fire1") && !playerAttack.recover)
        {
            playerAttack.MeleeAttack();
            if (playerAttack.cooldown <= 1 || playerAttack.cooldown >= 0)
            {
                playerAttack.cooldown = 1f;
            }

            playerAttack.isAttack = true;

            if (playerAttack.combo <= 4 || playerAttack.combo == 0)
            {
                playerAttack.combo += 1;
            }

            if (M_dir.x < 0 && _facingRight)
            {
                Flip();
            }
            else if (M_dir.x > 0 && !_facingRight)
            {
                Flip();
            }
        }

        //Dash
        if (Input.GetButtonDown("Fire2") && playerAttack.canKill && mousePos.onWallEnemy)
        {
            if (M_dir.x < 0 && _facingRight)
            {
                Flip();
                StartCoroutine(KillDash(mousePos.transform.position.x, mousePos.transform.position.y));
                StartCoroutine(MouseDown(mousePos.transform.position.x, mousePos.transform.position.y));
            }
            else if (M_dir.x > 0 && !_facingRight)
            {
                Flip();
                StartCoroutine(KillDash(mousePos.transform.position.x, mousePos.transform.position.y));
                StartCoroutine(MouseDown(mousePos.transform.position.x, mousePos.transform.position.y));
            }
            else
            {
                StartCoroutine(KillDash(mousePos.transform.position.x, mousePos.transform.position.y));
                StartCoroutine(MouseDown(mousePos.transform.position.x, mousePos.transform.position.y));
            }
        }
        else if (Input.GetButtonDown("Fire2") && canDash)
        {
            canDash = false;
            dashImage.fillAmount = 1;
            if (M_dir.x < 0 && _facingRight)
            {
                Flip();
                StartCoroutine(MouseDown(M_dir.x, M_dir.y));
            }
            else if (M_dir.x > 0 && !_facingRight)
            {
                Flip();
                StartCoroutine(MouseDown(M_dir.x, M_dir.y));
            }
            else
            {
                StartCoroutine(MouseDown(M_dir.x, M_dir.y));
            }
        }

        if (hasDashR)
        {
            StartCoroutine(KillDashFallR());
        }
        else if (hasDashL)
        {
            StartCoroutine(KillDashFallL());
        }
    }

    private void FixedUpdate()
    {


        //Step();

        if (GameSetting.PlayerHP <= 0)
        {
            playerDead = true;
            _rb.bodyType = RigidbodyType2D.Static;
        }

        if (GameSetting.PlayerHP > 0)
        {
            playerDead = false;
            Reallydead = false;
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }

        CanBeDropDown();
        CheckCollisions();


        if (_isDashing || isKilling)
        {
            return;
        }

        if (!_isDashing || !isKilling || !playerAttack.recover)
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

            if (playerAttack.recover && _onGround || _onOneWayPlatform && playerAttack.recover)
            {
                _rb.drag = 8000000000;
            }
            else if (_onGround || _onOneWayPlatform)
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
                if (_rb.velocity.y < 0f) _isJumping = false;
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

    #endregion

    #region 動畫相關

    void Animation()
    {
        if (playerAttack.recover)
        {
            _anim.SetBool("isKilling",false);
            _anim.SetBool("isDashing", false);
            _anim.SetBool("isGrounded", false);
            _anim.SetBool("isFalling", false);
            _anim.SetBool("WallGrab", false);
            _anim.SetBool("isJumping", false);
            _anim.SetFloat("horizontalDirection", 0f);
            _anim.SetFloat("verticalDirection", 0f);
        }
        else
        {
            if (_isDashing&&!isKilling)
            {
                _anim.SetBool("isDashing", true);
                _anim.SetBool("isGrounded", false);
                _anim.SetBool("isFalling", false);
                _anim.SetBool("WallGrab", false);
                _anim.SetBool("isJumping", false);
                _anim.SetFloat("horizontalDirection", 0f);
                _anim.SetFloat("verticalDirection", 0f);
            }
            else if (isKilling)
            {
                _anim.SetBool("isKilling", true);
                _anim.SetBool("isGrounded", false);
                _anim.SetBool("isFalling", false);
                _anim.SetBool("WallGrab", false);
                _anim.SetBool("isJumping", false);
                _anim.SetFloat("horizontalDirection", 0f);
                _anim.SetFloat("verticalDirection", 0f);
            
            }
            else
            {
                if (!playerDead)
                {
                    _anim.SetBool("isDashing", false);
                    _anim.SetBool("isKilling",false);

                    if ((_horizontalDirection < 0f && _facingRight || _horizontalDirection > 0f && !_facingRight) &&
                        !_wallGrab && !_wallSlide)
                    {
                        Flip();
                    }

                    if (_onGround || _onOneWayPlatform)
                    {
                        _anim.SetBool("isGrounded", true);
                        _anim.SetBool("isFalling", false);
                        _anim.SetBool("isJumping", false);
                        _anim.SetBool("WallGrab", false);
                        _anim.SetFloat("horizontalDirection", Mathf.Abs(_horizontalDirection));
                    }
                    else
                    {
                        _anim.SetBool("isGrounded", false);
                    }

                    if (_isJumping)
                    {
                        _anim.SetBool("isJumping", true);
                        _anim.SetBool("isGrounded", false);
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
        }

        /*if (playerDead!=true)
        {
            _anim.SetBool("Dead",false);
        }*/
    }

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
        _onWall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1f), Vector2.right,
                      _wallRaycastLength, _wallLayer) ||
                  Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1f), Vector2.left,
                      _wallRaycastLength, _wallLayer);
        _onRightWall = Physics2D.Raycast(transform.position, Vector2.right, _wallRaycastLength, _wallLayer);
        //OneWayPlatform Collisions
        _onOneWayPlatform = Physics2D.Raycast(transform.position + _oneWayRaycastOffset, Vector2.down,
                                _oneWayRaycastLength, _onOneWayPlatformLayerMask) ||
                            Physics2D.Raycast(transform.position - _oneWayRaycastOffset, Vector2.down,
                                _oneWayRaycastLength, _onOneWayPlatformLayerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

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
        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + 1f),
            new Vector3(transform.position.x, transform.position.y + 1f) + Vector3.right * _wallRaycastLength);
        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + 1f),
            new Vector3(transform.position.x, transform.position.y + 1f) + Vector3.left * _wallRaycastLength);
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

    #region 音效

    public void Step()
    {
        Footstep.volume = GameSetting.SEAudio.SE_audioSource.volume;
        if (_horizontalDirection != 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (isRunning)
        {
            if (!Footstep.isPlaying)
            {
                Footstep.Play();
            }
        }
        else
        {
            Footstep.Stop();
        }
    }

    public void Atk01()
    {
        GameSetting.SEAudio.Play(AudioMgr.eAudio.SE_Player_Attack_01);
    }

    public void Atk02()
    {
        GameSetting.SEAudio.Play(AudioMgr.eAudio.SE_Player_Attack_02);
    }

    public void Atk03()
    {
        GameSetting.SEAudio.Play(AudioMgr.eAudio.SE_Player_Attack_03);
    }

    #endregion

    #region 滑鼠移動

    IEnumerator MouseDown(float x, float y)
    {
        float dashStartTime = Time.time;

        _isDashing = true;

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
            if (!_facingRight) dir = new Vector2(M_pos.x, M_pos.y);
            else dir = new Vector2(M_pos.x, M_pos.y);
        }

        while (Time.time < dashStartTime + _dashLength)
        {
            _rb.velocity = dir.normalized * _dashSpeed;
            yield return null;
        }

        _isDashing = false;
    }

    #endregion

    #region 擊殺衝刺

    IEnumerator KillDash(float x, float y)
    {
        float dashStartTime = Time.time;

        canDash = true;
        isKilling = true;
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 0f;
        _rb.drag = 0f;
        Vector2 dir = new Vector2(x - transform.position.x, y - transform.position.y);

        GameSetting.SEAudio.Play(AudioMgr.eAudio.FinishHim);

        while (Time.time < dashStartTime + 0.05)
        {
            _rb.velocity = dir.normalized * airForce;
            yield return null;
        }

        transform.position = new Vector3(mousePos.enemyPos.x, mousePos.enemyPos.y);
        if (dir.x > 0)
        {
            hasDashR = true;
        }
        else if (dir.x < 0)
        {
            hasDashL = true;
        }

        isKilling = false;
    }

    IEnumerator KillDashFallR()
    {
        _rb.drag = 0;
        _rb.gravityScale = 0;
        _rb.velocity = Vector2.right * 2;
        yield return new WaitForSeconds(0.5f);
        hasDashR = false;
    }

    IEnumerator KillDashFallL()
    {
        _rb.drag = 0;
        _rb.gravityScale = 0;
        _rb.velocity = Vector2.left * 2;
        yield return new WaitForSeconds(0.5f);
        hasDashL = false;
    }

    #endregion

    

    /*void dashCDUI()
    {
        if (Input.GetButtonDown("Fire2") && canDash)
        {
            canDash = false;
            dashImage.fillAmount = 1;
        }

        if (!canDash)
        {
            dashImage.fillAmount -= 1 / dashCoolDown * Time.deltaTime;
            if (dashImage.fillAmount <= 0)
            {
                dashImage.fillAmount = 0;
                canDash = true;
            }
        }
    }*/

    public void TakeDmg()
    {
        InTrap = true;
        if (InTrap)
        {
            InTrap = false;
        }

        FindObjectOfType<HealthBar>().SetHealth(GameSetting.PlayerHP -= FindObjectOfType<Trap>().TrapDmg);
        FindObjectOfType<HealthBar>().CameraE(GameSetting.PlayerHP);
    }

    public void TakeDmgFromWalk()
    {
        FindObjectOfType<HealthBar>().SetHealth(GameSetting.PlayerHP -= FindObjectOfType<EnemyWalk>().Atk);
        FindObjectOfType<HealthBar>().CameraE(GameSetting.PlayerHP);
    }


}