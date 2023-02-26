using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevetorLoopingMap : MonoBehaviour
{
    public float speed;
    public Transform endpoint;
    public Transform elePoint;
    private Vector3 StartPoint;
    // Start is called before the first frame update
    void Start()
    {
        StartPoint = transform.position;
    }
    void ElePlay()
    {
        transform.Translate(translation: Vector3.up * speed * Time.deltaTime);
        if (transform.position.y > 15.30)
        {
            transform.position = StartPoint;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(elePoint.position == endpoint.position)
        {
            ElePlay();
        }
    }
}
