using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[System.Serializable]
public class textTimeline : PlayableAsset
{
    [Header("Text")]
    public ExposedReference<Text> dialog;
    
    [Multiline(3)]
    public string dialogStr;
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<talkText>.Create(graph);

        var talkText = playable.GetBehaviour();
        talkText.dialog = dialog;
        talkText.dialogStr = dialogStr;
        



        return Playable.Create(graph);
    }
}
