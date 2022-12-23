using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

// A behaviour that is attached to a playable
public class talkText : PlayableBehaviour
{
    [Header("Text")]
    public ExposedReference<Text> dialog;
    private Text _dialog;
    [Multiline(3)]
    public string dialogStr;
    
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        _dialog = dialog.Resolve(playable.GetGraph().GetResolver());
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        _dialog.gameObject.SetActive(true);
        _dialog.text = dialogStr;
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (_dialog)
        {
            _dialog.gameObject.SetActive(false);
        }
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        
    }
}
