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
    public bool moveUp;
    
    public GameObject _eKey;
    public bool elevatorStart;






    void Start()
    {
        transform.position = startingPoint.position;
        moveUp = false;
        playerIn = false;
        _eKey.SetActive(false);
        elevatorStart = false;
    }


    void Update()
    {
        if (transform.position == startingPoint.position&& Input.GetKeyDown(KeyCode.E)&&playerIn)
        {
            GameSetting.SEAudio.Play(AudioMgr.eAudio.SE_Elevator);
           
            StartCoroutine(DelaySwitchOnEnd());
            elevatorStart = true;
        }
        
 
        if (moveUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, endingPoint.position, speed*Time.deltaTime);
            

        } 
        if (transform.position == startingPoint.position && playerIn)
        {
            _eKey.SetActive(true);
        }
        else
        {
            _eKey.SetActive(false);
        }
        if (transform.position == endingPoint.position)
        {
            elevatorStart = false;
            
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

   
    IEnumerator DelaySwitchOnEnd()
    {

        yield return new WaitForSeconds(2);
        moveUp = true;
    }
    
}

    
    

    

