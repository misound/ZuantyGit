using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CameraMgr : MonoBehaviour
{
    UnityEngine.Rendering.VolumeProfile volumeProfile;
    public GameMgr gameMgr;
    public RawImage BlackScreen;
    [SerializeField] public bool Entry;
    [SerializeField] public float _entersecs;
    [SerializeField] public float _entertimer;
    
    [SerializeField] public float pauseblur;
    [SerializeField] public float pastetime;
    
    [SerializeField] public float DeadEffect;
    [SerializeField] public float Deadtime;
    
    [SerializeField] public float Blackscreenalpha;
    [SerializeField] public float Blackscreentime;
    
    [SerializeField] public float Camera_R;
    [SerializeField] public float Camera_G;
    [SerializeField] public float Camera_B;
    
    [SerializeField] public float Fallblur;
    [SerializeField] public float Falltime;
    // Start is called before the first frame update
    private void Awake()
    {
        Blackscreenalpha = 0.99f;
        BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, Blackscreenalpha);
        Entry = true;
    }

    void Start()
    {
        gameMgr = FindObjectOfType<GameMgr>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        VisualEffectDieReturn();
        
        
        if (Entry)
        {
            _entertimer += Time.deltaTime;
            CameraStatusSwitcher(CameraStatus.Start);

            if (_entertimer >= _entersecs)
            {
                _entertimer = 0f;
                CameraStatusSwitcher(default);
                Entry = false;
            }
        }
        else
        {
            if (GameSetting.PlayerHP > 0 && GameSetting.Falling)
            {
                CameraStatusSwitcher(CameraStatus.Fall);
            }

            if (GameSetting.PlayerHP > 0 && !GameSetting.Falling && !GameSetting.Falled)
            {
                CameraStatusSwitcher(default);
            }

            if (GameSetting.PlayerHP <= 0)
            {
                CameraStatusSwitcher(CameraStatus.Dead);
            }
        }
    }
    private void Update()
    {
        VisualEffectPause();
        PauseCamera();
    }

    public enum CameraStatus
    {
        Start,
        Hurt,
        Fall,
        Dead,
    }
    private void CameraStatusSwitcher(CameraStatus cameraStatus)
    {
        switch (cameraStatus)
        {
            case CameraStatus.Start:

                Blackscreenalpha -= (1f / Blackscreentime) * Time.unscaledDeltaTime;
                Blackscreenalpha = Mathf.Clamp(Blackscreenalpha, 0f, 1f);
            
                BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, Blackscreenalpha);

                break;
            
            case CameraStatus.Hurt:
                break;
            
            case CameraStatus.Fall:
                
                if (GameSetting.Falling)
                {
                    VisualEffectFall();
                    Fallblur += (1f / Falltime) * Time.unscaledDeltaTime;
                    Fallblur = Mathf.Clamp(Fallblur, 0f, 20f);
            
            
                    Blackscreenalpha += (1f / Blackscreentime) * Time.unscaledDeltaTime;
                    Blackscreenalpha = Mathf.Clamp(Blackscreenalpha, 0f, 1f);
            
                    BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, Blackscreenalpha);
                }

                if (GameSetting.Falled)
                {
                    VisualEffectFall();
                    Fallblur -= (1f / Falltime) * Time.unscaledDeltaTime * 50;
                    Fallblur = Mathf.Clamp(Fallblur, 0f, 20f);
            
            
                    Blackscreenalpha -= (1f / Blackscreentime) * Time.unscaledDeltaTime * 2;
                    Blackscreenalpha = Mathf.Clamp(Blackscreenalpha, 0f, 1f);
            
                    BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, Blackscreenalpha);
                    if (Blackscreenalpha <= 0)
                    {
                        GameSetting.Falling = false;
                        GameSetting.Falled = false;
                    }
                }
                
                break;
            
            case CameraStatus.Dead:
                
                if (GameSetting.PlayerHP <= 0)
                {
                    Camera_R += (1f / Deadtime) * Time.unscaledDeltaTime;
                    Camera_R = Mathf.Clamp(Camera_R, 0.08410467f, 1f);
            
                    Camera_G -= (1f / Deadtime) * Time.unscaledDeltaTime;
                    Camera_G = Mathf.Clamp(Camera_G, 0f, 0.1320755f);
            
                    Camera_B -= (1f / Deadtime) * Time.unscaledDeltaTime;
                    Camera_B = Mathf.Clamp(Camera_B, 0f, 0.1320755f);
            
                    DeadEffect += (1f / Deadtime) * Time.unscaledDeltaTime;
                    DeadEffect = Mathf.Clamp(DeadEffect, 0.1f, 1f);

                    
                    BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, Blackscreenalpha);
                    

                }
                
                if (DeadEffect >= 1)
                {
                    if (!GameSetting.Falling && !GameSetting.Falled)
                    {
                        Blackscreenalpha += (1f / Blackscreentime) * Time.unscaledDeltaTime;
                        Blackscreenalpha = Mathf.Clamp(Blackscreenalpha, 0f, 1f);
                        
                        BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, Blackscreenalpha);
                    }
                }
                break;
            
            default:
                if (GameSetting.PlayerHP > 0)
                {
                    Camera_R = 0.08410467f;
                    Camera_G = 0.1320755f;
                    Camera_B = 0.1320755f;

            
                    DeadEffect -= (1f / Deadtime) * Time.unscaledDeltaTime;
                    DeadEffect = Mathf.Clamp(DeadEffect, 0.1f, 1f);
            
                    if (!GameSetting.Falling && !GameSetting.Falled)
                    {
                        Blackscreenalpha -= (1f / Blackscreentime) * Time.unscaledDeltaTime * 2;
                        Blackscreenalpha = Mathf.Clamp(Blackscreenalpha, 0f, 1f);
                        BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, Blackscreenalpha);
                    }

                }
                break;
        }
    }

    #region 暫停

    private void PauseCamera()
    {
        if (gameMgr.pausestates > 0)
        {
            pauseblur += (1f / pastetime) * Time.unscaledDeltaTime;
            pauseblur = Mathf.Clamp(pauseblur, 0f, 300f);
        }
        if (gameMgr.pausestates == 0)
        {
            pauseblur = 0f;
        }
        if (pauseblur >= 50f)
        {
            PauseEffect();
        }
    }

    #endregion

    #region 掉落和死亡

    private void HpNFall()
    {



    }

    #endregion

    #region 範例

    private void VisualEffect()
    {
        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) 
            throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

        // You can leave this variable out of your function, so you can reuse it throughout your class.
        UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration;

        if (!volumeProfile.TryGet(out chromaticAberration)) 
            throw new System.NullReferenceException(nameof(chromaticAberration));


        if (!volumeProfile) 
            throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        UnityEngine.Rendering.Universal.MotionBlur motionBlur;
        if (!volumeProfile.TryGet(out motionBlur)) 
            throw new System.NullReferenceException(nameof(motionBlur));
        
    }

    #endregion
    #region 攝影機效果搭載
    private void VisualEffectPause()
    {
        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) 
            throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

        UnityEngine.Rendering.Universal.DepthOfField depthOfField;

        if (!volumeProfile.TryGet(out depthOfField))
            throw new System.NullReferenceException(nameof(depthOfField));


        depthOfField.focalLength.Override(pauseblur);
    }
    
    private void VisualEffectDieReturn()
    {
        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) 
            throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        
        UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration;

        if (!volumeProfile.TryGet(out chromaticAberration)) 
            throw new System.NullReferenceException(nameof(chromaticAberration));


        chromaticAberration.intensity.Override(DeadEffect);
        
        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) 
            throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        
        UnityEngine.Rendering.Universal.Vignette vignette;

        if (!volumeProfile.TryGet(out vignette)) 
            throw new System.NullReferenceException(nameof(vignette));

        vignette.color.value = new Color(Camera_R, Camera_G, Camera_B, 100);

    }
    private void VisualEffectFall()
    {
        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

        UnityEngine.Rendering.Universal.DepthOfField depthOfField;

        if (!volumeProfile.TryGet(out depthOfField))
            throw new System.NullReferenceException(nameof(depthOfField));


        depthOfField.focalLength.Override(Fallblur);
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
    
    private void VisualEffectHurtEffect()
    {
        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) 
            throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        
        UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration;

        if (!volumeProfile.TryGet(out chromaticAberration)) 
            throw new System.NullReferenceException(nameof(chromaticAberration));


        chromaticAberration.intensity.Override(DeadEffect);
        
        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) 
            throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        
        UnityEngine.Rendering.Universal.Vignette vignette;

        if (!volumeProfile.TryGet(out vignette)) 
            throw new System.NullReferenceException(nameof(vignette));

        vignette.color.value = new Color(Camera_R, Camera_G, Camera_B, 100);

    }
    
    #endregion
}
