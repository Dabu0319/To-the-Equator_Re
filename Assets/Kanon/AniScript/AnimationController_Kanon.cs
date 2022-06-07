using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController_Kanon : UnitySingleton<AnimationController_Kanon>
{
    private Animator m_Animator;
    public override void Awake()
    {
        base.Awake();
        m_Animator = GetComponent<Animator>();
    }


    public void PlayMoveAni(float f)
    {
        m_Animator.SetFloat("Horizontal",f);
    }
    
    public void PlayJumpAni()
    {
        m_Animator.SetTrigger("Jump");
    }
    
    // public void PlayLandAni()
    // {
    //     m_Animator.SetTrigger("Land");
    // }
    
    public void SetGrounded(bool b)
    {
        m_Animator.SetBool("IsGrounded",b);
    }
}
