using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LoopMap : MonoBehaviour
{
    [SerializeField] private Transform moveGroup;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform reloadPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private PlayableDirector playTimeline;
    [SerializeField] private GameObject moveplan;
    [SerializeField] private GameObject canloopObj;
    [SerializeField] private float speed;
    [SerializeField] private bool reload = true;
    
    [SerializeField] private bool canloop = false;

    // Start is called before the first frame update
    void Start()
    {
        if (playTimeline == null) 
        {
            playTimeline = null; 
        }
        if (moveplan == null|| canloopObj == null)
        {
            moveplan = null;
            canloopObj = null;
        }
    }
    private void Bool()
    {
        if (moveplan.transform.position == canloopObj.transform.position)
        {
            canloop = true;
        }
        
        
        if (canloop)
        {
            if (reload)
            {
                moveGroup.transform.position = Vector3.MoveTowards(moveGroup.transform.position, reloadPoint.position, speed * Time.deltaTime);
                if (moveGroup.transform.position == reloadPoint.transform.position)
                {
                    moveGroup.transform.position = startPoint.transform.position;
                }

            }
            else
            {
                moveGroup.transform.position = Vector3.MoveTowards(moveGroup.transform.position, endPoint.position, speed * Time.deltaTime);
                if (moveGroup.transform.position == endPoint.transform.position)
                {
                    
                    playTimeline.Play();
                    canloop = false;
                }

            }
           
            

        }

    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        Bool();
      
    }
}
