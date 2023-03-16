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
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<SpeedPlayerController>() != null)
        {
            other.GetComponent<SpeedPlayerController>().InTrap = false;
            Vector2 direction = transform.position - other.GetComponent<SpeedPlayerController>().transform.position;
            other.GetComponent<SpeedPlayerController>().TakeDmg();
            if (other.GetComponent<SpeedPlayerController>().transform.position.x >= transform.position.x)
            {
                other.GetComponent<SpeedPlayerController>()._rb.velocity = new Vector2(-3, 20);
            }
            else if(other.GetComponent<SpeedPlayerController>().transform.position.x <= transform.position.x)
            {
                other.GetComponent<SpeedPlayerController>()._rb.velocity = new Vector2(3, 20);
            }
        }
    }
}
