using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillDashTeach : MonoBehaviour
{
    [SerializeField] private EnemyBomb enemy;
    [SerializeField] private GameObject killTeach;
    [SerializeField] private GameObject bombTras; 
    // Start is called before the first frame update
    void Start()
    {
        killTeach.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(enemy.bombExMode)
        {
            killTeach.SetActive(true);
        }
        if (!enemy.bombExMode)
        {
            killTeach.SetActive(false);
        }
        if(enemy.Die)
        {
            killTeach.SetActive(false);
        }
        killTeach.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y);
    }
}
