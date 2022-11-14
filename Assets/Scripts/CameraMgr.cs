using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : MonoBehaviour
{
    UnityEngine.Rendering.VolumeProfile volumeProfile;
    public TakeEnemy takeEnemy;
    public GameMgr gameMgr;
    [SerializeField] public float pauseblur;
    [SerializeField] public float pastetime;
    // Start is called before the first frame update
    void Start()
    {
        takeEnemy = FindObjectOfType<TakeEnemy>();
        gameMgr = FindObjectOfType<GameMgr>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        VisualEffect();
    }
    private void Update()
    {
        VisualEffectPause();
        if (gameMgr.pausestates > 0)
        {
            pauseblur += (1f / pastetime) * Time.unscaledDeltaTime;
            pauseblur = Mathf.Clamp(pauseblur, 0f, 300f);
        }
        if (gameMgr.pausestates == 0)
        {
            pauseblur = 0f;
        }
        if (pauseblur >= 150f)
        {
            PauseEffect();
        }

    }
    private void VisualEffect()
    {
        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

        // You can leave this variable out of your function, so you can reuse it throughout your class.
        UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration;

        if (!volumeProfile.TryGet(out chromaticAberration)) throw new System.NullReferenceException(nameof(chromaticAberration));


        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        UnityEngine.Rendering.Universal.MotionBlur motionBlur;
        if (!volumeProfile.TryGet(out motionBlur)) throw new System.NullReferenceException(nameof(motionBlur));


       /* if (takeEnemy.slaind)
        {
            chromaticAberration.intensity.Override(0.5f);
            motionBlur.intensity.Override(1f);
        }
        else
        {
            chromaticAberration.intensity.Override(0f);
            motionBlur.intensity.Override(0f);
        }*/
    }
    private void VisualEffectPause()
    {
        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

        UnityEngine.Rendering.Universal.DepthOfField depthOfField;

        if (!volumeProfile.TryGet(out depthOfField))
            throw new System.NullReferenceException(nameof(depthOfField));


        depthOfField.focalLength.Override(pauseblur);
    }
    private void PauseEffect()
    {
        if (gameMgr.pausestates == 1)
        {
            gameMgr.OptionUI.SetActive(false);
            gameMgr.Panal.SetActive(true);
            gameMgr.pauseUI.SetActive(true);
            gameMgr.VolumeUI.SetActive(false);
        }
    }
}
