using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public int i;

    public bool playerIn;
    // Start is called before the first frame update

    // Update is called once per frame
    private void Start()
    {
        playerIn = false;
    }

    void Update()
    {
        if (playerIn)
        {
            SceneManager.LoadScene(i);
        }
    }
    public void OnCollisionEnter2D(Collision2D col)
    {
        playerIn = true;
    }
    
}
