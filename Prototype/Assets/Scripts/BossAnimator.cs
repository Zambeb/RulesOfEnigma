using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Spine.Unity;

public class BossAnimator : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle;
    public AnimationReferenceAsset wake;
    public AnimationReferenceAsset death;
    public string currentState;
    public string currentAnimation;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void WakeUp()
    {
        currentState = "Wake";
        SetCharacterState(currentState);
    }

    public void Idle()
    {
        currentState = "Idle";
        SetCharacterState(currentState);
    }
    
    public void Death()
    {
        currentState = "Death";
        SetCharacterState(currentState);
    }
    
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        currentAnimation = animation.name;
    }

    public void SetCharacterState(string state)
    {
        if (state.Equals("Idle"))
        {
            SetAnimation(idle, true, 1f);
        }
        
        if (state.Equals("Wake"))
        {
            SetAnimation(wake, false, 2.5f);
        }
        
        if (state.Equals("Death"))
        {
            SetAnimation(death, false, 1f);
        }
    }
}
