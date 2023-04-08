using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_background_follw : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    [SerializeField]  private Vector2 followStange;
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
        transform.position += new Vector3(deltaMaovent.x * followStange.x, deltaMaovent.y * followStange.y);
        lastCameraPosition = cameraTransform.position;
    }
}
