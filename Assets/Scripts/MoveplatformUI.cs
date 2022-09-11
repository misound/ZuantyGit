using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveplatformUI : MonoBehaviour
{
    public Collider2D collosion;

    public void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }
}
