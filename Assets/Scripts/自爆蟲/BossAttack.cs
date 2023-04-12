using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    private HealthBar playerhp;

    private int atkk = 14;
    // Start is called before the first frame update
    void Start()
    {
        playerhp = FindObjectOfType<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<SpeedPlayerController>() != null)
        {
            if (col.gameObject.GetComponent<SpeedPlayerController>().transform.position.x
                < transform.position.x)
            {
                col.gameObject.GetComponent<SpeedPlayerController>()._rb
                    .AddForce(new Vector2(-10000f,0),ForceMode2D.Force);
                GameSetting.PlayerHP -= atkk;
                playerhp.SetHealth(GameSetting.PlayerHP);
            }
            else if (col.gameObject.GetComponent<SpeedPlayerController>().transform.position.x
                > transform.position.x)
            {
                col.gameObject.GetComponent<SpeedPlayerController>()._rb
                    .AddForce(new Vector2(10000f,0),ForceMode2D.Force);
                GameSetting.PlayerHP -= atkk;
                playerhp.SetHealth(GameSetting.PlayerHP);
            }            
            
        }
    }
}
