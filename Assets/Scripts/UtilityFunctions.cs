using UnityEngine;

public class UtilityFunctions : MonoBehaviour
{
    public static bool IsAnimationPlaying(Animator anim, string animationName)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(animationName))
        {
            return true;
        }
        return false;
    }

    public static bool IsAnimationFinished(Animator anim, string animationName)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1f;
    }
}
