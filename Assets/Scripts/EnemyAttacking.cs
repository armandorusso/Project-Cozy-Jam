using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    private Enemy _enemy;
    public GameObject Target { get; set; }

    [SerializeField] private int _damage;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _strikingMovementSpeed;
    [SerializeField] private float _strikingWaitTime;
    private bool _isStriking;
    private float _strikingCountdown;
    public bool IsStriking
    {
        get => _isStriking;
        set => _isStriking = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        var distanceFromTarget = Vector2.Distance(transform.position, Target.transform.position);
        if (distanceFromTarget <= _attackRange && _enemy.EnemyMovement.IsMoving && !_isStriking)
        {
            _enemy.EnemyMovement.IsMoving = false;
            StartCoroutine(Attack());
        }
        else
        {
            if (_strikingCountdown > 0.0f)
            {
                StrikeMove();
                _strikingCountdown -= Time.deltaTime;
            }
            else if (_strikingCountdown <= 0.0f && _isStriking)
            {
                StartCoroutine(RecoverMissedCharge());
            }
        }

    }

    private IEnumerator RecoverMissedCharge()
    {
        _enemy.EnemyAnimator.AnimatorComponent.transform.rotation = Quaternion.identity;
        _enemy.EnemyAnimator.SetSpriteDirection();
        _strikingCountdown = 0.0f;
        _isStriking = false;
        _enemy.EnemyAnimator.AnimatorComponent.SetTrigger("Recover");
        // Reset shadow again just in case
        if (_enemy.EnemyAnimator.ShadowSpriteRendererComponent)
        {
            _enemy.EnemyAnimator.ShadowSpriteRendererComponent.transform.rotation = Quaternion.identity;
        }
        yield return new WaitForSeconds(_strikingWaitTime);
        _enemy.EnemyMovement.IsMoving = true;
        _enemy.EnemyAnimator.AnimatorComponent.SetTrigger("Recover");
        if (_enemy.EnemyAnimator.ShadowSpriteRendererComponent)
        {
            _enemy.EnemyAnimator.ShadowSpriteRendererComponent.transform.rotation = Quaternion.identity;
        }
    }
    
    private void StrikeMove()
    {
        transform.Translate(_strikingMovementSpeed * Time.deltaTime, 0.0f, 0.0f);
    }

    private IEnumerator Attack()
    {
        _enemy.EnemyAnimator.AnimatorComponent.SetTrigger("Prepare");
        yield return new WaitForSeconds(_strikingWaitTime);
        _strikingCountdown = _strikingWaitTime;
        _isStriking = true;
        _enemy.EnemyAnimator.AnimatorComponent.SetTrigger("Charge");
        
        if (_enemy.EnemyMovement.EnemyTargetingType == EnemyMovement.EnemyType.PlayerMainAttacker)
        {
            var angle = Mathf.Atan2(_enemy.EnemyMovement.EnemyDirection.y, _enemy.EnemyMovement.EnemyDirection.x) * Mathf.Rad2Deg;
            _enemy.EnemyAnimator.SetSpriteDirectionWhenCharging(angle);
            if (_enemy.EnemyAnimator.ShadowSpriteRendererComponent)
            {
                _enemy.EnemyAnimator.ShadowSpriteRendererComponent.transform.rotation = Quaternion.identity;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == Target && !_enemy.EnemyMovement.IsMoving && _isStriking)
        {
            StartCoroutine(HurtTarget());
        }
    }

    private IEnumerator HurtTarget()
    {
        bool isHiveDead = false;
        if (Target.TryGetComponent<Player>(out var player))
        {
            player.PlayerHurt.GetHurt(_enemy.EnemyMovement.EnemyDirection);
        }
        else if (Target.TryGetComponent<Hive>(out var hive))
        {
            isHiveDead = hive.HiveHurt.GetHurt(_enemy.EnemyMovement.EnemyDirection, _damage);
        }

        if (isHiveDead) yield break;
        
        _isStriking = false;
        _enemy.EnemyAnimator.AnimatorComponent.SetTrigger("Recover");
        yield return new WaitForSeconds(_strikingWaitTime);
        _enemy.EnemyMovement.IsMoving = true;
        _strikingCountdown = _strikingWaitTime;
        _enemy.EnemyAnimator.AnimatorComponent.SetTrigger("Recover");
    }
}
