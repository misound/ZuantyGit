using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkTeach : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject AA;
    [SerializeField] private GameObject BB;
    void Start()
    {
        BB.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(AA == null)
        {
            BB.SetActive(true);
        }
    }
}
