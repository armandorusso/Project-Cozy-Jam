using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyMovement _enemyMovement;
    [SerializeField] private EnemyAttacking _enemyAttacking;

    public EnemyMovement EnemyMovement => _enemyMovement;
    public EnemyAttacking EnemyAttacking => _enemyAttacking;
    
    // Start is called before the first frame update
    private void Start()
    {
        if (!_enemyMovement) _enemyMovement = GetComponent<EnemyMovement>();
        if (!_enemyAttacking) _enemyAttacking = GetComponent<EnemyAttacking>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
