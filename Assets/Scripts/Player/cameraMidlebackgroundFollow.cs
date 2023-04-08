using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMidlebackgroundFollow : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    [SerializeField] private Vector2 followStange;
    [SerializeField] private GameObject midle;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;

    }

    // Update is called once per frame
    void LateUpdate()
    {
       
            
            Vector3 deltaMaovent = cameraTransform.position - lastCameraPosition;
            midle.transform.position += new Vector3(deltaMaovent.x * followStange.x, deltaMaovent.y * followStange.y);
            lastCameraPosition = cameraTransform.position;
        
        
    }
    
}

