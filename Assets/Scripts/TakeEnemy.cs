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

    public bool slaind = false;

    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        TargetList = new List<Enemy>();
        TempList = new List<Enemy>();
        playerController = FindObjectOfType<PlayerController>();
        //UpdateTarget();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateTargetList();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetupTemp();
            SelectNextTarget();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetupTemp();
            SelectNextTarget();
        }
        if (Time.timeScale >= 0.4)
            HideSelectionEffect();
        if (slaind)
        {
            playerController._anim.SetBool("isAttack", false);
            SetupTemp();
        }
            
        if (playerController.KilllingTime)
        {
            SelectNextTarget();
            playerController.KilllingTime = false;
            
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
            if (distoEnemy > range)
            {
                TempList.Remove(TargetList[i]);
            }
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

            if (enemy != null)
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
            HideSelectionEffect();
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
        ShowSelectionEffect();
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
            HideSelectionEffect();
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
        ShowSelectionEffect();
    }
    public void CancelSelection()
    {
        HideSelectionEffect();
        EnemyTargets = null;
    }

    private void ShowSelectionEffect()
    {
        if (EnemyTargets != null)

            EnemyTargets.GetComponent<Renderer>().material.color = Color.red;
    }
    private void HideSelectionEffect()
    {
        if (EnemyTargets != null)
        {
            EnemyTargets.QTEBtn_I.SetActive(false);
            EnemyTargets.QTEBtn_U.SetActive(false);
            EnemyTargets.QTEBtn_O.SetActive(false);
            EnemyTargets.Trigger.SetActive(false);
            EnemyTargets.GetComponent<Renderer>().material.color = Color.white;
        }

    }
}


