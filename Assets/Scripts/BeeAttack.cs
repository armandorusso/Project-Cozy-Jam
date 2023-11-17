using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAttack : MonoBehaviour
{
    [SerializeField] private LayerMask AttackLayer;
    [SerializeField] public BeePoolScriptableObject BeePool;
    [SerializeField] public PolygonCollider2D TrailHitbox;
    
    private Vector2 _hitboxPosition;
    public List<GameObject> EnemiesInAttackZone;
    public List<BeeAlly> AttackingBees;
    private bool _isEnabled;
    public static Action<GameObject> KillEnemyAction;

    private void Start()
    {
        BeeCongoLine.BeeAttackAction += OnBeeAttack;

        EnemiesInAttackZone = new List<GameObject>();
    }

    private void OnBeeAttack(BeeAlly beeHit)
    {
        var beeIndex = beeHit.Index;

        for(var i = beeIndex; i >= 0; i--)
        {
            AttackingBees.Add(BeePool.BeePool[i]);
        }
        
        CreateHitbox(AttackingBees);
    }

    private void CreateHitbox(List<BeeAlly> AttackingBees)
    {
        List<Vector2> points = new List<Vector2>();

        foreach(var bee in AttackingBees)
        {
            points.Add(bee.transform.position);
        }

        TrailHitbox.SetPath(0, points);
        TrailHitbox.enabled = true;

        AttackEnemies();
    }

    private void AttackEnemies()
    {
        StartCoroutine(DisableHitbox());
    }

    private IEnumerator DisableHitbox()
    {
        yield return new WaitForSeconds(0.4f);
        
        if (EnemiesInAttackZone.Count >= 0)
        {
            // KillEnemyAction?.Invoke(AttackingBees, EnemiesInAttackZone);
        }
        
        TrailHitbox.pathCount = 0;
        TrailHitbox.enabled = false;
    }

    private void DetectEnemiesInAttackZone(GameObject go)
    {
        if (go == null) return;
        
        if (((1 << go.layer) & AttackLayer) != 0 && !EnemiesInAttackZone.Contains(go))
        {
            EnemiesInAttackZone.Add(go);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        DetectEnemiesInAttackZone(col.gameObject);
    }

    private void OnDestroy()
    {
        BeeCongoLine.BeeAttackAction -= OnBeeAttack;
    }
}
