using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleMgr : MonoBehaviour
{
    public int States;

    public Button PBtn;
    public Button OpBtn;
    public Button QBtn;

    public GameObject PlayBtn;
    public GameObject TitleUI;
    public GameObject OptoinUI;
    public GameObject VolumeUI;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        PBtn.onClick.AddListener(Play);
        OpBtn.onClick.AddListener(Play);
        QBtn.onClick.AddListener(Play);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(PlayBtn);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public enum eTitleStates
    {
        Title,
        Option,
        Volume,
    }

    public void UIstates()
    {
        switch (States)
        {
            case (int)eTitleStates.Title:
                break;
            case (int)eTitleStates.Option:
                break;
            case (int)eTitleStates.Volume:
                break;
        }
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Option()
    {

    }
    public void Volume()
    {

    }
}
