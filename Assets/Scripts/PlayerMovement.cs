using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private enum PlayerType
    {
        Main,
        Secondary
    };

    private Player _player;
    [SerializeField] private PlayerType _playerType;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _acceleratedSpeed;
    [SerializeField] private Transform _followBeePoint;
    private float _currentSpeed;
    public Vector2 PlayerDirection;
    public bool IsMoving;
    
    private bool _isColliding;
    private float _collidingCountdown;
    
    // Start is called before the first frame update
    private void Start()
    {
        _player = GetComponent<Player>();
        if (!_rigidbody2D)
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        PlayerDirection = _playerType switch
        {
            PlayerType.Main => Vector3.left,
            PlayerType.Secondary => Vector3.right,
            _ => PlayerDirection = Vector3.zero
        };
        
        if (_playerType == PlayerType.Main)
        {
            _player.SpriteRendererComponent.flipX = true;
        }
        
        UpdateMovementDirectionSprites();
        _currentSpeed = _movementSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.DrawRay(_followBeePoint.position, PlayerDirection * 0.5f, Color.magenta);
        
        if (IsMoving)
        {
            MovePlayer();
        }

        if (_isColliding)
        {
            if (_collidingCountdown > 0.0f)
            {
                _collidingCountdown -= Time.deltaTime;
            }
            else if (_collidingCountdown <= 0.0f)
            {
                _collidingCountdown = 0.0f;
                _isColliding = false;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!IsMoving) return;
        
        if (context.performed && !_isColliding)
        {
            PlayerDirection = context.ReadValue<Vector2>();
            PlayerDirection.Normalize();
            UpdateMovementDirectionSprites();
            _currentSpeed = _acceleratedSpeed;
            _player.AnimatorComponent.speed = 1.6f;
        }
        else
        {
            _currentSpeed = _movementSpeed;
            _player.AnimatorComponent.speed = 1.0f;
        }

        if (_player.PlayerHurt.isHurt)
        {
            _player.AnimatorComponent.speed = 1.0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("BorderHorizontal"))
        {
            PlayerDirection.x = -PlayerDirection.x;
            ActivateBorderCollisionState();
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("BorderVertical"))
        {
            PlayerDirection.y = -PlayerDirection.y;
            ActivateBorderCollisionState();
        }
    }

    private void ActivateBorderCollisionState()
    {
        _isColliding = true;
        _collidingCountdown = 0.3f;
        UpdateMovementDirectionSprites();
    }

    private void MovePlayer()
    {
        transform.Translate(PlayerDirection * (_currentSpeed * Time.deltaTime));
    }
    
    public void UpdateMovementDirectionSprites()
    {
        if (!IsMoving) return;

        switch (PlayerDirection)
        {
            // Right
            case { x: <=1.0f, x: > 0.0f, y: 0.0f }:
                _player.AnimatorComponent.Play("RunSide");
                _player.SpriteRendererComponent.flipX = false;
                break;
            // Left
            case { x: >=-1.0f, x: < 0.0f, y: 0.0f }:
                _player.AnimatorComponent.Play("RunSide");
                _player.SpriteRendererComponent.flipX = true;
                break;
            // North
            case { x: 0.0f, y: <=1.0f, y: >0.0f }:
                _player.AnimatorComponent.Play("RunNorth");
                break;
            // South
            case { x: 0.0f, y: >=-1.0f, y: <0.0f }:
                _player.AnimatorComponent.Play("RunSouth");
                break;
            // Right-Up
            case { x: <=1.0f, x: > 0.0f, y: <=1.0f, y: > 0.0f }:
                _player.AnimatorComponent.Play("RunNorthSide");
                _player.SpriteRendererComponent.flipX = false;
                break;
            // Left-Up
            case { x: >=-1.0f, x: < 0.0f, y: <=1.0f, y: > 0.0f }:
                _player.AnimatorComponent.Play("RunNorthSide");
                _player.SpriteRendererComponent.flipX = true;
                break;
            // Right-Down
            case { x: <=1.0f, x: > 0.0f, y: >=-1.0f, y: < 0.0f }:
                _player.AnimatorComponent.Play("RunSouthSide");
                _player.SpriteRendererComponent.flipX = false;
                break;
            // Left-Down
            case { x: >=-1.0f, x: < 0.0f, y: >=-1.0f, y: < 0.0f }:
                _player.AnimatorComponent.Play("RunSouthSide");
                _player.SpriteRendererComponent.flipX = true;
                break;
        }
    }
}
