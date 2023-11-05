using System.Collections;
using System.Collections.Generic;
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
    private Vector2 _inputMovement;
    private Vector3 _playerDirection;
    public bool IsMoving;
    // Start is called before the first frame update
    private void Start()
    {
        _player = GetComponent<Player>();
        if (!_rigidbody2D)
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        _playerDirection = _playerType switch
        {
            PlayerType.Main => Vector3.left,
            PlayerType.Secondary => Vector3.right,
            _ => _playerDirection = Vector3.zero
        };
        
        if (_playerType == PlayerType.Main)
        {
            _player.SpriteRendererComponent.flipX = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsMoving)
        {
            MovePlayer();
        }
    }

    public void OnMoveUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_playerDirection.y == 1.0f)
            {
                _playerDirection.x = 0.0f;
            }
            _playerDirection.y = 1.0f;
            UpdateMovementDirectionSprites();
        }
    }
    
    public void OnMoveDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_playerDirection.y == -1.0f)
            {
                _playerDirection.x = 0.0f;
            }
            _playerDirection.y = -1.0f;
            UpdateMovementDirectionSprites();
        }
    }
    
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_playerDirection.x == -1.0f)
            {
                _playerDirection.y = 0.0f;
            }
            _playerDirection.x = -1.0f;
            UpdateMovementDirectionSprites();
        }
    }
    
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_playerDirection.x == 1.0f)
            {
                _playerDirection.y = 0.0f;
            }
            _playerDirection.x = 1.0f;
            UpdateMovementDirectionSprites();
        }
    }
    
    private void MovePlayer()
    {
        transform.Translate(_playerDirection * (_movementSpeed * Time.deltaTime));
    }
    
    public void UpdateMovementDirectionSprites()
    {
        if (!IsMoving) return;
        
        // Right
        if (_playerDirection is { x: 1.0f, y: 0.0f })
        {
            _player.AnimatorComponent.Play("RunSide");
            _player.SpriteRendererComponent.flipX = false;
        }
        // Left
        else if (_playerDirection is { x: -1.0f, y: 0.0f })
        {
            _player.AnimatorComponent.Play("RunSide");
            _player.SpriteRendererComponent.flipX = true;
        }
        // North
        else if (_playerDirection is { x: 0.0f, y: 1.0f })
        {
            _player.AnimatorComponent.Play("RunNorth");
        }
        // South
        else if (_playerDirection is { x: 0.0f, y: -1.0f })
        {
            _player.AnimatorComponent.Play("RunSouth");
        }
        // Right-Up
        else if (_playerDirection is { x: 1.0f, y: 1.0f })
        {
            _player.AnimatorComponent.Play("RunNorthSide");
            _player.SpriteRendererComponent.flipX = false;
        }
        // Left-Up
        else if (_playerDirection is { x: -1.0f, y: 1.0f })
        {
            _player.AnimatorComponent.Play("RunNorthSide");
            _player.SpriteRendererComponent.flipX = true;
        }
        // Right-Down
        else if (_playerDirection is { x: 1.0f, y: -1.0f })
        {
            _player.AnimatorComponent.Play("RunSouthSide");
            _player.SpriteRendererComponent.flipX = false;
        }
        // Left-Down
        else if (_playerDirection is { x: -1.0f, y: -1.0f })
        {
            _player.AnimatorComponent.Play("RunSouthSide");
            _player.SpriteRendererComponent.flipX = true;
        }
    }
}
