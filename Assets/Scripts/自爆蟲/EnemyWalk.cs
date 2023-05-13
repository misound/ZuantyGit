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
    [SerializeField] private GameObject Deadbody;
    [SerializeField] public GameObject Wounds;
    [SerializeField] private float Wounds_x;
    [SerializeField] private float Wounds_y;

    [Header("Check Point")] 
    [SerializeField] public Vector3 CheckGround;

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
    [SerializeField] public float WarningRange;

    [Header("Layer")] 
    [SerializeField] public LayerMask GroundLayer;
    [SerializeField] public LayerMask WallLayer;
    [SerializeField] public LayerMask PlayerLayer;
    
    [Header("OnhitStatus")]
    [SerializeField] private bool Onhit;
    [SerializeField] public float Knockback;
    [SerializeField] public float KnockbackHeight;
    [SerializeField] public float HitColor;
    [SerializeField] public float HitColorReturn;
    [SerializeField] public bool Die;
    [SerializeField] public bool Locked;
    
    [Header("CoolDown")] 
    [SerializeField] public float AniSecs;
    [SerializeField] public bool CDing;
    
    [Header("Animation")]
    [SerializeField] public Animator _anim;
    
    private float distance;
    public float timer = 0f;

    [Header("ExcutionMode")] 
    [SerializeField] public bool exMode;
    [SerializeField] public GameObject aim;
    [SerializeField] public MousePos mospos;
    [SerializeField] public float exTime = 5;
    [SerializeField] public float startExTime;
    [SerializeField] public bool enemyBeChoose;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = FindObjectOfType<SpeedPlayerController>();
        PlayerHP = FindObjectOfType<HealthBar>();
        _anim = GetComponent<Animator>();
        aim.SetActive(false);

        rb.gravityScale = 12f;
        mustPatrol = true;
        startExTime = 0;
        Locked = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Excution();
        if (mustPatrol)
        {
            StatusSwitcher(Status.Patrol);
        }

        if (_chase)
        {
            StatusSwitcher(Status.Warning);
        }


        if (CDing)
        {
            timer += Time.deltaTime;
            StatusSwitcher(Status.CD);
            if (HP > 0 && HP <= 40)
            {
                timer = 300f;
            }
            //rb.drag = 100000f;
        }

        if (timer >= AniSecs)
        {
            rb.drag = 2.5f;
            timer = 0f;
            CDing = false;
            if (HP >= 19750)
            {
                mustPatrol = true;
            }
        } 
        
        Animation();
    }

    private void FixedUpdate()
    {
        if (Die)
        {
            GameObject temp = Instantiate(Deadbody);
            temp.transform.parent = transform.parent;
            temp.transform.localPosition = transform.localPosition;
            temp.transform.localScale = transform.localScale;
            this.gameObject.SetActive(false);
        }
        
        if (Onhit)
        {

            HitColor += (1f / HitColorReturn) * Time.unscaledDeltaTime;
            HitColor = Mathf.Clamp(HitColor, 0f, 1f);

            Wounds_x -= (1f / HitColorReturn) * Time.unscaledDeltaTime;
            Wounds_x = Mathf.Clamp(Wounds_x, 0f, 1f);

            Wounds_y -= (1f / HitColorReturn) * Time.unscaledDeltaTime;
            Wounds_y = Mathf.Clamp(Wounds_y, 0f, 1f);

            Wounds.transform.localScale = new Vector2(Wounds_x, Wounds_y);

            GetComponent<SpriteRenderer>().color = new Color(1f, HitColor, HitColor, 1f);
        }
        
        if (HP > 0 && HP <=19750)
        {
            StatusSwitcher(Status.FinishHim);
        }


    }

    enum Status
    {
        Patrol,
        Warning,
        Attack,
        CD,
        FinishHim,
    }

    void StatusSwitcher(Status status)
    {
        switch (status)
        {
            case Status.Patrol:
                exMode = false;
                mustPatrol = true;
                _chase = false;
                Patrol();
                CheckPlayerR();
                CheckPlayerL();
                break;
            case Status.Warning:
                exMode = false;
                mustPatrol = false;
                Chase();
                break;
            case Status.Attack:
                _anim.SetBool("walk",false);
                _anim.SetBool("chase",false);
                _anim.SetBool("attack",true);
                _anim.SetBool("CD",false);
                _anim.SetBool("FinishHim",false);
                StartCoroutine(Attack());
                break;
            case Status.CD:
                _anim.SetBool("walk",false);
                _anim.SetBool("chase",false);
                _anim.SetBool("attack",false);
                _anim.SetBool("CD",true);
                _anim.SetBool("FinishHim",false);
                mustPatrol = false;
                _chase = false;
                break;
            case Status.FinishHim:
                _anim.SetBool("walk",false);
                _anim.SetBool("chase",false);
                _anim.SetBool("attack",false);
                _anim.SetBool("CD",false);
                _anim.SetBool("FinishHim",true);
                
                aim.SetActive(true);
                exMode = true;
                _chase = false;
                mustPatrol = false;
                Atking = false;
                rb.drag = 4f;
                startExTime += Time.deltaTime;
            
                if (playerController.isKilling)
                {
                    if (Locked)
                    {
                        Die = true;
                    }   
                }
                if (startExTime > exTime)
                {
                    aim.SetActive(false);
                    startExTime = 0;
                    mustPatrol = true;
                    _anim.SetBool("FinishHim",false);
                    HP = 20000;
                }
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
    #region 追擊

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

    #endregion
    #region 攻擊

        IEnumerator Attack()
        {
            mustPatrol = false;
            _chase = false;
            rb.drag = 4f;
            yield return new WaitForSeconds(0.5f);
    
            
            distance = Vector2.Distance(transform.position, playerController.transform.position);
    
            if (transform.position.x < playerController.transform.position.x)
            {
                _isFacingRight = true;
                CDing = true;
            }
    
            if (transform.position.x > playerController.transform.position.x)
            {
                _isFacingRight = false;
                CDing = true;
            }
    
            /*if (distance <= WarningRange)
            {
                _chase = true;
                Atking = false;
            }
            else
            {
                mustPatrol = true;
                Atking = false;
            }*/
        }

    #endregion
    #region 動畫

        void Animation()
        {
            if (mustPatrol)
            {
                _anim.SetBool("walk",true);
                _anim.SetBool("chase",false);
                _anim.SetBool("attack",false);
                _anim.SetBool("CD",false);
                _anim.SetBool("FinishHim",false);
            }
    
            if (_chase)
            {
                _anim.SetBool("chase",true);
                _anim.SetBool("walk",false);
                _anim.SetBool("attack",false);
                _anim.SetBool("CD",false);
                _anim.SetBool("FinishHim",false);
            }
        }

    #endregion
    #region 生命

    public void TakeWalkHealth(int Damage)
    {
        HP -= Damage;
        if (HP <= 0)
        {
            HP = 0;
            Die = true;
        }
    }

    public void SetWalkMaxHealth(int MaxHeath)
    {
        MaxHeath = 100;
        HP = MaxHeath;
    }

    #endregion
    #region 音效

    public void SE_ATK() //音效
    {
        GameSetting.SEAudio.Play(AudioMgr.eAudio.SE_WalkAtk);
    }
    public void SE_RUN() //音效
    {
        GameSetting.SEAudio.Play(AudioMgr.eAudio.SE_WalkRun);
    }
    public void SE_FIND() //音效
    {
        GameSetting.SEAudio.Play(AudioMgr.eAudio.SE_WalkFind);
    }

    #endregion

    #region 殘血狀態

    public void Excution()
    {
        if (HP > 0 && HP <= 40)
        {
            aim.SetActive(true);
            _chase = false;
            mustPatrol = false;
            Atking = false;
            exMode = true;
            rb.drag = 4f;
            startExTime += Time.deltaTime;
            
            if (playerController.isKilling)
            {
                if (Locked)
                {
                    Die = true;
                }   
            }
        }

        if (startExTime > exTime)
        {
            aim.SetActive(false);
            startExTime = 0;
            exMode = false;
            mustPatrol = true;
            HP = 60;
        }
    }
    

    #endregion
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerAttack>() != null)
        {
            BeAttack();
            HitColor = 0f;
            Wounds_x = 1f;
            Wounds_y = 1f;
        }

        if (other.CompareTag("Mouse"))
        {
            Locked = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Mouse"))
        {
            Locked = false;
        }
    }

    void BeAttack()
    {
        if (transform.position.x < playerController.transform.position.x)
        {
            rb.AddForce(new Vector2(-Knockback, KnockbackHeight), ForceMode2D.Force);
        }

        if (transform.position.x > playerController.transform.position.x)
        {
            rb.AddForce(new Vector2(Knockback, KnockbackHeight), ForceMode2D.Force);
        }
        

        Onhit = true;
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
    

    