using UnityEngine;

public class TransitionTwoBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InputManager.Instance.canReceiveAttackInput = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (InputManager.Instance.inputAttackReceived)
        {
            animator.SetTrigger("AttackThree");
            AudioManager.instance.PlaySFX(AudioManager.instance.attack3);
            InputManager.Instance.InputAttack();
            InputManager.Instance.inputAttackReceived = false;
        }
    }



}
