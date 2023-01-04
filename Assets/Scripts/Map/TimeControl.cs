using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class TimeControl : MonoBehaviour
{
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private GameObject _timeline;
    [SerializeField] private Collider2D  _canPlay;
    bool isPause;
   
    public int i;   

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
        SceneManager.LoadScene(i);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") )
        {

            _playableDirector.Play();
        }
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


    // Update is called once per frame
    void Update()
    {
        PlayTalk();
        
    }
}
