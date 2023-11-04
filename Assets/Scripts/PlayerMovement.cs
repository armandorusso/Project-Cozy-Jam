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
        _inputMovement = context.ReadValue<Vector2>();
    }

    private void MovePlayer()
    {
        transform.Translate(_inputMovement * (_movementSpeed * Time.deltaTime));
    }
}
