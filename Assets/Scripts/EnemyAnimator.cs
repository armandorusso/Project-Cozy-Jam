using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Enemy _enemy;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _shadowSpriteRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private float SpriteZOffset;
    public Animator AnimatorComponent => _animator;
    public SpriteRenderer SpriteRendererComponent => _spriteRenderer;
    public SpriteRenderer ShadowSpriteRendererComponent => _shadowSpriteRenderer;

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
            _hornetAnimator.position = transform.position - new Vector3(0.0f, SpriteZOffset, 0.0f);
        }

        if (_spriteRenderer && !_enemy.EnemyAttacking.IsStriking)
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

    public void SetSpriteDirection()
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
    
    public void SetSpriteDirectionWhenCharging(float angle)
    {
        _animator.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle + 180);
        _spriteRenderer.flipX = _animator.transform.rotation.z is < 270.0f or > 90.0f;
    }
}
