using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.U2D;

public class LightControlAsset : PlayableAsset
{
    public ExposedReference<SpriteShapeController> map ;
    
    public float intensity = 1.0f;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<LightControlBehaviour>.Create(graph);

        var lightControlBehaviour = playable.GetBehaviour();
 
        
        return playable;
    }
}

