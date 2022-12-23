using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeControl : MonoBehaviour
{
    [SerializeField] private PlayableDirector _playableDirector;
    bool isPause;
    

    // Start is called before the first frame update
    void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        isPause = false;
    }
    public void talkPauseTimeline()
    {
        Debug.Log("STOP");
        _playableDirector.Pause();
        isPause = true;

    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
                _playableDirector.Play();
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
