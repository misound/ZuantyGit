using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    public Animator _anim;

    // Update is called once per frame
    void Update()
    {
        GetComponent<OldPlayerController>();
    }

    public void FadeToLevel()
    {
        _anim.SetTrigger("FadeOut");
    }
}
