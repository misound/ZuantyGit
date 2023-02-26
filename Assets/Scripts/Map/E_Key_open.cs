using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Key_open : MonoBehaviour
{
    [SerializeField] private GameObject plsOpenThis;
    [SerializeField] private Collider2D colliderTriiger;
    [SerializeField] private GameObject teaching;
    private bool playIn = false;
    private bool hasType = true;

    

    // Start is called before the first frame update
    void Start()
    {
        if (teaching == null)
        {
            hasType = false;
           
        }
        plsOpenThis.SetActive(false);
        if (hasType == true)
        {
            teaching.SetActive(false);
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        boolscenes();
    }  
     void boolscenes()
    {
        if (playIn && Input.GetKeyDown(KeyCode.E))
        {
            plsOpenThis.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            if (hasType) 
            {
                teaching.SetActive(true);
            }
            
            playIn = true;
            
        }

    }
    private void OnCollisionExit2D(Collision2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            if (hasType)
            {
                teaching.SetActive(false);
            }
        }
            
    }
}
