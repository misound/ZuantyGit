using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkWallTimeline : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject noice;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (wall.gameObject.transform.GetChild(0).gameObject.GetComponent<AtkWallHandler>().AWBreak)
        {
            noice.SetActive(true);
        }
    }
}
