using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level3MeetDoctor : MonoBehaviour
{
   
    // Start is called before the first frame update
    [SerializeField] private GameObject atkWall;
    [SerializeField] private GameObject timeLine;
    

    void Start()
    {
        timeLine.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (atkWall.GetComponent<CanAtkDoor>().Open)
        {
            
            timeLine.SetActive(true);
        }
    }
}

