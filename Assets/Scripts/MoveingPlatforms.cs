using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveingPlatforms : MonoBehaviour

{
    public float speed;
    public Transform startingPoint;
    public Transform  endindPoint;
    

    public bool moveUp;
    public bool readyToGo;
    
   
    
    
    void Start()
    {
        transform.position = startingPoint.position;
        moveUp = true;
    }


    void Update()
    {
        
        if (transform.position == endindPoint.position)
        {
            moveUp = false;
        }
        else if (transform.position == startingPoint.position)
        {
            moveUp = true;
        }
        if (moveUp == false& readyToGo)
        {
            transform.position = Vector3.MoveTowards(transform.position, startingPoint.position, speed*Time.deltaTime);
        }
 
        else if (moveUp&readyToGo)
        {
            transform.position = Vector3.MoveTowards(transform.position, endindPoint.position, speed*Time.deltaTime);
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
        readyToGo = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
        readyToGo = false;
    }
}

    
    

    

