using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    /// 
    ///2022/06/03��s��x
    ///�}�l�s����Ʈw
    #endregion

    public Sprite Sprite_QTEButton;

    private Sprite _qteButton;

    public Sprite QTEButton
    {
        get { return _qteButton; }
        set
        {
            _qteButton = value;
            _isDirty = true;
        }
    }

    private bool _isDirty = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_isDirty) //�H���ˬd�A��粒�F�N�ۧ�����
        {
            if(_qteButton != null)
            {
                Sprite_QTEButton = _qteButton;
            }

            _isDirty = false;
        }
    }
}
