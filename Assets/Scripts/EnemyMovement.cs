using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private enum EnemyType
    {
        HiveAttacker,
        PlayerAttacker
    }

    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _movementSpeed;
    
    //PLACE HOLDERS
    [SerializeField] private Transform _hive;
    [SerializeField] private Transform[] _players;

    private Transform _target;
    // Start is called before the first frame update
    void Start()
    {
        SelectTarget();
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsTarget();
        Move();
    }

    private void SelectTarget()
    {
        if (_enemyType == EnemyType.PlayerAttacker)
        {
            // TO DO: Make it pick randomly between the two players
            _target = _players[Random.Range(0, _players.Length)];
        }
        else if (_enemyType == EnemyType.HiveAttacker)
        {
            // Pick hive
            _target = _hive;
        }
    }

    private void RotateTowardsTarget()
    {
        var distance = _target.transform.position - transform.position;
        var direction = distance.normalized;
        var angleToTarget = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.Euler(0.0f, 0.0f, angleToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
    }

    private void Move()
    {
        transform.Translate(_movementSpeed * Time.deltaTime, 0.0f, 0.0f);
    }
}
