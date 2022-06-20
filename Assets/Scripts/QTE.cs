using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTE : MonoBehaviour
{

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
