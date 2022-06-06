using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    #region ��s��x
    /// 2022/05/27��s��x
    /// �s�W�F����QTE���s���;���
    /// �쥻���ʺA���͡A�אּ�@�}�l�N�ͦn�\�b�ĤH�W���óz�LonTrigger�ӱ���
    /// ���ͪ�����]�אּ��Enemy��cs�Ӳ��͡C
    /// �ӥB�{�b�u����w��Ĥ@�ӹJ�����ĤH�A���ӷ|�אּ�ĤH����C
    /// �ڲz�Q�O�Q�n�C������(���s�}�ҹC��, ��������, etc...)���O���P����(�w�]3��)
    /// ��boss�@�������o������h�A�ҥH�T�w����C
    /// 

    /// 2022/05/29��s��x
    /// ��QTE���s�@�Ϊ��覡��GetComponent�ܬ��Ĥ@�Ӳ��ͪ��l���O�C
    /// �H�����ͫ��s���覡�i�H���ե�GetSet�Ӽg�ոլ�
    /// ���L�����ʧ@�o�n��hPlayerController���Ӥ���g�bEnemy
    /// 

    /// 2022/05/35��s��x
    /// QTE���s���ͤ覡�אּ��QTE.cs�A�P�ɥi�۩w�q�Ϥ��M���s
    /// ���]�J��FBUG�A���OSetActive�L�k�@�ΦbHierarchy��QTE���s�W�A���OProject���䦳�@��
    /// �٦��N�Otarget�������QTE.cs�MQTEspriteMgr.cs�A���N�O�L�k���Ϥ�
    /// 
    #endregion
    [Header("Objects")]
    [SerializeField] public QTE qte;
    [SerializeField] public QTESpriteMgr qTESpriteMgr;

    [SerializeField] public GameObject QTEBtn;
    [SerializeField] public GameObject pool;

    [SerializeField] public GameObject Player;



    UnityEvent m_MyEvent = new UnityEvent(); //QTE���s�}�������ƥ�Ĳ�o

    [Header("hp")]
    public int maxHealth = 100;

    private int currentHealth;
    // Start is called before the first frame update
    void Start()
    {

        currentHealth = maxHealth;

        m_MyEvent.AddListener(QTEBtnActive);

        GameObject target = Instantiate(QTEBtn, transform.position, transform.rotation); //��Ҥ�QTE���s�ø��H�ؼ�
        target.transform.parent = pool.transform; //��h�����O

        QTE _qte = target.GetComponent<QTE>();
        _qte.QTEButton = qTESpriteMgr.sprites[Random.Range(0, 2)];
        if (GetComponent<QTE>())
        {
            Debug.Log("�̦n���ڰʳ�");
        }


        //target = gameObject.transform.GetChild(0).gameObject;   //�������w�Ĥ@�Ӥl���O
        Player = GameObject.Find("player");         //�������a�����A����Ҽ{���L��n�@�k

    }

    void Update()
    {

    }

    public void TakeDamege(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    { 
        //Die ani,ation
        
        //Disable the enemy
        Debug.Log(gameObject.name+"DIE!!!");
        Destroy(this.gameObject);
    }

    #region �I������
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.timeScale <= 0.4 && m_MyEvent != null)
        {
            m_MyEvent.Invoke();            //Begin the action
            if (Input.GetKeyDown(KeyCode.U))
            {
                Destroy(this.gameObject);
                Player.transform.position = this.gameObject.transform.localPosition;
            }
        }
        else
        {
            QTEBtn.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        QTEBtn.SetActive(false);
        //Debug.Log("gggggggggg");
    }
    #endregion

    /// <summary>
    /// QTE�}�������ƥ�Ĳ�o
    /// </summary>
    void QTEBtnActive()
    {
        QTEBtn.SetActive(true);
        if(QTEBtn.activeInHierarchy == true)
        {
        Debug.Log("�ڬOEnemyĲ�o");
        }

    }
}
