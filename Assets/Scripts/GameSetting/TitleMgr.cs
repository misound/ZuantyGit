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

    public GameObject PlayBtn;
    public GameObject OpfirstBtn;
    public GameObject VofirstBtn;
    public GameObject TitleUI;
    public GameObject OptoinUI;
    public GameObject VolumeUI;

    public Slider TBGMSli;
    public Slider TSESli;

    public GameMgr gameMgr;
    private AudioMgr audioMgr;

    // Start is called before the first frame update
    void Start()
    {
        UIstates(eTitleStates.Title);

        Time.timeScale = 1;
        PBtn.onClick.AddListener(Play);
        OpBtn.onClick.AddListener(Option);
        QBtn.onClick.AddListener(Quit);
        VolumeBtn.onClick.AddListener(Volume);
        OpBackBtn.onClick.AddListener(OptionBack);
        VoBackBtn.onClick.AddListener(VolumeBack);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(PlayBtn);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //audioMgr = FindObjectOfType<AudioMgr>();
        audioMgr = GameSetting.BGMAudio;
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

    public void UIstates(eTitleStates States)
    {
        switch (States)
        {
            case eTitleStates.Title:
                TitleUI.SetActive(true);
                OptoinUI.SetActive(false);
                VolumeUI.SetActive(false);
                break;
            case eTitleStates.Option:
                TitleUI.SetActive(false);
                OptoinUI.SetActive(true);
                VolumeUI.SetActive(false);
                break;
            case eTitleStates.Volume:
                TitleUI.SetActive(false);
                OptoinUI.SetActive(false);
                VolumeUI.SetActive(true);
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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(OpfirstBtn);
    }
    public void OptionBack()
    {
        UIstates(eTitleStates.Title);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(PlayBtn);
    }
    public void Volume()
    {
        UIstates(eTitleStates.Volume);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(VofirstBtn);
    }
    public void VolumeBack()
    {
        UIstates(eTitleStates.Option);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(OpfirstBtn);
    }
    public void Quit()
    {
        Application.Quit();
    }

    private void OnGUI()
    {
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
        }
    }
}
