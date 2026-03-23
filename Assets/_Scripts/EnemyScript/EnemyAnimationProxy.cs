using UnityEngine;

public class EnemyAnimationProxy : MonoBehaviour
{
    private EnemyController _enemyController;

    void Awake()
    {
        _enemyController = GetComponentInParent<EnemyController>();

        if (_enemyController == null)
        {
            Debug.LogError("EnemyAnimationProxy: Không t?m th?y EnemyController ? object cha!");
        }
    }

    private void Start()
    {
        _enemyController = GetComponentInParent<EnemyController>();
    }


    public void OnAttackEvent()
    {
        _enemyController.OnAttackEvent();
    }

    public void OnAttackFinished()
    {
        _enemyController.OnAttackFinished();
    }
    public void OnDeathAnimationFinished()
    {
        _enemyController.OnDeathAnimationFinished();
    }
    public void OnHurtFinished()
    {
        _enemyController.OnHurtFinished();
    }

}
