using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyType
    {
        HiveAttacker,
        PlayerMainAttacker,
        PlayerSecondaryAttacker
    }

    private Enemy _enemy;
    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _attackSpeed;

    private Transform _target;
    private Vector3 _enemyDirection;
    public EnemyType EnemyTargetingType => _enemyType;
    public Vector3 EnemyDirection => _enemyDirection;

    public bool IsMoving { get; set; }
    // Start is called before the first frame update
    private void Start()
    {
        IsMoving = true;
        _enemy = GetComponent<Enemy>();
        SelectTarget();
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsMoving)
        {
            RotateTowardsTarget();
            Move();
        }
    }

    private void SelectTarget()
    {
        _target = _enemyType switch
        {
            EnemyType.PlayerMainAttacker => GameManager.Instance.Players[0].transform,
            EnemyType.PlayerSecondaryAttacker => GameManager.Instance.Players[1].transform,
            EnemyType.HiveAttacker => GameManager.Instance.Hive.transform,
            _ => GameManager.Instance.Hive.transform
        };

        _enemy.EnemyAttacking.Target = _target.gameObject;
    }

    private void RotateTowardsTarget()
    {
        var distance = _target.transform.position - transform.position;
        _enemyDirection = distance.normalized;
        var angleToTarget = Mathf.Atan2(_enemyDirection.y, _enemyDirection.x) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.Euler(0.0f, 0.0f, angleToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
    }

    private void Move()
    {
        transform.Translate(_movementSpeed * Time.deltaTime, 0.0f, 0.0f);
    }
}
