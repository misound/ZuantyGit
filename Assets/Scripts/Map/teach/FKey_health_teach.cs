using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FKey_health_teach : MonoBehaviour
{
    [SerializeField] private GameObject fKey;
    private bool onCollider;
    private bool canHealth;
    // Start is called before the first frame update
    void Start()
    {
        fKey.SetActive(false);
        onCollider = false;
        canHealth = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (onCollider && Input.GetKeyDown(KeyCode.E))
        {
            fKey.SetActive(true);
            canHealth = true;
        }
        if(canHealth && Input.GetKeyDown(KeyCode.F))
        {
            fKey.SetActive(false);
            Destroy(this);
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
