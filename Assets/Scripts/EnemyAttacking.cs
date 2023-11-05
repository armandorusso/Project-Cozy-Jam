using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    private Enemy _enemy;
    public GameObject Target { get; set; }

    [SerializeField] private int _damage;
    [SerializeField] private float _attackRange;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        var distanceFromTarget = Vector2.Distance(transform.position, Target.transform.position);
        if (distanceFromTarget <= _attackRange && _enemy.EnemyMovement.IsMoving)
        {
            _enemy.EnemyMovement.IsMoving = false;
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        if (Target.TryGetComponent<Player>(out var player))
        {
            player.PlayerHurt.GetHurt(_enemy.EnemyMovement.EnemyDirection);
        }
        else if (Target.TryGetComponent<Hive>(out var hive))
        {
            hive.HiveHurt.GetHurt(_damage);
        }
        yield return new WaitForSeconds(1.0f);
        _enemy.EnemyMovement.IsMoving = true;
    }
}
