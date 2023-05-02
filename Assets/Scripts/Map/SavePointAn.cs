using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SavePointAn : MonoBehaviour
{
    [SerializeField] private bool isSave;
    [SerializeField] private bool onCollider;
    private Animator _anSavePoint;
    // Start is called before the first frame update
    void Start()
    {
        _anSavePoint = gameObject.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Animation();
        if (Input.GetKeyDown(KeyCode.E) && onCollider)
        {
            isSave = true;
        }

    }
    void Animation()
    {
        if (isSave)
        {

            _anSavePoint.SetTrigger("Saving");
            isSave = false;
        }
        if (onCollider)
        {
            _anSavePoint.SetBool("onCollider",true);
        }
        else
        {
            _anSavePoint.SetBool("onCollider", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            onCollider = true;      
        }
       


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            onCollider = false;
        }
    }
}
