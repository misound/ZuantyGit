using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.U2D;
public class LightControlBehaviour : PlayableBehaviour
{
   public SpriteShapeController map = null;
    public SplineControlPoint splineControlPoint;
   public float intensity = 1f;  

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
   {
        if (map != null)
        {
            
             }
   }
}
