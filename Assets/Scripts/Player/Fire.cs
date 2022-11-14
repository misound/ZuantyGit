 using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float fireSpeed;
    
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right*fireSpeed*Time.deltaTime,Space.Self);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.transform.name);
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
