using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : MonoBehaviour
{
    private Player _player;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _hurtTime;
    private float _hurtCountdown;

    public bool isHurt;
    // Start is called before the first frame update
    private void Start()
    {
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHurt) return;
        
        if (_hurtCountdown > 0)
        {
            _hurtCountdown -= Time.deltaTime;
            return;
        }
        
        if (_hurtCountdown <= 0 && !_player.PlayerMovement.IsMoving)
        {
            _hurtCountdown = 0.0f;
            isHurt = false;
            _player.PlayerMovement.IsMoving = true;
        }
    }

    public void GetHurt(Vector3 _enemyDirection)
    {
        isHurt = true;
        if (isHurt && _player.PlayerMovement.IsMoving)
        {
            _hurtCountdown = _hurtTime;
            _player.PlayerMovement.IsMoving = false;
            _rigidbody2D.AddForce(100f * _enemyDirection);
        }
    }
}
