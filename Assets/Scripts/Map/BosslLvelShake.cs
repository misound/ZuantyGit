using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BosslLvelShake : MonoBehaviour
{
    // Start is called before the first frame update
    CinemachineImpulseSource impulse;
    [SerializeField] private float shakeLevle;
    [SerializeField] private bool canShake = false;
    void Start()
    {
        impulse = transform.GetComponent<CinemachineImpulseSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canShake)
        {
            shake();
        }
    }
    void shake()
    {
        impulse.GenerateImpulse(shakeLevle);
    }
}
