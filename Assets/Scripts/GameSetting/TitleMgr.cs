using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleMgr : MonoBehaviour
{
    public Button PBtn;
    public Button OpBtn;
    public Button QBtn;

    public Button VolumeBtn;

    public Button OpBackBtn;
    public Button VoBackBtn;
    public Button KeyBackBtn;
    public Button StuffBackBtn;
    

    public Button KeyBtn;
    public Button StuffBtn;

    public GameObject PlayBtn;
    public GameObject TitleUI;
    public GameObject OptoinUI;
    public GameObject VolumeUI;
    public GameObject KeyBoardUI;
    public GameObject StuffUI;

    public Slider TBGMSli;
    public Slider TSESli;

    public GameMgr gameMgr;

    // Start is called before the first frame update
    void Start()
    {
        UIstates(eTitleStates.Title);

        Time.timeScale = 1;
        PBtn.onClick.AddListener(Play);
        OpBtn.onClick.AddListener(Option);
        QBtn.onClick.AddListener(Quit);
        VolumeBtn.onClick.AddListener(Volume);
        KeyBtn.onClick.AddListener(Key);
        StuffBtn.onClick.AddListener(Stuff);
        OpBackBtn.onClick.AddListener(OptionBack);
        VoBackBtn.onClick.AddListener(BackToOption);
        KeyBackBtn.onClick.AddListener(BackToOption);
        StuffBackBtn.onClick.AddListener(BackToOption);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        
    }
    public enum eTitleStates
    {
        Title,
        Option,
        Volume,
        KeyBoard,
        Stuff,
    }

    public void UIstates(eTitleStates States)
    {
        switch (States)
        {
            case eTitleStates.Title:
                TitleUI.SetActive(true);
                OptoinUI.SetActive(false);
                VolumeUI.SetActive(false);
                KeyBoardUI.SetActive(false);
                StuffUI.SetActive(false);
                break;
            case eTitleStates.Option:
                TitleUI.SetActive(false);
                OptoinUI.SetActive(true);
                VolumeUI.SetActive(false);
                KeyBoardUI.SetActive(false);
                StuffUI.SetActive(false);
                break;
            case eTitleStates.Volume:
                TitleUI.SetActive(false);
                OptoinUI.SetActive(false);
                VolumeUI.SetActive(true);
                KeyBoardUI.SetActive(false);
                StuffUI.SetActive(false);
                break;
            case eTitleStates.KeyBoard:
                TitleUI.SetActive(false);
                OptoinUI.SetActive(false);
                VolumeUI.SetActive(false);
                KeyBoardUI.SetActive(true);
                StuffUI.SetActive(false);
                break;
            case eTitleStates.Stuff:
                TitleUI.SetActive(false);
                OptoinUI.SetActive(false);
                VolumeUI.SetActive(false);
                KeyBoardUI.SetActive(false);
                StuffUI.SetActive(true);
                break;
        }
    }
    public void Play()
    {
        SceneManager.LoadScene(5);
    }
    public void Option()
    {
        UIstates(eTitleStates.Option);
    }
    public void OptionBack()
    {
        UIstates(eTitleStates.Title);
    }
    public void Volume()
    {
        UIstates(eTitleStates.Volume);
    }
    public void BackToOption()
    {
        UIstates(eTitleStates.Option);
    }

    public void Key()
    {
        UIstates(eTitleStates.KeyBoard);
    }

    public void Stuff()
    {
        UIstates(eTitleStates.Stuff);
    }
    public void Quit()
    {
        Application.Quit();
    }

    private void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("level_1_testColillder");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene("level_2");
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SceneManager.LoadScene("level_3");
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            SceneManager.LoadScene("BOSS");
        }
        /*
        if (GUI.Button(new Rect(50, 50, 160, 100), "Level1"))
        {
            SceneManager.LoadScene("level_1_testColillder");
        }
        if (GUI.Button(new Rect(50, 150, 160, 100), "Level2"))
        {
            SceneManager.LoadScene("level_2");
        }
        if (GUI.Button(new Rect(50, 250, 160, 100), "Level3"))
        {
            SceneManager.LoadScene("level_3");
        }
        if (GUI.Button(new Rect(50, 350, 160, 100), "Boss關卡"))
        {
            SceneManager.LoadScene("BOSS");
        }*/
    }
}
