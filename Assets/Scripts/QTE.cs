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

    #region QTE�I������
    /// <summary>
    /// ��ɶ��q�l�u�ɶ���_��0.4�H�W���ɭԮ����ۤv
    /// �b0.4�H�U�ɥi�H��Q�����ۤv
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

    //���}�d��ɮ����ۤv
    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(QTE_Q);
    }
    #endregion
}
