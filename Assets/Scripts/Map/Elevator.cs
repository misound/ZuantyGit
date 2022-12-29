using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Elevator : MonoBehaviour

{
    public float speed;
    public Transform startingPoint;
    public Transform  endingPoint;
    public bool playerIn;
    public AudioSource switchOn;
    public bool moveUp;
    public bool oneTime;





    void Start()
    {
        transform.position = startingPoint.position;
        moveUp = false;
        playerIn = false;
    }


    void Update()
    {
        
        if (transform.position == endingPoint.position&& Input.GetKeyDown(KeyCode.E)&&playerIn)
        {
            
            switchOn.Play();
            StartCoroutine(DelaySwitchOnStart());

        }
        else if (transform.position == startingPoint.position&& Input.GetKeyDown(KeyCode.E)&&playerIn)
        {
            switchOn.Play();
            StartCoroutine(DelaySwitchOnEnd());
        }
        if (moveUp == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, startingPoint.position, speed * Time.deltaTime);
            
        }
 
        else if (moveUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, endingPoint.position, speed*Time.deltaTime);
            
        }
        if (transform.position == endingPoint.position && oneTime)
        {

            Destroy(this);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(transform);
            playerIn = true;
        }
        

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            collision.transform.SetParent(null);
            playerIn = false;
        }
        

    }

    IEnumerator DelaySwitchOnStart()
    {
        yield return new WaitForSeconds(2);
        moveUp = false;
    }
    IEnumerator DelaySwitchOnEnd()
    {

        yield return new WaitForSeconds(2);
        moveUp = true;
    }
    
}

    
    

    

