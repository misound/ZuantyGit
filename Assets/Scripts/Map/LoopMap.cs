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
    private bool reloadPlase = false;
    private bool canPlayTimeline = false;
 
    // Start is called before the first frame update
    void Start()
    {

    }
    private void Bool()
    {
        if (moveGroup.transform == endPoint.transform)
        {
            bool playTimelime = true;
        }
        if (moveGroup.transform == reloadPoint)
        {
            bool reloadplase = true;
        }
        if (reloadPlase)
        {
            moveGroup.transform.position = startPoint.transform.position;
        }
        if (canPlayTimeline)
        {
            playTimeline.Play();
            canPlayTimeline = false;
        }

    }
    private void OnCollisionEnter2D(Collision2D ReloadPoint)
    {
       
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
