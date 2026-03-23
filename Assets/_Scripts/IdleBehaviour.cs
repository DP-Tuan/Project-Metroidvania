using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (InputManager.Instance.inputAttackReceived)
        {
            animator.SetTrigger("AttackOne");
            AudioManager.instance.PlaySFX(AudioManager.instance.attack1);
            InputManager.Instance.InputAttack();
            InputManager.Instance.inputAttackReceived = false;
        }
    }



}
