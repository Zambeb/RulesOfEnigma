using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Spine.Unity;

public class WaterBossAnimator : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle;
    public AnimationReferenceAsset death;
    public string currentState;
    public string currentAnimation;
    
    // Start is called before the first frame update
    void Start()
    {
        Idle();
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

        else if (state.Equals("Death"))
        {
            SetAnimation(death, false, 1f);
        }
    }
}