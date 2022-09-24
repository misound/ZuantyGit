using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveingPlatforms : MonoBehaviour

{
    public float speed;
    public Transform startingPoint;
    public Transform  endindPoint;
    public bool playerIn;
    
    

    public bool moveUp;
    //public bool readyToGo;
    
   
    
    
    void Start()
    {
        transform.position = startingPoint.position;
        moveUp = false;
        playerIn = false;
    }


    void Update()
    {
        
        if (transform.position == endindPoint.position&& Input.GetKeyDown(KeyCode.E)&&playerIn)
        {
            moveUp = false;
        }
        else if (transform.position == startingPoint.position&& Input.GetKeyDown(KeyCode.E)&&playerIn)
        {
            moveUp = true;
        }
        if (moveUp == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, startingPoint.position, speed*Time.deltaTime);
        }
 
        else if (moveUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, endindPoint.position, speed*Time.deltaTime);
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
        playerIn = true;

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
        playerIn = false;

    }
}

    
    

    
