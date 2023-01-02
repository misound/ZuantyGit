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
        if (mousePos.onWallEnemy && speedPlayerController.wallEnemyIn&& beChoose)
        {
            sp.color = new Color(1, 0, 0, 1);
        }
        else
        {
            sp.color = new Color(1, 1, 1, 1);
        }
        if ( speedPlayerController.isKilling&& mousePos.onWallEnemy&&beChoose)
        {
            gameObject.SetActive(false);
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
