using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAttack : MonoBehaviour
{
    [SerializeField] private LayerMask AttackLayer;
    [SerializeField] public BeePoolScriptableObject BeePool;
    [SerializeField] public PolygonCollider2D TrailHitbox;
    [SerializeField] public Transform PlayerTransform;
    
    private Vector2 _hitboxPosition;
    private Transform newEnemyPosition;
    public List<GameObject> EnemiesInAttackZone;
    public List<BeeAlly> AttackingBees;
    private bool _isEnabled;
    private bool _isAttackCommenced;
    private int _startingBeeIndex;
    
    public static Action<GameObject, BeeCongoLine.BeeType> KillEnemyAction;

    private void Start()
    {
        BeeCongoLine.BeeAttackAction += OnBeeAttack;

        EnemiesInAttackZone = new List<GameObject>();

        AttackingBees = new List<BeeAlly>();
    }

    private void OnBeeAttack(BeeAlly beeHit, int poolId)
    {
        if (poolId == BeePool.GetInstanceID() && !_isAttackCommenced)
        {
            _isAttackCommenced = true;
            _startingBeeIndex = beeHit.Index;

            for (var i = _startingBeeIndex; i >= 0; i--)
            {
                AttackingBees.Add(BeePool.BeePool[i]);
            }

            CreateHitbox(AttackingBees);
        }
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
        TrailHitbox.pathCount = 0;
        TrailHitbox.enabled = false;

        if (EnemiesInAttackZone.Count == 0)
        {
            Debug.Log("No enemies detected");
            _isAttackCommenced = false;
            AttackingBees.Clear();
            StopCoroutine(DisableHitbox());
            yield break;
        }
        
        Debug.Log("Enemies detected! Swarm Attack!");
        
        CommenceAttack();
        BeePool.RemoveAttackingBeesFromPool(_startingBeeIndex, PlayerTransform);

        foreach (var enemy in EnemiesInAttackZone)
        {
            KillEnemyAction?.Invoke(enemy, BeePool.BeeType);
        }
        
        yield return new WaitForSeconds(1f);

        EnemiesInAttackZone.Clear();
        BeePool.MoveAttackingBeesToCongoLine(PlayerTransform);


        foreach (var bee in AttackingBees)
        {
            bee.OnAttackFinish(true);
        }

        BeePool.IsAttacking = false;
        AttackingBees.Clear();
        
        // Delay a bit before attacking again or else an attack can trigger while the bees are catching up to the congo line
        yield return new WaitForSeconds(0.1f);
        _isAttackCommenced = false;
    }

    private void CommenceAttack()
    {
        if (EnemiesInAttackZone.Count == 1)
        {
            foreach (var enemy in EnemiesInAttackZone)
            {
                foreach (var bee in AttackingBees)
                {
                    bee.MoveTowardsEnemyPosition(enemy.transform);
                    bee.OnBeeAttack(true);
                }
            }
        }
        else if (EnemiesInAttackZone.Count >= 2 && EnemiesInAttackZone.Count < AttackingBees.Count)
        {
            var index = 0;
            foreach (var enemy in EnemiesInAttackZone)
            {
                for(var i = 0; i < AttackingBees.Count / EnemiesInAttackZone.Count; i++)
                {
                    // Move bee towards enemy position
                    var bee = AttackingBees[index];
                    bee.MoveTowardsEnemyPosition(enemy.transform);
                    bee.OnBeeAttack(true);
                    index++;
                }
            }
        }
        else if(EnemiesInAttackZone.Count >= 2 && EnemiesInAttackZone.Count > AttackingBees.Count)
        {
            // Take average position of all enemies and command all bees to that position
            var midpoint = Vector3.zero;
            foreach (var enemy in EnemiesInAttackZone)
            {
                midpoint += enemy.transform.position;
            }

            var enemyLength = EnemiesInAttackZone.Count;
            midpoint /= enemyLength;
            newEnemyPosition = EnemiesInAttackZone[0].transform;
            newEnemyPosition.position = midpoint;
            
            foreach(var bee in AttackingBees)
            {
                // Move bees towards enemy position
                bee.MoveTowardsEnemyPosition(newEnemyPosition);
                bee.OnBeeAttack(true);
            }
        }
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
