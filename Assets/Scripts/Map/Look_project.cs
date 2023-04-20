using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Look_project : MonoBehaviour
{
    [SerializeField] private Button close;
    [SerializeField] private PlayableDirector _playableDirector;
    
    void Start()
    {
        close.onClick.AddListener(closeProject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void closeProject()
    {
        _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }
    public void Stop()
    {
        _playableDirector.playableGraph.GetRootPlayable(1).SetSpeed(0);
    }
    
}
