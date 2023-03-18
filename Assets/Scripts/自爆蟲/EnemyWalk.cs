using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    [Header("Data")] 
    [SerializeField] public int HP;
    [SerializeField] public int Atk;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    enum Status
    {
        
    }

    void StatusSwitcher(Status status)
    {
        switch (status)
        {
            default:
                break;
        }
    }
}
