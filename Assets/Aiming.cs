using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    public SpeedPlayerController speedPlayerController;
    public MousePos mousePos;
    public WallEnemy wallEnemy;

    public bool inRange;
    public Animator _anim;
    
    // Start is called before the first frame update
    void Start()
    {
        wallEnemy = FindObjectOfType<WallEnemy>();
        mousePos = FindObjectOfType<MousePos>();
        speedPlayerController = FindObjectOfType<SpeedPlayerController>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        TargetAim();
        
    }

    public void TargetAim()
    {
        if (inRange)
        {
            _anim.SetBool("InRange",true);
            if (mousePos.onWallEnemy && wallEnemy.beChoose)
            {
                _anim.SetBool("Aim", true);
                
            }
            else
            {
                _anim.SetBool("Aim",false);
            }
        }
        else
        {
            _anim.SetBool("InRange",false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger != true && other.CompareTag("Player"))
        {
            inRange = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger!= true&&other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
