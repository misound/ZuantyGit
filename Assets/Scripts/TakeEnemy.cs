using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeEnemy : MonoBehaviour
{

    public List<Enemy> Targets;

    public Enemy EnemyTargets;

    public Transform target;

    public float range = 20.0f;
    /*
    public Transform OnGetEnemy()
    {
        //���b�j�M���b�|
        int radius = 1;
        //�@�B�@�B�X�j�j���b�|,�̤j�X�j��100
        while (radius < 100)
        {
            //�y�ήg�u�˴�,�o��b�|radius�̽d�򤺩Ҧ�������
            Collider[] cols = Physics.OverlapSphere(transform.position, radius);
            //�P�_�˴��쪺���󤤦��S��Enemy
            if (cols.Length > 0)
                for (int i = 0; i < cols.Length; i++)
                    if (cols[i].tag.Equals("Enemy"))
                        return cols[i].transform;
            //�S���˴���Enemy,�N�˴��b�|�X�j2��
            radius += 2;
        }
        return null;
    }*/
    // Start is called before the first frame update
    void Start()
    {
        Targets = new List<Enemy>();
        UpdateTargetList();
        showSelectionEffect();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SelectNextTarget();
        }
        //OnGetEnemy();
        UpdateTarget();
    }

    void UpdateTarget()
    {
        float sdis = Mathf.Infinity;
        GameObject nearEnemy = null;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemys in enemies)
        {
           float distoEnemy = Vector3.Distance(transform.position, enemys.transform.position);

            if(distoEnemy < sdis)
            {
                nearEnemy = enemys;
                sdis = distoEnemy;
            }

        }
        if(nearEnemy != null && sdis <= range)
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
        Targets.Clear();
        var gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var obj in gameObjects)
        {
            var enemy = obj.GetComponent<Enemy>();

            if
             (enemy != null)
                Targets.Add(enemy);
        }
        if (EnemyTargets == null)
        {
            EnemyTargets = Targets[0];
        }
    }

    public void SelectNextTarget()
    {
        if(Targets.Count == 0)
        {
            return;
        }
        if(EnemyTargets == null)
        {
            EnemyTargets = Targets[0];
        }
        else
        {
            hideSelectionEffect();
            var index = Targets.IndexOf(EnemyTargets);

            if
             (index < 0 || index == Targets.Count - 1)

            {

                EnemyTargets = Targets[0];

            }
            else
            {
                EnemyTargets = Targets[index + 1];
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
        if(EnemyTargets != null)

            EnemyTargets.GetComponent<Renderer>().material.color = Color.red;
    }
    private void hideSelectionEffect()
    {
        if(EnemyTargets != null)

            EnemyTargets.GetComponent<Renderer>().material.color = Color.white;
    }
}


