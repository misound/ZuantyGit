using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QTE : MonoBehaviour
{
    #region ��s��x 
    /// 2022/05/27��s��x 
    /// ��s�FQTE���;���
    /// �j�n�������@�}�l�N���͡A�M���onTrigger����}�ҩM����
    /// 

    /// 2022/05/29��s��x
    /// ���Ҩç�QTE���s���ͤ�k��bEnemy�W��
    /// �M�w��QTE���Ʈw
    /// 
    #endregion
    public GameObject QTEBtn;
    public GameObject enemy;

    //UnityEvent m_MyEvent = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        //m_MyEvent.AddListener(QTEBtnActive);
    }

    // Update is called once per frame
    void Update()
    {
        QTEBtn.transform.LookAt(enemy.transform.position);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    #region QTE�I������

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Time.timeScale <= 0.4 )
        {
            //QTEBtnActive();

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(gameObject.name + "DIE!!!");
            Destroy(this.gameObject);
        }
    }

    //���}�d��ɮ����ۤv
    private void OnTriggerExit2D(Collider2D collision)
    {

    }
    #endregion
}
