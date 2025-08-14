using UnityEngine;

public class SetFixedScale : StateMachineBehaviour
{
    public Vector3 fixedScale = Vector3.one;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.localScale = fixedScale;
    }
}
