using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossTimeControl : MonoBehaviour
{
    [SerializeField] private PlayableDirector _playableDirector;
    // Start is called before the first frame update
    void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }
    public void pauseTimeline()
    {
        Debug.Log("STOP");
        _playableDirector.Pause();

    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") )
        {

            _playableDirector.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
