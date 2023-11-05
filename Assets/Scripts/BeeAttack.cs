using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAttack : MonoBehaviour
{
    [SerializeField] private LayerMask AttackLayer;
    [SerializeField] private ParticleSystemForceField ForceFieldParticles;

    public static Action<GameObject> KillRedEnemyAction;

    private void Start()
    {
        TrailIntersection.BeeSwarmAction += OnBeeAttack;
    }

    private void OnBeeAttack(bool isEnabled, Vector2 hitboxPosition, float forceFieldRadius)
    {
        ForceFieldParticles.enabled = isEnabled;
        ForceFieldParticles.transform.position = hitboxPosition;
        ForceFieldParticles.endRange = forceFieldRadius + 1.5f;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & AttackLayer) != 0)
        {
            Debug.Log("Red Enemy Detected!");
            KillRedEnemyAction?.Invoke(col.gameObject);
        }
    }
}
