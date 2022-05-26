using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QTE : MonoBehaviour
{
    #region 更新日誌 
    /// <summary>
    /// 2022/05/27更新日誌 
    /// 更新了QTE產生機制
    /// 大要說明為一開始就產生，然後用onTrigger控制開啟和關閉
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

    #region QTE碰撞相關

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

    //離開範圍時消除自己
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
