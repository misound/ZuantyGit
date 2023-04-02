using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyWalk : MonoBehaviour
{
    [Header("Data")] 
    [SerializeField] public int HP;
    [SerializeField] public int Atk;

    [Header("Components")] 
    [SerializeField] public SpeedPlayerController playerController;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public HealthBar PlayerHP;

    [Header("Check Point")] 
    [SerializeField] public Vector3 CheckGround;
    [SerializeField] public GameObject AtkPoint;

    [Header("Speed")] 
    [SerializeField] public float VariableSpeed;
    [SerializeField] public float PatrolSpeed;
    [SerializeField] public float HuntSpeed;

    [Header("Terrain Status")] 
    [SerializeField] private bool mustTurn;
    [SerializeField] private bool mustPatrol;
    [SerializeField] private bool _chase;
    [SerializeField] private bool _isFacingRight;
    [SerializeField] private bool hitwall;
    [SerializeField] public bool Atking;
    
    

    [Header("Ray Range")] 
    [SerializeField] public float CheckGroundRange;
    [SerializeField] public float CheckPlayerRange;
    [SerializeField] public float CheckWallRange;
    [SerializeField] public float AttackRange;
    [SerializeField] public float AttackingRange;
    [SerializeField] public float WarningRange;

    [Header("Layer")] 
    [SerializeField] public LayerMask GroundLayer;
    [SerializeField] public LayerMask WallLayer;
    [SerializeField] public LayerMask PlayerLayer;
    
    [Header("Animation")]
    [SerializeField] public Animator _anim;

    [Header("sec")] 
    [SerializeField] public float AniSecs;

    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = FindObjectOfType<SpeedPlayerController>();
        PlayerHP = FindObjectOfType<HealthBar>();
        _anim = GetComponent<Animator>();

        rb.gravityScale = 12f;
        mustPatrol = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (mustPatrol)
        {
            StatusSwitcher(Status.Patrol);

        }

        if (_chase)
        {
            StatusSwitcher(Status.Warning);

        }

        Animation();
    }

    enum Status
    {
        Patrol,
        Warning,
        Attack,
        Die,
    }

    void StatusSwitcher(Status status)
    {
        switch (status)
        {
            case Status.Patrol:
                mustPatrol = true;
                _chase = false;
                Patrol();
                CheckPlayerR();
                CheckPlayerL();
                break;
            case Status.Warning:
                mustPatrol = false;
                Chase();
                break;
            case Status.Attack:
                _anim.SetBool("walk",false);
                _anim.SetBool("chase",false);
                _anim.SetBool("attack",true);
                StartCoroutine(Attack());
                break;
            default:
                break;
        }
    }

    #region 巡邏

    void Patrol()
    {
        rb.drag = 2.5f;


        mustTurn = !Physics2D.Raycast(transform.position + CheckGround,
                       Vector2.down * CheckGroundRange, CheckGroundRange, GroundLayer)
                   || !Physics2D.Raycast(transform.position - CheckGround,
                       Vector2.down * CheckGroundRange, CheckGroundRange, GroundLayer);


        hitwall = Physics2D.Raycast(transform.position,
                      Vector2.left * CheckWallRange, CheckWallRange, WallLayer) ||
                  Physics2D.Raycast(transform.position,
                      Vector2.right * CheckWallRange, CheckWallRange, WallLayer);

        if (mustTurn)
        {
            if (_isFacingRight)
                FlipToL();
            else
                FlipToR();
        }

        if (hitwall)
        {
            if (_isFacingRight)
                FlipToL();
            else
                FlipToR();
        }

        if (_isFacingRight)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            rb.velocity = new Vector2(PatrolSpeed, rb.velocity.y);
        }

        if (!_isFacingRight)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            VariableSpeed = -PatrolSpeed;
            rb.velocity = new Vector2(VariableSpeed, rb.velocity.y);
        }
        
    }

    #endregion

    #region 角色翻轉

    void FlipToL()
    {
        if (_isFacingRight)
            _isFacingRight = false;

        if (!_isFacingRight)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            VariableSpeed = -VariableSpeed;
        }

        mustTurn = false;
        hitwall = false;
    }

    void FlipToR()
    {
        if (!_isFacingRight)
            _isFacingRight = true;

        if (_isFacingRight)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            VariableSpeed = Mathf.Abs(VariableSpeed);
        }

        mustTurn = false;
        hitwall = false;
    }

    #endregion

    #region 檢查玩家

    void CheckPlayerR()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * CheckPlayerRange
            , CheckPlayerRange, PlayerLayer);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                _isFacingRight = true;
                _chase = true;
                mustPatrol = false;
                StatusSwitcher(Status.Warning);
            }
        }
    }

    void CheckPlayerL()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left * CheckPlayerRange
            , CheckPlayerRange, PlayerLayer);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                _isFacingRight = false;
                _chase = true;
                mustPatrol = false;
                StatusSwitcher(Status.Warning);
            }
        }
    }

    #endregion

    void Chase()
    {
        rb.drag = 2.5f;
        if (transform.position.x < playerController.transform.position.x)
        {
            rb.velocity = new Vector2(HuntSpeed, rb.velocity.y);
            _isFacingRight = true;
            FlipToR();
        }

        if (transform.position.x > playerController.transform.position.x)
        {
            rb.velocity = new Vector2(-HuntSpeed, rb.velocity.y);
            _isFacingRight = false;
            FlipToL();
        }
        
        distance = Vector2.Distance(transform.position, playerController.transform.position);

        if (distance < AttackRange)
        {        
            RaycastHit2D hitL = Physics2D.Raycast(transform.position, Vector2.right * CheckPlayerRange
                , CheckPlayerRange, PlayerLayer);
            if (hitL.collider != null)
            {
                if (hitL.collider.gameObject.CompareTag("Player"))
                {
                    StatusSwitcher(Status.Attack);
                }
            }
        
            RaycastHit2D hitR = Physics2D.Raycast(transform.position, Vector2.left * CheckPlayerRange
                , CheckPlayerRange, PlayerLayer);
            if (hitR.collider != null)
            {
                if (hitR.collider.gameObject.CompareTag("Player"))
                {
                    StatusSwitcher(Status.Attack);
                }
            }

        }

        if (distance > WarningRange)
        {
            StatusSwitcher(Status.Patrol);
        }
    }

    IEnumerator Attack()
    {
        mustPatrol = false;
        _chase = false;
        rb.drag = 100000f;
        yield return new WaitForSeconds(AniSecs);

        distance = Vector2.Distance(transform.position, playerController.transform.position);

        if (transform.position.x < playerController.transform.position.x)
        {
            _isFacingRight = true;
        }

        if (transform.position.x > playerController.transform.position.x)
        {
            _isFacingRight = false;
        }

        if (distance <= WarningRange)
        {
            _chase = true;
        }
        else
        {
            mustPatrol = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<SpeedPlayerController>() != null)
        {
            
            other.GetComponent<SpeedPlayerController>().TakeDmgFromWalk();
            GameSetting.PlayerHP -= Atk;
            PlayerHP.SetHealth(GameSetting.PlayerHP);
        }
        Debug.Log("被攻擊");
    }

    void Animation()
    {
        if (mustPatrol)
        {
            _anim.SetBool("walk",true);
            _anim.SetBool("chase",false);
            _anim.SetBool("attack",false);
        }

        if (_chase)
        {
            _anim.SetBool("chase",true);
            _anim.SetBool("walk",false);
            _anim.SetBool("attack",false);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + CheckGround, Vector2.down * CheckGroundRange);
        Gizmos.DrawRay(transform.position - CheckGround, Vector2.down * CheckGroundRange);
        Gizmos.DrawRay(transform.position - new Vector3(0, 0.05f, 0), Vector2.left * CheckWallRange);
        Gizmos.DrawRay(transform.position - new Vector3(0, 0.05f, 0), Vector2.right * CheckWallRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + new Vector3(0, 0.05f, 0), Vector2.left * CheckPlayerRange);
        Gizmos.DrawRay(transform.position + new Vector3(0, 0.05f, 0), Vector2.right * CheckPlayerRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, WarningRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
    
}