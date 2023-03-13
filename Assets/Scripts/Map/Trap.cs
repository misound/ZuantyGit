using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Collider2D _col;

    public int TrapDmg = 36;
    // Start is called before the first frame update
    void Start()
    {
        _col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //問碰到陷阱後的演出

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<SpeedPlayerController>() != null)
        {
            other.GetComponent<SpeedPlayerController>().TakeDmg();
        }
    }
}
