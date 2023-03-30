using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BosslLvelShake : MonoBehaviour
{
    // Start is called before the first frame update
    CinemachineImpulseSource impulse;
    void Start()
    {
        impulse = transform.GetComponent<CinemachineImpulseSource>();
        Invoke("shake", 3f);
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    void shake()
    {
        impulse.GenerateImpulse();
    }
}
