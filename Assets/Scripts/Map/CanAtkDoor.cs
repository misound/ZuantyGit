using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanAtkDoor : MonoBehaviour
{
    public GameObject[] DoorBody;
    public int DoorHP;
    public Test playerController;
    float timer = 0f;
    float distimer = 3f;
    bool DisOn = false;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<Test>();

        DoorBody[0].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        DoorBody[1].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        DoorBody[2].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y)) //to do:有攻擊數值計算之後要改血量
        {
            DoorBody[0].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            DoorBody[1].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            DoorBody[2].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            DisOn = true;
        }
        if (DisOn)
        {
            timer += Time.deltaTime;
            if(timer >= distimer)
            {
                DoorBody[0].SetActive(false);
                DoorBody[1].SetActive(false);
                DoorBody[2].SetActive(false);
            }
        }
    }
    private void OnGUI()
    {
        GUIStyle GUIstyle = new GUIStyle();
        GUIstyle.fontSize = 16;
        GUI.Box(new Rect(1820, 80, 80, 50), "按Y破門");
    }
}
