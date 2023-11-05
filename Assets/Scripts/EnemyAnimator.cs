using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Enemy _enemy;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    public Animator AnimatorComponent => _animator;

    private Transform _hornetAnimator;
    // Start is called before the first frame update
    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (_hornetAnimator)
        {
            _hornetAnimator.position = transform.position;
        }

        if (_spriteRenderer)
        {
            SetSpriteDirection();
        }
    }

    public void SetHornetAnimator(Transform hornetAnimator)
    {
        _hornetAnimator = hornetAnimator;
        _animator = _hornetAnimator.gameObject.GetComponent<Animator>();
        _spriteRenderer = _hornetAnimator.gameObject.GetComponent<SpriteRenderer>();
    }

    private void SetSpriteDirection()
    {
        if (_enemy.EnemyMovement.EnemyDirection.x < 0.0f)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }
}
