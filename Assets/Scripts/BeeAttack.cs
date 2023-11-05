using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAttack : MonoBehaviour
{
    [SerializeField] private LayerMask AttackLayer;
    [SerializeField] private ParticleSystemForceField ForceFieldParticles;

    private bool _isEnemyInside;
    private Vector2 _hitboxPosition;
    private float _forceFieldRadius;
    private bool _isEnabled;
    public static Action<GameObject> KillRedEnemyAction;

    private void Start()
    {
        TrailIntersection.BeeSwarmAction += OnBeeAttack;
    }

    private void OnBeeAttack(bool isEnabled, Vector2 hitboxPosition, float forceFieldRadius)
    {
        if (!isEnabled)
        {
            ForceFieldParticles.enabled = false;
        }
        _hitboxPosition = hitboxPosition;
        _forceFieldRadius = forceFieldRadius + 1.5f;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & AttackLayer) != 0)
        {
            Debug.Log("Red Enemy Detected!");

            _isEnemyInside = true;
            
            ActivateForceField();
            
            KillRedEnemyAction?.Invoke(col.gameObject);
        }
    }

    private void ActivateForceField()
    {
        ForceFieldParticles.enabled = true;
        ForceFieldParticles.transform.position = _hitboxPosition;
        ForceFieldParticles.endRange = _forceFieldRadius;

        _isEnemyInside = false;
    }

    private void OnDestroy()
    {
        TrailIntersection.BeeSwarmAction -= OnBeeAttack;
    }
}
