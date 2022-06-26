using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeEnemy : MonoBehaviour
{

    public List<Enemy> Targets;

    public Enemy EnemyTargets;
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
    }

    public void UpdateTargetList()
    {
        Targets.Clear();
        var gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach
          (var obj in gameObjects)
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


