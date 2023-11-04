using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    private Enemy _enemy;
    public GameObject Target { get; set; }

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private int _hiveDamage;
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
        _spriteRenderer.color = Color.green;
        yield return new WaitForSeconds(1.0f);
        _spriteRenderer.color = Color.red;
        _enemy.EnemyMovement.IsMoving = true;
    }
}
