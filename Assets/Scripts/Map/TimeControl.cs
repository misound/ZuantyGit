using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeControl : MonoBehaviour
{
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private GameObject _timeline;
    [SerializeField] private Collider2D  _canPlay;

    public GameObject LoadingPerson;
    
    bool isPause;
   
    public int sceneIndex;   

    // Start is called before the first frame update
    void Start()
    {
        if (_playableDirector == null) { _playableDirector = GetComponent<PlayableDirector>(); }
        isPause = false;
       
    }
    public void talkPauseTimeline()
    {
        Debug.Log("STOP");
        _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0); 
        isPause = true;

    }
    public void IsEnd()
    {
        Destroy(_timeline);
    }
    public void nextLevel()
    {
        //SceneManager.LoadScene(i);

        StartCoroutine(StartLV1Loading(sceneIndex));
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") )
        {

            _playableDirector.Play();
        }
    }
    public void Activeperson()
    {
        LoadingPerson.SetActive(true);
    }

    private void PlayTalk()
    {
        if (isPause)
        {

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("E");
                _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
                isPause = false;
            }
        }
    }
    
    void Update()
    {
        PlayTalk();
        
    }

    IEnumerator StartLV1Loading(int SceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneIndex);

        
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            yield return null;
        }
        

    }
}
