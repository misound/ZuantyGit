using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Components")] public Rigidbody2D _rb;
    public Animator _anim;
    public GameObject Trigger;

    [Header("Layer Masks")] [SerializeField]
    private LayerMask _groundLayer;

    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _cornerCorrectLayer;
    [SerializeField] public LayerMask _onOneWayPlatformLayerMask;

    [Header("Movement Variables")] [SerializeField]
    private float _movementAcceleration = 70f;

    [SerializeField] private float _maxMoveSpeed = 12f;
    [SerializeField] private float _groundLinearDrag = 7f;
    private float _horizontalDirection;
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

    [SerializeField] private float _wallRunModifier = 0.85f;
    [SerializeField] private float _wallJumpXVelocityHaltDelay = 0.2f;
    private bool _wallGrab => _onWall && !_onGround && Input.GetButton("WallGrab") && !_wallRun;

    private bool _wallSlide =>
        _onWall && !_onGround && !Input.GetButton("WallGrab") && _rb.velocity.y < 0f && !_wallRun;

    private bool _wallRun => _onWall && _verticalDirection > 0f;

    [Header("Dash Variables")] [SerializeField]
    private float _dashSpeed = 15f;

    [SerializeField] private float _dashLength = 0.3f;
    [SerializeField] private float _dashBufferLength = 0.1f;
    private float _dashBufferCounter;
    public bool _isAttack;

    public bool _hasAttacked;
    private bool _canDash => _dashBufferCounter > 0f && !_hasAttacked;

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

    [Header("SlowMotion")] [SerializeField]
    public float slowdownFactor = 0.05f;

    [SerializeField] public float slowdownLength = 2f;
    [SerializeField] public float cooldownTime = 1.0f;
    [SerializeField] private float timer = 0;
    [SerializeField] private bool isStartTime = false;
    [SerializeField] private bool skillInvalid = false;

    [Header("Killing Spree")] [SerializeField]
    public float MovetoTime;

    [SerializeField] public bool KilllingTime = false;
    [SerializeField] private float KillDash;
    [SerializeField] public float SlowDelay;
    [SerializeField] public bool CanKill = false;
    private TakeEnemy takeEnemy;
    private Vector2 direction;

    [Header("Audio")] [SerializeField] public AudioSource Footstep;
    [SerializeField] public bool isRunning;

    [Header("Respawn")] [SerializeField] public Transform spawnPoint;
    [SerializeField] public bool die;
    [SerializeField] public bool inTrap;
    [SerializeField] public LayerMask _trapLayer;
    [SerializeField] public Animator fadeAnim;
    
    
    

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        takeEnemy = FindObjectOfType<TakeEnemy>();
        Footstep = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _horizontalDirection = GetInput().x;
        _verticalDirection = GetInput().y;
        if (Input.GetButtonDown("Jump")) _jumpBufferCounter = _jumpBufferLength;
        else _jumpBufferCounter -= Time.deltaTime;
        if (Input.GetButtonDown("Dash")) _dashBufferCounter = _dashBufferLength;
        else _dashBufferCounter -= Time.deltaTime;
        Animation();
    }

    private void FixedUpdate()
    {
        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        TriggerActive();


        if (takeEnemy.slaind == true)
        {
            KillingSpree();
            _anim.SetBool("isAttack", true);
        }

        CheckTerrain();
        CanBeDropDown();
        CheckCollisions();
        SlowMotionBtn();

        if (_canDash)
        {
            StartCoroutine(Dash(_horizontalDirection, _verticalDirection));
        }
        if (!_isAttack)
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
                _hasAttacked = false;
            }
            else
            {
                ApplyAirLinearDrag();
                FallMultiplier();
                _hangTimeCounter -= Time.fixedDeltaTime;
                if (!_onWall || _rb.velocity.y < 0f || _wallRun) _isJumping = false;
            }

            if (_canJump)
            {
                if (_onWall && !_onGround && !_onOneWayPlatform)
                {
                    if (!_wallRun && (_onRightWall && _horizontalDirection > 0f ||
                                      !_onRightWall && _horizontalDirection < 0f))
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
                if (_wallRun) WallRun();
                if (_onWall) StickToWall();
            }
        }

        //轉角
        if (_canCornerCorrect)
        {
            CornerCorrect(_rb.velocity.y);
        }

        //聲音
        Sound();
        //重生
        Respawn();

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, takeEnemy.range);
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

    void WallRun()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _verticalDirection * _maxMoveSpeed * _wallRunModifier);
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
        _hasAttacked = true;
        _isAttack = true;
        _isJumping = false;

        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 0f;
        _rb.drag = 0f;

        Vector2 dir;
        if (x != 0f || y != 0f) dir = new Vector2(x, y);
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

        _isAttack = false;
    }

    #endregion

    #region 動畫相關

    void Animation()
    {
        if (_isAttack)
        {
            _anim.SetBool("isAttack", true);
            _anim.SetBool("isGrounded", false);
            _anim.SetBool("isFalling", false);
            _anim.SetBool("WallGrab", false);
            _anim.SetBool("isJumping", false);
            _anim.SetFloat("horizontalDirection", 0f);
            _anim.SetFloat("verticalDirection", 0f);
        }
        else
        {
            _anim.SetBool("isAttack", false);

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

                if (_wallRun)
                {
                    _anim.SetBool("isFalling", false);
                    _anim.SetBool("WallGrab", false);
                    _anim.SetFloat("verticalDirection", Mathf.Abs(_verticalDirection));
                }
            }
        }
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

    #region 時間相關

    void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    void SlowMotionBtn()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) /*|| Input.GetButtonDown("Fire1")*/)
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
                timer += Time.deltaTime;
            }
        }
    }

    void TriggerActive()
    {
        if (Time.timeScale > 0.4)
        {
            Trigger.SetActive(false);
        }

        if (Time.timeScale < 0.4)
        {
            Trigger.SetActive(true);
        }
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

    #region 擊殺衝刺

    public void KillingSpree()
    {
        float distoEnemy = Vector3.Distance(transform.position, takeEnemy.EnemyTargets.transform.position);
        Vector2 direction = takeEnemy.EnemyTargets.transform.position - transform.position;
        if (takeEnemy.slaind && distoEnemy > 0.2f)
        {
            _rb.velocity = Vector2.zero;
            _rb.gravityScale = 0f;
            _rb.drag = 0f;
            _rb.AddForceAtPosition(direction * MovetoTime, takeEnemy.EnemyTargets.transform.position);
        }

        if (takeEnemy.slaind && distoEnemy <= 0.2f)
        {
            KilllingTime = true;
            Invoke("DoSlowMotion", SlowDelay);
            _rb.AddForceAtPosition(direction * KillDash, takeEnemy.EnemyTargets.transform.position);
            takeEnemy.slaind = false;
            _isAttack = false;
        }
    }

    void CheckTerrain()
    {
        if (takeEnemy.EnemyTargets)
        {
            Vector2 direction = takeEnemy.EnemyTargets.transform.position - transform.position;
            Ray2D MyRay = new Ray2D(transform.position, direction);
            RaycastHit2D info = Physics2D.Raycast(transform.position, direction, takeEnemy.range,
                1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Wall"));
            Debug.DrawRay(transform.position, direction, color: Color.cyan);
            if (info.collider != null)
            {
                if (info.collider.gameObject.CompareTag("Untagged"))
                {
                    CanKill = false;
                }
            }
            else
                CanKill = true;
        }
    }

    #endregion

    #region 音效

    public void Sound()
    {
        if (_horizontalDirection != 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (_onGround && isRunning)
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
        fadeAnim.SetBool("FadeOut",true);
        yield return new WaitForSeconds(1);
        transform.position = spawnPoint.position;
        fadeAnim.SetBool("FadeOut",false);

    }
    
    #endregion
}
