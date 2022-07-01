using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeEnemy : MonoBehaviour
{

    public List<Enemy> TargetList;

    public List<Enemy> TempList;

    public Enemy EnemyTargets;

    public Transform target;

    public float range = 20.0f;


    /*
    public Transform OnGetEnemy()
    {
        //タ穓碝畖
        int radius = 1;
        //˙˙耎穓畖,程耎100
        while (radius < 100)
        {
            //瞴甮絬浪代,眔畖radiusμ絛瞅ず┮Τン
            Collider[] cols = Physics.OverlapSphere(transform.position, radius);
            //耞浪代ンいΤ⊿ΤEnemy
            if (cols.Length > 0)
                for (int i = 0; i < cols.Length; i++)
                    if (cols[i].tag.Equals("Enemy"))
                        return cols[i].transform;
            //⊿Τ浪代Enemy,盢浪代畖耎2μ
            radius += 2;
        }
        return null;
    }*/
    // Start is called before the first frame update
    void Start()
    {
        TargetList = new List<Enemy>();
        TempList = new List<Enemy>();
        UpdateTargetList();
        showSelectionEffect();
        UpdateTarget();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTargetList();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //SetupTemp();
            SelectNextTarget();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetupTemp();
            SelectNextTarget();
        }


        if(Time.timeScale >= 0.9)
        {
            hideSelectionEffect();
        }
    }


    public void SetupTemp()
    {
        TempList.Clear();
        for (int i = 0; i < TargetList.Count; i++)
        {
            float distoEnemy = Vector3.Distance(transform.position, TargetList[i].transform.position);
            if (distoEnemy < range)
            {
                TempList.Add(TargetList[i]);
            }
        }
    }
    void UpdateTarget()
    {
        float sdis = Mathf.Infinity;
        GameObject nearEnemy = null;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemys in enemies)
        {
            float distoEnemy = Vector3.Distance(transform.position, enemys.transform.position);

            if (distoEnemy < sdis)
            {
                nearEnemy = enemys;
                sdis = distoEnemy;
            }

        }
        if (nearEnemy != null && sdis <= range)
        {
            target = nearEnemy.transform;
            target.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            target = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void UpdateTargetList()
    {

        TargetList.Clear();
        var gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var obj in gameObjects)
        {
            var enemy = obj.GetComponent<Enemy>();

            if
             (enemy != null)
                TargetList.Add(enemy);
        }
        if (EnemyTargets == null)
        {
            EnemyTargets = TargetList[0];
        }


    }

    public void SelectNextTarget()
    {
        if (TempList.Count == 0)
        {
            return;
        }
        if (EnemyTargets == null)
        {
            EnemyTargets = TempList[0];
        }
        else
        {
            hideSelectionEffect();
            var index = TempList.IndexOf(EnemyTargets);

            if (index < 0 || index == TempList.Count - 1)
            {
                EnemyTargets = TempList[0];
            }
            else
            {
                EnemyTargets = TempList[index + 1];
            }
        }
        showSelectionEffect();
    }
    public void SelectNextTarget1()
    {
        if (TargetList.Count == 0)
        {
            return;
        }
        if (EnemyTargets == null)
        {
            EnemyTargets = TargetList[0];
        }
        else
        {
            hideSelectionEffect();
            var index = TargetList.IndexOf(EnemyTargets);

            if (index < 0 || index == TargetList.Count - 1)
            {
                EnemyTargets = TargetList[0];
            }
            else
            {
                EnemyTargets = TargetList[index + 1];
            }
        }
        showSelectionEffect();
    }
    public void CancelSelection()
    {
        hideSelectionEffect();
        EnemyTargets = null;
    }

    private void showSelectionEffect()
    {
        if (EnemyTargets != null)

            EnemyTargets.GetComponent<Renderer>().material.color = Color.red;
    }
    private void hideSelectionEffect()
    {
        if (EnemyTargets != null)

            EnemyTargets.GetComponent<Renderer>().material.color = Color.white;
    }
}


