using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class doorControl : MonoBehaviour
{
    public bool elevatorIn;
    public bool canOpen;
    public Transform elevatorEnd;
    public Transform elevatorPoint;
    private Animator elevatorAnim;
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
        if (elevatorPoint.position == elevatorEnd.position)
        {
            elevatorIn = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && elevatorIn)
        {
            canOpen = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && elevatorIn)
        {
            canOpen = false;
        }



    }
    

    void Animation()
    {
        if (canOpen)
        {
            elevatorAnim.SetBool("isOpen", true);

        }
        else
        {
            elevatorAnim.SetBool("isOpen", false);
        }

    }
}
