using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class TimelineTest : MonoBehaviour
{
    public bool isTrigger;

    public GameObject ui_tip;

    public PlayableDirector playableDirector;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {


            playableDirector.Play();
            isTrigger = true;
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            
            
        }
    }

    
    

}
