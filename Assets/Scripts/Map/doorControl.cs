using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class doorControl : MonoBehaviour
{
    public bool elevatorIn;
    public bool canOpen;

    private Animator elevatorAnim;
    [SerializeField]private GameObject ElevatorBase;
    // Start is called before the first frame update
    void Start()
    {
        elevatorIn = false;
        canOpen = false;
        elevatorAnim = GetComponent<Animator>();
         
    }

    // Update is called once per frame
    void Update()
    {
        Animation();
        if (ElevatorBase.GetComponent<Elevator>().elevatorStart)
        {
            elevatorIn = false;
        }
        else
        {
            elevatorIn = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            canOpen = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            canOpen = false;
        }



    }
    

    void Animation()
    {
        if (canOpen&& elevatorIn)
        {
            
            elevatorAnim.SetBool("isOpen", true);

        }
        else
        {
            
            elevatorAnim.SetBool("isOpen", false);
        }

    }
}
