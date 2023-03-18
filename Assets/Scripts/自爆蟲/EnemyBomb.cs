using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{

    [Header("Data")]
    [SerializeField] int HP;
    [SerializeField] int Atk;
    [SerializeField] private GameObject Deadbody;
    
    [Header("Components")]
    [SerializeField] public SpeedPlayerController playerController;
    [SerializeField] public PlayerCombat PlayerAtks;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public HealthBar PlayerHP;

    [Header("Check Points")]
    [SerializeField] public GameObject CheckpointR;
    [SerializeField] public GameObject CheckpointL;

    [Header("Speed")]
    [SerializeField] public float VariableSpeed;
    [SerializeField] public float Patrolspeed;
    [SerializeField] public float Boomspeed;
    [SerializeField] public float MaxBoomspeed;
    [SerializeField] public float Jumpforce;

    [Header("Ray Range")]
    [SerializeField] public float CheckGroundRange;
    [SerializeField] public float CheckPlayerRange;
    [SerializeField] public float Checkwallrange;
    [SerializeField] public float UnstoppableRange;
    [SerializeField] public float WarningRange;

    [SerializeField] private bool mustTurn;
    [SerializeField] private bool mustPatrol;
    [SerializeField] private bool CanJump;
    [SerializeField] private bool _isFacingRight;
    [SerializeField] private bool hitwall;
    [SerializeField] private bool WannaBoom;

    [Header("Animation")]
    [SerializeField] public Animator _anim;

    [Header("Explotion")]
    [SerializeField] public float Boomtime;
    [SerializeField] public float timer;
    [SerializeField] public float BoomRange;
    [SerializeField] public bool explosion;
    [SerializeField] public bool explosionReady;
    [SerializeField] public bool explosioned = false;
    [SerializeField] public float Dietime;
    [SerializeField] public float Dietimer;
    
    [Header("DieStatus")]
    [SerializeField] private bool Onhit;
    [SerializeField] bool Die;

    public LayerMask playerlayer;

    public GameObject[] Particles;

    float distance;
    // Start is called before the first frame update
    void Start()
    {
        SetBombMaxHealth(HP = 100);

        mustPatrol = true;
        playerController = FindObjectOfType<SpeedPlayerController>();
        PlayerHP = FindObjectOfType<HealthBar>();
        PlayerAtks = FindObjectOfType<PlayerCombat>();

        _isFacingRight = true;

        _anim = GetComponent<Animator>();
        explosion = false;

        if (Particles[0].GetComponent<ParticleSystem>().isPlaying == true)
        {
            Particles[0].GetComponent<ParticleSystem>().Stop();
            Particles[1].GetComponent<ParticleSystem>().Stop();
            Particles[2].GetComponent<ParticleSystem>().Stop();
        }
    }

    private void FixedUpdate()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;

        if (Die)
        {
            GameObject temp = Instantiate(Deadbody);
            temp.transform.parent = transform.parent;
            temp.transform.localPosition = transform.localPosition;
            temp.transform.localScale = transform.localScale;
            this.gameObject.SetActive(false);
        }
        Animation();
        if (mustPatrol)
        {
            Patrol();
            _anim.SetBool("isAttack", false);
            _anim.SetBool("explosion", false);
        }
        if (_isFacingRight)
            CheckPlayerR();
        if (!_isFacingRight)
            CheckPlayerL();
        if (WannaBoom)
            Attack();
        if (explosionReady)
            Explooooootion();

        if (explosioned) //自毀
        {
            Dietimer += Time.deltaTime;
            if (Dietimer >= Dietime)
            {
                Destroy(this.gameObject);
                timer = 0;
            }
        }

        if (Onhit)
        {
            
            Boomspeed += (1f / 0.05f) * Time.unscaledDeltaTime;
            Boomspeed = Mathf.Clamp(Boomspeed, 0f, MaxBoomspeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (WannaBoom)
            mustPatrol = false;
        else
            mustPatrol = true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(CheckpointR.transform.position, Vector2.down * CheckGroundRange);
        Gizmos.DrawRay(CheckpointL.transform.position, Vector2.down * CheckGroundRange);
        Gizmos.DrawRay(transform.position - new Vector3(0, 0.05f, 0), Vector2.right * Checkwallrange);
        Gizmos.DrawRay(transform.position - new Vector3(0, 0.05f, 0), Vector2.left * Checkwallrange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, CheckPlayerRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, UnstoppableRange);
        Gizmos.DrawWireSphere(transform.position, BoomRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, WarningRange);
    }

    #region 巡邏
    public void Patrol()
    {
        mustTurn = Physics2D.Raycast(CheckpointR.transform.position, Vector2.down * CheckGroundRange, CheckGroundRange, 1 << LayerMask.NameToLayer("Ground"))
            && !Physics2D.Raycast(CheckpointL.transform.position, Vector2.down * CheckGroundRange, CheckGroundRange, 1 << LayerMask.NameToLayer("Ground"));

        mustTurn = !Physics2D.Raycast(CheckpointR.transform.position, Vector2.down * CheckGroundRange, CheckGroundRange, 1 << LayerMask.NameToLayer("Ground"))
            && Physics2D.Raycast(CheckpointL.transform.position, Vector2.down * CheckGroundRange, CheckGroundRange, 1 << LayerMask.NameToLayer("Ground"));

        hitwall = Physics2D.Raycast(transform.position, Vector2.right * Checkwallrange, Checkwallrange,
                1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("EnemyColliderWall"))
            || Physics2D.Raycast(transform.position, Vector2.left * Checkwallrange, Checkwallrange,
                1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("EnemyColliderWall"));

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
            rb.velocity = new Vector2(Patrolspeed, rb.velocity.y);
            //speed = Mathf.Abs(speed);
        }
        if (!_isFacingRight)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            VariableSpeed = -Patrolspeed;
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
    #region 攻擊玩家
    public void Attack()
    {
        if (explosioned) //以下不動作
        {
            return;
        }
        rb.gravityScale = 4f;
        if (rb.velocity.y != 0f)
            rb.drag = 2.5f;

        CanJump = Physics2D.Raycast(transform.position, Vector2.down * Checkwallrange, Checkwallrange, 1 << LayerMask.NameToLayer("Ground"));
        distance = Vector2.Distance(transform.position, playerController.transform.position);

        if (CanJump && distance - UnstoppableRange < 0.6 && distance - UnstoppableRange > 0.4 && (playerController.transform.position.y + 1f) > transform.position.y)
        {
            rb.velocity = new Vector2(rb.velocity.x, Jumpforce);
            explosionReady = true;
        }

        if (CanJump && distance - UnstoppableRange < 0.6 && distance - UnstoppableRange > 0.4 && (playerController.transform.position.y + 1f) < transform.position.y)
        {
            explosionReady = true;
        }

        if (transform.position.x > playerController.transform.position.x)
        {
            rb.velocity = new Vector2(-Boomspeed, rb.velocity.y);
            if (distance < UnstoppableRange)
            {
                rb.velocity = new Vector2(-Boomspeed * 2, rb.velocity.y);
            }
            _isFacingRight = false;
            transform.localScale = new Vector2(1, transform.localScale.y);
        }

        if (transform.position.x < playerController.transform.position.x)
        {
            rb.velocity = new Vector2(Boomspeed, rb.velocity.y);
            if (distance < UnstoppableRange)
                rb.velocity = new Vector2(Boomspeed * 2, rb.velocity.y);
            _isFacingRight = true;
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }

        if (distance > WarningRange)
        {
            WannaBoom = false;
        }

        if (distance < UnstoppableRange)
        {
            explosionReady = true;
        }
        mustPatrol = false;
    }
    #endregion
    #region 藝術
    private void Explooooootion()
    {
        if (explosionReady) //計時器
        {
            timer += Time.deltaTime;
            if (timer >= Boomtime)
            {
                explosion = true;
                timer = 0;
                explosionReady = false;
            }
        }

        if (explosion) //攻擊範圍和攻擊判定
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, BoomRange, playerlayer);
            explosion = false;
            foreach (Collider2D player in hit)
            {
                GameSetting.PlayerHP -= Atk;
                PlayerHP.SetHealth(GameSetting.PlayerHP);
                break;
            }

            explosioned = true;
        }


        if (explosioned) //發射粒子並讓自爆重停在原地爆炸
        {
            GetComponent<CircleCollider2D>().enabled = false;
            rb.gravityScale = 0f;
            rb.drag = 4 + Time.deltaTime;
            Particles[0].GetComponent<ParticleSystem>().Play();
            Particles[1].GetComponent<ParticleSystem>().Play();
            Particles[2].GetComponent<ParticleSystem>().Play();
        }
    }
    #endregion
    #region 檢查玩家
    private void CheckPlayerR()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * CheckPlayerRange, CheckPlayerRange, 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Ground"));
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                WannaBoom = true;
            }
        }
    }
    private void CheckPlayerL()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left * CheckPlayerRange, CheckPlayerRange, 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Ground"));
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                WannaBoom = true;
            }
        }
    }
    #endregion
    #region 動畫

    public void Animation()
    {
        if (explosioned)
        {
            _anim.SetBool("explosion", true);
            _anim.SetBool("isAttack", true);
        }

        if (mustPatrol)
        {
            _anim.SetBool("isAttack", false);
        }
        else if (WannaBoom)
        {
            _anim.SetBool("isAttack", true);
        }

        if (explosionReady)
        {
            _anim.SetBool("explosionReady", true);
        }
    }

    #endregion
    #region 生命

        public void TakeBombHealth(int Damage)
        {
            HP -= Damage;
            if (HP <= 0)
            {
                HP = 0;
                Die = true;
            }
        }
        public void SetBombMaxHealth(int MaxHeath)
        {
            MaxHeath = 100;
            HP = MaxHeath;
        }
        public void SE() //音效
        {
            GameSetting.SEAudio.PlayBoom();
        }

    #endregion
    #region 被攻擊處理

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag=="Player")
            {
                BeAttack();
            }
        }
    
        void BeAttack()
        {
            rb.AddForce(new Vector2(-10000,10),ForceMode2D.Force);
            Boomspeed = Patrolspeed;
            Onhit = true;
        }

    #endregion

}
