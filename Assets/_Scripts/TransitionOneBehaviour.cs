using UnityEngine;

public class TransitionOneBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InputManager.Instance.canReceiveAttackInput = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (InputManager.Instance.inputAttackReceived)
        {
            animator.SetTrigger("AttackTwo");
            AudioManager.instance.PlaySFX(AudioManager.instance.attack2);
            InputManager.Instance.InputAttack();
            InputManager.Instance.inputAttackReceived = false;
        }
    }



}
