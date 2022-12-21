using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTest : MonoBehaviour
{
    public Rigidbody2D rb;

    public float dashSpeed;

    public float startDashTime;
    public bool isDashing;

    public float dashTime;

    private Vector3 dashDirection;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        dashDirection = Camera.main.WorldToScreenPoint(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0)&&!isDashing)
        {
            
            rb.velocity = dashDirection * dashSpeed;
            Debug.Log("DASH");
            isDashing = true;
        }
        else if(isDashing)
        {
            if (dashTime<=0)
            {
                dashTime = startDashTime;
                dashDirection = new Vector2(0,0);
                isDashing = false;
                
            }
            else
            {
                dashTime -= Time.deltaTime;
                isDashing = true;
            }
            
        }
        
        
    }

    
    
}
