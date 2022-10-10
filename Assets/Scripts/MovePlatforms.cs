using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatforms : MonoBehaviour
{

    public float speed;
    public Transform startingPoint;
    public Transform  endindPoint;
    public bool moveUp;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = startingPoint.position;
        moveUp = false;

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
        if (moveUp == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, startingPoint.position, speed * Time.deltaTime);
            
        }
 
        else if (moveUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, endindPoint.position, speed*Time.deltaTime);
            
        }
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);

    }
}
