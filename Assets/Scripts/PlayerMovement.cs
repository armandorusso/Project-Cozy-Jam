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

        IsMoving = true;

        _playerDirection = _playerType switch
        {
            PlayerType.Main => Vector3.left,
            PlayerType.Secondary => Vector3.right,
            _ => _playerDirection = Vector3.zero
        };
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsMoving)
        {
            MovePlayer();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>() != Vector2.zero)
        {
            _inputMovement = context.ReadValue<Vector2>();
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
        }
    }

    private void MovePlayer()
    {
        transform.Translate(_playerDirection * (_movementSpeed * Time.deltaTime));
    }
}
