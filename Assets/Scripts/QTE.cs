using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTE : MonoBehaviour
{
    public GameObject QTE_Q;
    public PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region QTE碰撞相關
    /// <summary>
    /// 當時間從子彈時間恢復到0.4以上的時候消除自己
    /// 在0.4以下時可以按Q消除自己
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Time.timeScale >= 0.4)
        {
            Destroy(QTE_Q);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Destroy(QTE_Q);
        }
    }

    //離開範圍時消除自己
    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(QTE_Q);
    }
    #endregion
}
