using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour
{
    private Enemy _enemy;
    void Start()
    {
        BeeAttack.KillRedEnemyAction += OnEnemySwarmed;
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnemySwarmed(GameObject enemy)
    {
        if (gameObject == enemy && enemy.TryGetComponent<Enemy>(out var enemyComponent))
        {
            GameManager.Instance.CurrentEnemiesOnScene--;
            Destroy(enemyComponent.EnemyAnimator.AnimatorComponent.gameObject);
            Destroy(enemy);
        }
    }

    private void OnDestroy()
    {
        BeeAttack.KillRedEnemyAction -= OnEnemySwarmed;
    }
}
