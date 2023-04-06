using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkDoorTeach : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject atkDoorTeach;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (door.GetComponent<CanAtkDoor>().Open)
        {
            atkDoorTeach.SetActive(false);
        }
    }
}
