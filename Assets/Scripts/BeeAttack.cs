using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAttack : MonoBehaviour
{
    [SerializeField] private LayerMask AttackLayer;

    public static Action<GameObject> KillRedEnemyAction;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & AttackLayer) != 0)
        {
            Debug.Log("Red Enemy Detected!");
            KillRedEnemyAction?.Invoke(col.gameObject);
        }
    }
}
