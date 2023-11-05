using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAttack : MonoBehaviour
{
    [SerializeField] private LayerMask AttackLayer;
    [SerializeField] private ParticleSystemForceField ForceFieldParticles;

    private Vector2 _hitboxPosition;
    private float _forceFieldRadius;
    private bool _isEnabled;
    public static Action<GameObject> KillEnemyAction;

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
        _forceFieldRadius = forceFieldRadius + 7.0f;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        StartCoroutine(DetectEnemiesInCollider(col.gameObject));
    }

    private IEnumerator DetectEnemiesInCollider(GameObject go)
    {
        if (go == null) yield break;
        
        if (((1 << go.layer) & AttackLayer) != 0)
        {
            ActivateForceField();
            yield return new WaitForSeconds(0.5f);
            KillEnemyAction?.Invoke(go);
        }
    }
    
    private void ActivateForceField()
    {
        ForceFieldParticles.enabled = true;
        ForceFieldParticles.transform.position = _hitboxPosition;
        ForceFieldParticles.endRange = _forceFieldRadius;
    }

    private void OnDestroy()
    {
        TrailIntersection.BeeSwarmAction -= OnBeeAttack;
    }
}
