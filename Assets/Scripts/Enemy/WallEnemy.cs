using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WallEnemy : MonoBehaviour
{
    public SpriteRenderer sp;
    public GameObject aim;
    public MousePos mousePos;
    public SpeedPlayerController speedPlayerController;
    public bool beChoose;


    public GameObject Deadbody;

    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        mousePos = FindObjectOfType<MousePos>();
        speedPlayerController = FindObjectOfType<SpeedPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (mousePos.onWallEnemy && speedPlayerController.wallEnemyIn&& beChoose)
        {
            aim.SetActive(true);
        }
        else
        {
            aim.SetActive(false);
        }*/
        
        if ( speedPlayerController.isKilling&& mousePos.onWallEnemy&&beChoose)
        {
            GameObject temp = Instantiate(Deadbody);
            temp.transform.parent = transform.parent;
            temp.transform.localPosition = transform.localPosition;
            temp.transform.localScale = transform.localScale;
            GameSetting.SEAudio.Play(AudioMgr.eAudio.SE_FlyDie);
            this.gameObject.SetActive(false);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Mouse"))
        {
            beChoose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Mouse"))
        {
            beChoose = false;
        }
    }
}
