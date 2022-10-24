using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : MonoBehaviour
{
    UnityEngine.Rendering.VolumeProfile volumeProfile;
    public TakeEnemy takeEnemy;
    // Start is called before the first frame update
    void Start()
    {
        takeEnemy = FindObjectOfType<TakeEnemy>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        VisualEffect();
    }
    private void VisualEffect()
    {
        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

        // You can leave this variable out of your function, so you can reuse it throughout your class.
        UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration;

        if (!volumeProfile.TryGet(out chromaticAberration)) throw new System.NullReferenceException(nameof(chromaticAberration));
        if (takeEnemy.slaind)
            chromaticAberration.intensity.Override(0.5f);
        else
            chromaticAberration.intensity.Override(0f);
    }
}
