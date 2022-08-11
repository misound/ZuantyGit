using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{

    public PlatformEffector2D platformEffector2D;
    
    // Start is called before the first frame update
    void Start()
    {
        platformEffector2D = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDownPlat()
    {
        if (Input.GetButtonDown("Jump" + "s"))
        {
            Debug.Log("往下");
        }
    }
}
