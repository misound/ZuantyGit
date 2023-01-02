using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MousePos : MonoBehaviour
{
    public Vector3 mosPos;
    public bool onWallEnemy;
    public Vector3 enemyPos;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mosPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);;
        transform.position = new Vector3(mosPos.x,mosPos.y,0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger!=true&& other.CompareTag("WallEnemy"))
        {
            onWallEnemy = true;
            enemyPos = other.transform.position;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger!=true&& other.CompareTag("WallEnemy"))
        {
            onWallEnemy = false; 
        }
    }
}
