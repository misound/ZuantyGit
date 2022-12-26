using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WallEnemy : MonoBehaviour
{
    public SpriteRenderer sp;

    public bool beChoose;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnMouseEnter()
    {
        sp.color = new Color(1, 0, 0, 1);
        beChoose = true;

    }
    public void OnMouseExit()
    {
        sp.color = new Color(1, 1, 1, 1);
        beChoose = false;
    }

    
}
