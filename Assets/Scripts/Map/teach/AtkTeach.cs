using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkTeach : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject teachEnemy;
    [SerializeField] private GameObject timeLine;
    private bool enemyIsDie = false;
    
    void Start()
    {
        timeLine.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(teachEnemy.GetComponent<EnemyBomb>().Die|| teachEnemy.GetComponent<EnemyBomb>().explosioned)
        {
            timeLine.SetActive(true);
            timeLine = null;
            Destroy(this);
        }
    }
}
