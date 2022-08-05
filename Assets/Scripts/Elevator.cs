using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    public float speed;
    public Transform[] movePos;

    private int i;
    // Start is called before the first frame update
    void Start()
    {
        i = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, movePos[i].position,speed*Time.deltaTime);
        if(Vector2.Distance(transform.position,movePos[i].position)<0.1f)
        {
            
        }
        
    }
}
