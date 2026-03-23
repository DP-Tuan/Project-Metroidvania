using UnityEngine;

public class RangedSkeAnimProxy : MonoBehaviour
{
    private RangedSkeBehaviour mainScript;

    private void Awake()
    {
        mainScript = GetComponentInParent<RangedSkeBehaviour>();

        if (mainScript == null)
        {
            Debug.LogError("RangedSkeAnimProxy: Không t?m th?y RangedSkeBehaviour ? object cha!");
        }
    }


    public void OnAttackEvent()
    {
        if (mainScript != null)
            mainScript.OnAttackEvent();
    }

    public void OnAttackFinished()
    {
        Debug.Log("On AttackFinished");
        if (mainScript != null)
        {
            Debug.Log("Enter Finished");
            mainScript.OnAttackFinished();
        }
    }

    public void OnHurtFinished()
    {
        if (mainScript != null)
            mainScript.OnHurtFinished();
    }

    public void OnDeathAnimationFinished()
    {
        if (mainScript != null)
            mainScript.OnDeathAnimationFinished();
    }
}