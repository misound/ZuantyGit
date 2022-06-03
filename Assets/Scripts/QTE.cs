using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTE : MonoBehaviour
{
    #region 更新日誌 
    /// 2022/05/27更新日誌 
    /// 更新了QTE產生機制
    /// 大要說明為一開始就產生，然後用onTrigger控制開啟和關閉
    /// 

    /// 2022/05/29更新日誌
    /// 驗證並把QTE按鈕產生方法改在Enemy上面
    /// 決定把QTE當資料庫
    /// 
    /// 
    ///2022/06/03更新日誌
    ///開始編撰資料庫
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
        if (_isDirty) //隨時檢查，更改完了就自我關閉
        {
            if(_qteButton != null)
            {
                Sprite_QTEButton = _qteButton;
            }

            _isDirty = false;
        }
    }
}
