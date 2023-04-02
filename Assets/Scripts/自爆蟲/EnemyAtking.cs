using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtking : MonoBehaviour
{
    [Header("攻擊係數")]
    [SerializeField] public int WalkAtkDmg = 34;
    [SerializeField] public Collider2D col;
    [SerializeField] public EnemyWalk Walk;
    [SerializeField] public HealthBar PlayerHP;
    // Start is called before the first frame update
    void Start()
    {
        //Walk = gameObject.GetComponent<EnemyWalk>();
        PlayerHP = FindObjectOfType<HealthBar>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Walk.Atking)
        {
            col.enabled = Walk.Atking;
        }
        else
        {
            col.enabled = Walk.Atking;
        }
    }

    void Attack()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<SpeedPlayerController>() != null)
        {
            //根據動畫做collider的開關
            other.GetComponent<SpeedPlayerController>().TakeDmgFromWalk();
            //GameSetting.PlayerHP -= WalkAtkDmg;
            //PlayerHP.SetHealth(GameSetting.PlayerHP);
        }
        Debug.Log("被攻擊");
    }
}
