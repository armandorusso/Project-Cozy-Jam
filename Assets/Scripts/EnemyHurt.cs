using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour
{
    void Start()
    {
        BeeAttack.KillRedEnemyAction += OnEnemySwarmed;
    }

    private void OnEnemySwarmed(GameObject enemy)
    {
        if (gameObject == enemy)
        {
            Destroy(enemy);
        }
    }

    private void OnDestroy()
    {
        BeeAttack.KillRedEnemyAction -= OnEnemySwarmed;
    }
}
