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
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        elevatorIn = false;
        canOpen = false;
        elevatorAnim = GetComponent<Animator>();
        player = GameObject.Find("player");    }

    // Update is called once per frame
    void Update()
    {
        Animation();
        if (elevatorPoint.position == elevatorEnd.position)
        {
            elevatorIn = true;
        }
        else
        {
            elevatorIn = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player)
        {
            canOpen = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (player)
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
