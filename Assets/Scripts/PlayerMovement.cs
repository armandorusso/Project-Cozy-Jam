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
    
    [SerializeField] private PlayerType _playerType;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _movementSpeed;
    private Vector2 _inputMovement;
    // Start is called before the first frame update
    private void Start()
    {
        if (!_rigidbody2D)
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        MovePlayer();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputMovement = context.ReadValue<Vector2>();
    }

    private void MovePlayer()
    {
        transform.Translate(_inputMovement * _movementSpeed * Time.deltaTime);
    }
}
