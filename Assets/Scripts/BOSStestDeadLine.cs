using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BOSStestDeadLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckPlayerR();
        CheckPlayerL();
    }
    private void CheckPlayerR()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * 3000, 3000, 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Ground"));
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                SceneManager.LoadScene("BOSS");
            }
        }
    }
    private void CheckPlayerL()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left * 3000, 3000, 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Ground"));
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                SceneManager.LoadScene("BOSS");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position,Vector3.left * 3000);
        Gizmos.DrawRay(transform.position,Vector3.right * 3000);
    }
}
