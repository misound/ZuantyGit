using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigElevatorDoorControl : MonoBehaviour
{
    public bool elevatorIn;
    public bool canOpen;
    public Transform elevatorEnd;
    public Transform elevatorStart;
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
        if (elevatorPoint.position == elevatorEnd.position || elevatorPoint.position == elevatorStart.position)
        {
            elevatorIn = true;
        }
        else
        {
            elevatorIn = false;
        }
    }
   
    void Animation()
    {
        if (elevatorIn)
        {
            elevatorAnim.SetBool("isOpen", true);

        }
        else
        {
            elevatorAnim.SetBool("isOpen", false);
        }

    }
}


