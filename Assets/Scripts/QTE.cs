using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QTE : MonoBehaviour
{
    #region ��s��x 
    /// <summary>
    /// 2022/05/27��s��x 
    /// ��s�FQTE���;���
    /// �j�n�������@�}�l�N���͡A�M���onTrigger����}�ҩM����
    /// </summary>
    #endregion
    public GameObject QTE_Q;
    public GameObject QTEBtn;
    public GameObject enemy;
    private float speed = 1f;
    UnityEvent m_MyEvent = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        m_MyEvent.AddListener(QTEBtnActive);
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
            QTEBtnActive();
            //Destroy(QTE_Q);
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
        //Destroy(QTE_Q);
    }
    #endregion

    void QTEBtnActive()
    {
        this.gameObject.SetActive(true);
    }
}
