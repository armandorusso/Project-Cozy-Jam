using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour
{
    private Enemy _enemy;

    public static Action<GameObject> EnemyDeathAction;
    
    void Start()
    {
        BeeAttack.KillEnemyAction += OnEnemySwarmed;
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnemySwarmed(GameObject enemy)
    {
        if (gameObject == enemy && enemy.TryGetComponent<Enemy>(out var enemyComponent))
        {
            GameManager.Instance.CurrentEnemiesOnScene--;
            EnemyDeathAction?.Invoke(enemy);
            Destroy(enemyComponent.EnemyAnimator.AnimatorComponent.gameObject);
            Destroy(enemy);
        }
    }

    private void OnDestroy()
    {
        BeeAttack.KillEnemyAction -= OnEnemySwarmed;
    }
}
