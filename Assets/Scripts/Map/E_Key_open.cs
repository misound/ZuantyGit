using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Key_open : MonoBehaviour
{
    [SerializeField] private GameObject plsOpenThis;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private GameObject teaching;
    private bool _playIn = false;

    // Start is called before the first frame update
    void Start()
    {
        plsOpenThis.SetActive(false);
        teaching.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        boolscenes();
    }
     void boolscenes()
    {
        if (_playIn && Input.GetKeyDown(KeyCode.E))
        {
            plsOpenThis.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            teaching.SetActive(true);
            _playIn = true;
            
        }

    }
    private void OnCollisionExit2D(Collision2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            teaching.SetActive(false);
        }
            
    }
}
