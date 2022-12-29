using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WallEnemy : MonoBehaviour
{
    public SpriteRenderer sp;
    public MousePos mousePos;
    public SpeedPlayerController speedPlayerController;

    public bool beChoose;
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
        if (mousePos.onWallEnemy && speedPlayerController.wallEnemyIn)
        {
            sp.color = new Color(1, 0, 0, 1);
        }
        else
        {
            sp.color = new Color(1, 1, 1, 1);
        }
    }

    

    
}
