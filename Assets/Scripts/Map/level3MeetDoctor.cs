using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level3MeetDoctor : MonoBehaviour
{
   
    // Start is called before the first frame update
    [SerializeField] private GameObject atkWall;
    [SerializeField] private GameObject timeLine;
    [SerializeField] private bool wallBreak;


    void Start()
    {
        timeLine.SetActive(false);
        wallBreak = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (atkWall.GetComponent<CanAtkDoor>().Open)

        {
            StartCoroutine(DelaySwitchOnEnd());

            
        }
        if (timeLine != null && wallBreak)
        {
            timeLine.SetActive(true);
        }
    }
    IEnumerator DelaySwitchOnEnd()
    {

        yield return new WaitForSeconds(0.5f);
        wallBreak = true;
    }
}

