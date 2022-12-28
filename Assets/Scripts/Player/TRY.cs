using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TRY : MonoBehaviour
{
    public bool wallEnemyIn;
    public float range;
    


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,range);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

    }
}
